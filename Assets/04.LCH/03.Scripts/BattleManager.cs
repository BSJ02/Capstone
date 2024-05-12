using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState 
{
    Start,
    PlayerTurn,
    MonsterTurn,
    Won,
    Lost 
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private CardManager cardManager;

    private CharacterSelector characterSelector;

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

    public int MaximumOfMonster = 3; // 선택된 몬스터 마릿수
    private float delay = 1.5f;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        CardData cardData = FindObjectOfType<CardData>();
        cardManager = FindObjectOfType<CardManager>();
        characterSelector = FindObjectOfType<CharacterSelector>();
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

    
    public void PlayerTurn()
    {
        battleState = BattleState.PlayerTurn;
        isPlayerTurn = true;
        foreach (GameObject player in /*characterSelector.playerSelectList.*/players)
        {
            playerScripts = player.GetComponent<Player>();
            playerScripts.ResetActivePoint();
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
        } // 레이캐스트 비활성호 

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

        // MaximumOfMonster에 설정되어 있는 값 만큼 몬스터 움직이기
        for (int i = 0; i < MaximumOfMonster; i++)
        {
            int randValue = Random.Range(0, monsters.Count);

            // 중복된 몬스터 설정(!몬스터가 두 번 움직이는 것도 가능함. 따라서, 이 부분은 확실히 정해야함)
            while (selectedMonsters.Contains(randValue))
            {
                randValue = Random.Range(0, monsters.Count);
            }
            // HashSet에 랜덤 몬스터 추가
            selectedMonsters.Add(randValue);

            // 몬스터[randValue]의 StartDetection() 코루틴 실행
            MonsterMove monsterMove = monsters[randValue].GetComponent<MonsterMove>();
            IEnumerator detectionCoroutine = monsterMove.StartDetection();
            yield return StartCoroutine(detectionCoroutine);

            // 스킬을 쓰는 동안 다음 몬스터로 넘어가지 않도록 방지
            while (monsters[randValue].GetComponent<Monster>().attack == AttackState.SkillAttack)
                yield return null;
            // 각 몬스터 이동 후 delay 만큼 대기
            yield return new WaitForSeconds(delay);
        }
        // HashSet에 선택되었던 몬스터들 초기화(버그 예방 차원) 
        selectedMonsters.Clear();
        // 모든 몬스터 행동 종료 후 턴 넘기기
        StartCoroutine(EscapeMonsterTurn());
    }

    IEnumerator EscapeMonsterTurn()
    {
        // 대기 처리
        yield return new WaitForSeconds(3f);
        BattleManager.instance.turn_UI[1].gameObject.SetActive(false);
        BattleManager.instance.PlayerTurn();

    }

}

