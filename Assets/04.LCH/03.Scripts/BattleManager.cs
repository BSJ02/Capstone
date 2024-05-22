using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState 
{
    Start,
    PlayerTurn,
    MonsterTurn,
    Won,
    Lost,
    EndStage
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    private CardManager cardManager;

    public BattleState battleState;

    [Header("# 스테이지 몬스터 및 플레이어")]
    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> monsters = new List<GameObject>();
    private Player playerScripts;

    // 랜덤으로 선택된 몬스터를 HashSet에 저장
    HashSet<int> selectedMonsters = new HashSet<int>();

    [Header("# 몬스터 버프")] 
    public float damage;
    public float heal;
    public float amor;

    [Header("# UI")]
    public GameObject[] turn_UI; // 턴 UI
    private GameObject buff_UI;
    public Button turnEnd_Btn; // Turn End 버튼
    public GameObject stageEnd; // StageEnd 이미지

    public int MaximumOfMonster = 3; // 선택된 몬스터 마릿수
    private float delay = 1.5f;

    bool stageClear = false;

    [HideInInspector] public bool isPlayerMove = false;
    [HideInInspector] public bool isPlayerTurn = false;
    [HideInInspector] public bool isRandomCard = false;

    // Player Buff & DeBuff
    [HideInInspector] public int IsHealing = 0;
    [HideInInspector] public int IsPoisoned = 0;
    [HideInInspector] public int IsBurned = 0;
    [HideInInspector] public int IsBleeding = 0;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        //players = characterSelector.playerSelectList.players;

        battleState = BattleState.Start;

        // 스테이지 오브젝트 활성화
        foreach (GameObject player in players)
        {
            //Instantiate(player, new Vector3(2, 0.35f, 0), Quaternion.identity);
            player.gameObject.SetActive(true);
            playerScripts = player.GetComponent<Player>();
        }
        foreach (GameObject monster in monsters)
        {
            monster.gameObject.SetActive(true);
        }

        // 맵 생성
        MapGenerator.instance.CreateMap(MapGenerator.instance.garo, MapGenerator.instance.sero);

        PlayerTurn();
    }
    
    // 스테이지 종료 감지
    private void Update()
    {
        // 모든 몬스터가 죽으면 스테이지 종료
        if(monsters.Count <= 0)
        {
            StartCoroutine(EndStage());
        }
    }


    IEnumerator EndStage()
    {
        // Stage 종료가 종료될 때 실행되는 것들(사운드, UI, 페이드 인 & 아웃 애니메이션, 다음 씬 이동 등..)
        yield return new WaitForSeconds(delay);

        battleState = BattleState.EndStage;
        stageClear = true;

        stageEnd.SetActive(true);

        // 씬 로드
        yield return new WaitForSeconds(delay * 3);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);

    }

    public void PlayerTurn()
    {
        battleState = BattleState.PlayerTurn;
        isPlayerTurn = true;

        foreach (GameObject player in /*characterSelector.playerSelectList.*/players)
        {
            playerScripts = player.GetComponent<Player>();
            playerScripts.ResetActivePoint();
            playerScripts.isAttack = false;
        }
        if (cardManager.handCardCount < 8)
        {
            cardManager.CreateRandomCard();
        }
        else
        {
            Debug.Log("카드가 너무 많음");
        }
        isPlayerTurn = true;
        battleState = BattleState.PlayerTurn;
        turn_UI[0].gameObject.SetActive(true);
        turn_UI[0].gameObject.GetComponent<Animator>().Play("PlayerTurn", -1, 0f);
        turnEnd_Btn.interactable = true;
        
        foreach (GameObject player in players)
        {
            player.gameObject.layer = LayerMask.NameToLayer("Player");
        }

    }

    public void MonsterTurn()
    {
        battleState = BattleState.MonsterTurn;
        isPlayerTurn = false;
        
        // 플레이어 턴 UI 비활성화
        turn_UI[0].gameObject.SetActive(false);
        foreach (GameObject player in players)
        {
            player.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        } // 레이캐스트 비활성화

        // 몬스터 턴 UI 활성화
        turn_UI[1].gameObject.SetActive(true);
        turn_UI[1].gameObject.GetComponent<Animator>().Play("MonsterTurn", -1, 0f);
        turnEnd_Btn.interactable = false;  

        StartCoroutine(NextMonster());

    }

    IEnumerator NextMonster()
    {
        // 지정된 delay 시간 동안 대기(MonsterTurn UI 재생 때문에)
        yield return new WaitForSeconds(delay);

        // 선택되지 않은 몬스터 리스트 초기화
        List<int> availableMonsters = new List<int>();
        for (int i = 0; i < monsters.Count; i++)
        {
            if (!selectedMonsters.Contains(i))
            {
                availableMonsters.Add(i);
            }
        }

        // MaximumOfMonster에 설정되어 있는 값 만큼 몬스터 움직이기
        for (int i = 0; i < MaximumOfMonster; i++)
        {
            // 선택되지 않은 몬스터 중 랜덤하게 선택
            int randIndex = Random.Range(0, availableMonsters.Count);
            int selectedIndex = availableMonsters[randIndex];
            GameObject selectedMonster = monsters[selectedIndex];

            // 선택된 몬스터가 이미 움직였는지 확인
            if (!selectedMonsters.Contains(selectedIndex))
            {
                // 선택된 몬스터의 특정 메서드 실행
                MonsterMove monsterMove = selectedMonster.GetComponent<MonsterMove>();
                IEnumerator detectionCoroutine = monsterMove.StartDetection();
                yield return StartCoroutine(detectionCoroutine);

                selectedMonsters.Add(selectedIndex);

                // 선택된 몬스터 추가 및 스킬을 쓰는 동안 대기
                while (selectedMonster.GetComponent<Monster>().attack == AttackState.SkillAttack)
                    yield return null;

                // 각 몬스터 이동 후 delay 만큼 대기
                yield return new WaitForSeconds(delay);
            }
            else
            {
                // 이미 선택된 몬스터는 스킵
                Debug.Log("이미 선택된 몬스터입니다.");
            }
        }

        // 선택된 몬스터들 초기화(버그 예방 차원) 
        selectedMonsters.Clear();

        // 모든 몬스터 행동 종료 후 턴 넘기기
        StartCoroutine(EscapeMonsterTurn());
    }

    IEnumerator EscapeMonsterTurn()
    {
        // 대기 처리
        yield return new WaitForSeconds(3f);
        turn_UI[1].gameObject.SetActive(false);
        PlayerTurn();
    }

}

