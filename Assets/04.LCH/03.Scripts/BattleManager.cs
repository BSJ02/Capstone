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

    public BattleState battleState;

    [Header("# 스테이지 몬스터 및 플레이어")]
    public GameObject player;
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
        playerScripts = player.GetComponent<Player>();
        cardManager = FindObjectOfType<CardManager>();

        battleState = BattleState.Start;

        // 스테이지 오브젝트 활성화
        player.gameObject.SetActive(true);
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
        isPlayerTurn = true;
        playerScripts.ResetActivePoint();
        cardManager.CreateRandomCard();
        isPlayerTurn = true;
        battleState = BattleState.PlayerTurn;
        turn_UI[0].gameObject.SetActive(true);
        turn_UI[0].gameObject.GetComponent<Animator>().Play("PlayerTurn", -1, 0f);
        turnEnd_Btn.interactable = true;
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }


    public void MonsterTurn()
    {
        isPlayerTurn = false;
        battleState = BattleState.MonsterTurn;
        turn_UI[1].gameObject.SetActive(true);
        turn_UI[1].gameObject.GetComponent<Animator>().Play("MonsterTurn", -1, 0f);
        turnEnd_Btn.interactable = false;  
        StartCoroutine(NextMonster());
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
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
                // 몬스터 버프
                for (int i = 0; i < monsters.Count; i++)
                {
                    buff_UI.gameObject.SetActive(true);
                    buff_UI.GetComponent<Animator>().Play("Buff", -1, 0f);
                    monsters[i].GetComponent<Monster>().monsterData.IncreaseDamage(damage);
                    Debug.Log(monsters[i].name + "의 스탯이 증가하였습니다.");
                }
                // 초기화 
                currentMonsterIndex = -1;
            }
        }
    }
}

