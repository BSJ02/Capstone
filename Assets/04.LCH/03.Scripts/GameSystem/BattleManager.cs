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
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    private CardManager cardManager;

    public BattleState battleState;

    [Header("# 스테이지 몬스터 및 플레이어")]
    public List<GameObject> players = new List<GameObject>();
    [HideInInspector] public List<int> playerList = new List<int>();
    public List<GameObject> monsters = new List<GameObject>();
    [HideInInspector] public GameObject selectedMonster;
    private Player playerScripts;

    [Header("# 행동 할 몬스터 개수")]
    public int MaximumOfMonster = 3; // 선택된 몬스터 마릿수

    // 랜덤으로 선택된 몬스터를 HashSet에 저장
    HashSet<int> selectedMonsters = new HashSet<int>();

    [Header("# UI")]
    public GameObject[] turn_UI; // 턴 UI
    public Button turnEnd_Btn; // Turn End 버튼
    public GameObject stageEnd; // StageEnd 이미지
    public GameObject[] controlAllUI; // 스테이지 클리어 후 모든 UI 끄기

  
    private float delay = 1.5f;
    [HideInInspector] public static int turncount = 1;

    bool isEnd;

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
        SoundManager.instance.PlayFightBackgroundMusic("Fight");

        cardManager = FindObjectOfType<CardManager>();
        // players = characterSelector.playerSelectList.players;
        CharacterSelector characterSelector = FindObjectOfType<CharacterSelector>();

        battleState = BattleState.Start;


        //스테이지 오브젝트 활성화
        //1 = Warrior(HP) / 2 = Warrior(ATK) / 3 = Wizard / 4 = Archer
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

        //비활성화된 오브젝트 삭제
        for (int i = players.Count - 1; i >= 0; i--)
        {
            if (!players[i].activeSelf)
            {
                Destroy(players[i]);
                players.RemoveAt(i);
            }
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
        if (monsters.Count <= 0 && !isEnd)
        {
            StartCoroutine(EndStage());
            isEnd = true;
        }
        else if(players.Count <= 0 && !isEnd)
        {
            StartCoroutine(EndStage());
            isEnd = true;
        }
    }

    // 스테이지 종료 시 실행
    IEnumerator EndStage()
    {
        // Stage 종료가 종료될 때 실행되는 것들(사운드, UI, 페이드 인 & 아웃 애니메이션, 다음 씬 이동 등..)
        yield return new WaitForSeconds(delay);

        battleState = BattleState.Won;

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
        CardManager.instance.isCardButtonClicked = false;

        StartCoroutine(StartPlayerTurn());

        foreach (GameObject player in players)
        {
            player.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    IEnumerator PlayerTurnUI()
    {
        turn_UI[0].gameObject.SetActive(true);
        turn_UI[0].gameObject.GetComponent<Animator>().Play("PlayerTurn", -1, 0f);
        turnEnd_Btn.interactable = true;
        yield return null;
    }

    IEnumerator StartPlayerTurn()
    {
        selectedMonster = null;
        CameraController.instance.ZoomCamera(false);

        if (turncount == 1)
        {
            yield return StartCoroutine(CameraController.instance.StartCameraMoving());
        }

        yield return StartCoroutine(PlayerTurnUI());

        foreach (GameObject player in players)
        {
            playerScripts = player.GetComponent<Player>();
            playerScripts.ResetActivePoint();
            playerScripts.isAttack = false;
        }

        yield return StartCoroutine(StatusEffectManager.instance.PlayerTurnSimulation());

        if (CameraController.instance.startGame)
        {
            CardManager.instance.StartSettingCards();
            CameraController.instance.startGame = false;
        }
        else
        {
            if (CardManager.instance.handCardList.Count < 7)
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
        }

        yield return null;
    }


    // 몬스터 턴 UI 제어
    public void MonsterTurn()
    {
        battleState = BattleState.MonsterTurn;
        isPlayerTurn = false;
        
        // 플레이어 턴 UI 비활성화
        turn_UI[0].gameObject.SetActive(false);

        // 레이캐스트 비활성화
        foreach (GameObject player in players)
        {
            player.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); 
        } 

        // 몬스터 턴 UI 활성화
        turn_UI[1].gameObject.SetActive(true);
        turn_UI[1].gameObject.GetComponent<Animator>().Play("MonsterTurn", -1, 0f);
        turnEnd_Btn.interactable = false;  

        StartCoroutine(SelectRandomMonster());
    }


    // 랜덤 몬스터 선택
    IEnumerator SelectRandomMonster()
    {
        // 지정된 delay 시간 동안 대기(MonsterTurn UI 재생 딜레이)
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
        for (int i = 0; i < MaximumOfMonster && availableMonsters.Count > 0; i++)
        {
            // 선택되지 않은 몬스터 중 랜덤하게 선택(중복 가능)
            int randIndex = Random.Range(0, availableMonsters.Count);
            int selectedIndex = availableMonsters[randIndex];

            selectedMonster = monsters[selectedIndex];

            // 선택된 몬스터가 이미 움직였는지 확인
            if (!selectedMonsters.Contains(selectedIndex))
            {
                yield return new WaitForSeconds(FadeController.instance.totalFadeDuration + 0.7f);
                yield return StartCoroutine(StatusEffectManager.instance.MonsterTurnSimulation(selectedMonster));

                // 선택된 몬스터의 행동 실행
                Debug.Log("선택된 몬스터:" + selectedMonster.name);
                MonsterMove monsterMove = selectedMonster.GetComponent<MonsterMove>();
                IEnumerator detectionCoroutine = monsterMove.StartDetection(); // 선택된 몬스터가 움직이기 시작
                yield return StartCoroutine(detectionCoroutine);

                selectedMonsters.Add(selectedIndex); // 선택된 몬스터 중복 방지

                // 선택된 몬스터 추가 및 스킬을 쓰는 동안 대기
                while (selectedMonster.GetComponent<Monster>().attack == AttackState.SkillAttack
                    && selectedMonster.GetComponent<Monster>().state == MonsterState.Skill)
                    yield return null;

                // 각 몬스터 이동 후 delay 만큼 대기
                yield return new WaitForSeconds(delay);
            }

            // 선택된 몬스터를 사용한 후에는 사용한 몬스터를 목록에서 제거하여 중복 방지
            availableMonsters.RemoveAt(randIndex);
        }
        // 선택된 몬스터들 초기화
        selectedMonsters.Clear();

        // 모든 몬스터 행동 종료 후 턴 넘기기
        StartCoroutine(EscapeMonsterTurn());
    }

    IEnumerator EscapeMonsterTurn()
    {
        // 대기 처리
        yield return new WaitForSeconds(3f);
     
        turn_UI[1].gameObject.SetActive(false);
        turncount += 1;

        PlayerTurn();
    }

}

