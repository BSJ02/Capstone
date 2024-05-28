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
    public List<int> playerList = new List<int>();
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
    public GameObject[] controlAllUI; // 스테이지 클리어 후 모든 UI 끄기
    public GameObject selectedMonster;

    public int MaximumOfMonster = 3; // 선택된 몬스터 마릿수
    private float delay = 1.5f;

    bool isEnd;

    [HideInInspector] public GameObject monsterObj = null;
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
        //SoundManager.instance.PlayBackgroundMusic("BGM");

        cardManager = FindObjectOfType<CardManager>();
        //players = characterSelector.playerSelectList.players;
        CharacterSelector characterSelector = FindObjectOfType<CharacterSelector>();

        battleState = BattleState.Start;


        // 스테이지 오브젝트 활성화
        foreach (int playerIndex in characterSelector.playerSelectList.playerList)
        {
            switch (playerIndex)
            {
                case 0:
                    players[0].gameObject.SetActive(true);
                    playerScripts = players[0].GetComponent<Player>();
                    break;
                case 1:
                    players[1].gameObject.SetActive(true);
                    playerScripts = players[1].GetComponent<Player>();
                    break;
                case 2:
                    players[2].gameObject.SetActive(true);
                    playerScripts = players[2].GetComponent<Player>();
                    break;
                case 3:
                    players[3].gameObject.SetActive(true);
                    playerScripts = players[3].GetComponent<Player>();
                    break;
            }
        }


        // 비활성화된 오브젝트 삭제
        for (int i = players.Count - 1; i >= 0; i--)
        {
            if (!players[i].activeSelf)
            {
                Destroy(players[i]);
                players.RemoveAt(i);
            }
        }

        /*        for (int i = 0; i < 2; i++)
                {
                    Instantiate(players[i], new Vector3(2 + i, 0.35f, 0), Quaternion.identity);
                    players[i].gameObject.SetActive(true);
                    playerScripts = players[i].GetComponent<Player>();
                }*/

        // 스테이지 오브젝트 활성화
        /*        foreach (GameObject player in players)
                {
                    player.gameObject.SetActive(true);
                    playerScripts = player.GetComponent<Player>();
                }*/

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
        if (monsters.Count <= 0 && !isEnd)
        {
            StartCoroutine(EndStage());
            isEnd = true;
        }
    }


    IEnumerator EndStage()
    {
        // Stage 종료가 종료될 때 실행되는 것들(사운드, UI, 페이드 인 & 아웃 애니메이션, 다음 씬 이동 등..)
        yield return new WaitForSeconds(delay);

        battleState = BattleState.EndStage;

        // UI 오브젝트 제어
        CameraController.instance.startGame = true;
        stageEnd.SetActive(true);
        foreach (var ui in controlAllUI)
        {
            ui.SetActive(false);
        }
        GameObject.Find("Deck").SetActive(false);
        GameObject.Find("PanelObject_Group").SetActive(false);

        // 씬 로드
        yield return new WaitForSeconds(delay * 3);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);

    }

    public void PlayerTurn()
    {
        battleState = BattleState.PlayerTurn;
        isPlayerTurn = true;

        StartCoroutine(StartPlayerTurn());

        foreach (GameObject player in players)
        {
            player.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    IEnumerator StartPlayerTurn()
    {
        yield return StartCoroutine(CameraController.instance.StartCameraMoving());

        turn_UI[0].gameObject.SetActive(true);
        turn_UI[0].gameObject.GetComponent<Animator>().Play("PlayerTurn", -1, 0f);
        turnEnd_Btn.interactable = true;

        turn_UI[0].gameObject.SetActive(false);

        foreach (GameObject player in /*characterSelector.playerSelectList.*/players)
        {
            playerScripts = player.GetComponent<Player>();
            playerScripts.ResetActivePoint();
            playerScripts.isAttack = false;
        }

        if (CameraController.instance.startGame)
        {
            CardManager.instance.StartSettingCards();
            CameraController.instance.startGame = false;
        }

        if (cardManager.handCardCount < 8)
        {
            cardManager.CreateRandomCard();
        }
        else
        {
            Debug.Log("카드가 너무 많음");
        }

        foreach (GameObject player in players)
        {
            player.gameObject.layer = LayerMask.NameToLayer("Player");
        }

        yield return null;
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
            selectedMonster = monsters[selectedIndex];

            //// 몬스터 중 랜덤하게 선택(중복 가능)
            //int randIndex = Random.Range(0, monsters.Count - 1);
            //GameObject selectedMonster = monsters[randIndex]; // 랜덤하게 뽑힌 몬스터


            // 선택된 몬스터가 이미 움직였는지 확인
            if (!selectedMonsters.Contains(selectedIndex))
            {
                yield return new WaitForSeconds(FadeController.instance.totalFadeDuration + 1f);

                // 선택된 몬스터의 특정 메서드 실행

                MonsterMove monsterMove = selectedMonster.GetComponent<MonsterMove>();
                IEnumerator detectionCoroutine = monsterMove.StartDetection();
                yield return StartCoroutine(detectionCoroutine);

                selectedMonsters.Add(selectedIndex);
                // 중복 방지용 HastSet(중복 방지 시 사용 예정)
                selectedMonsters.Add(randIndex);

                // 선택된 몬스터 추가 및 스킬을 쓰는 동안 대기
                while (selectedMonster.GetComponent<Monster>().attack == AttackState.SkillAttack)
                    yield return null;
                // 선택된 몬스터 스킬을 쓰는 동안 대기
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
            // 각 몬스터 이동 후 delay 만큼 대기
            yield return new WaitForSeconds(delay);
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

