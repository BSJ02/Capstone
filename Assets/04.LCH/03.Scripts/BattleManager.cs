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

    [Header("# 몬스터 버프")] 
    public float damage;
    public float heal;
    public float amor;

    [Header("# UI")]
    public GameObject[] turn_UI; // 턴 UI
    public GameObject buff_UI;
    public Button turnEnd_Btn; // Turn End 버튼

    private int currentMonsterIndex = -1;
    private float delay = 1.5f;

    [HideInInspector] public bool isPlayerMove = false;
    [HideInInspector] public bool isPlayerTurn = false;
    [HideInInspector] public bool isRandomCard = false;

    // Player Buff & DeBuff
    [HideInInspector] public int IsHealing = 0;
    [HideInInspector] public int IsPoisoned = 0;
    [HideInInspector] public int IsBurned = 0;
    [HideInInspector] public int IsBleeding = 0;

    private CardData cardData;
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
        cardData = FindObjectOfType<CardData>();
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
        turn_UI[0].gameObject.SetActive(false);
        turn_UI[1].gameObject.SetActive(true);
        turn_UI[1].gameObject.GetComponent<Animator>().Play("MonsterTurn", -1, 0f);
        turnEnd_Btn.interactable = false;  
        StartCoroutine(NextMonster());
        foreach (GameObject player in players)
        {
            player.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    IEnumerator NextMonster()
    {
        yield return new WaitForSeconds(delay);

        if (currentMonsterIndex < monsters.Count - 1)
        {
            currentMonsterIndex++;
            monsters[currentMonsterIndex].GetComponent<MonsterMove>().MoveStart();

            // 몬스터 순회 완료
            if (currentMonsterIndex == monsters.Count - 1)
            {
                /*// 몬스터 버프
                for (int i = 0; i < monsters.Count; i++)
                {
                    buff_UI.gameObject.SetActive(true);
                    buff_UI.GetComponent<Animator>().Play("Buff", -1, 0f);
                    monsters[i].GetComponent<Monster>().monsterData.IncreaseDamage(damage); // 원하는 스탯을 랜덤하게 뽑기
                }*/
                // 초기화 
                currentMonsterIndex = -1;
            }
        }
    }

}

