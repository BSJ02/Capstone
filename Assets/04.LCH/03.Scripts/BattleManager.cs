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

    public BattleState battleState;

    [Header("# 플레이어 및 몬스터")]
    public GameObject player;
    private PlayerData playerData;
    public List<GameObject> monsters = new List<GameObject>();

    [Header("# 몬스터 버프")]
    public float damage = 5f;
    // 추가 예정

    private int currentMonsterIndex = -1;
    private float delay = 1.5f;

    public bool isPlayerMove = false;
    public bool isPlayerTurn = false;

    [Header("# UI")]
    public GameObject[] ui; // 턴 UI
    public Button turnEnd; // Turn End 버튼

    private CardManager cardManager;

    public bool isRandomCard = false;


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
        battleState = BattleState.Start;

        player.gameObject.SetActive(true);
        foreach (GameObject monster in monsters)
        {
            monster.gameObject.SetActive(true);
            Debug.Log("현재 몬스터:" + monster.name);
        }

        MapGenerator.instance.CreateMap(MapGenerator.instance.garo, MapGenerator.instance.sero);

        cardManager = FindObjectOfType<CardManager>();

        PlayerTurn();
    }

    
    public void PlayerTurn()
    {
        cardManager.CreateRandomCard();

        battleState = BattleState.PlayerTurn;
        ui[0].gameObject.SetActive(true);
        ui[0].gameObject.GetComponent<Animator>().Play("PlayerTurn", -1, 0f);
        turnEnd.interactable = true;
    }


    public void MonsterTurn()
    {
        isPlayerTurn = false;
        battleState = BattleState.MonsterTurn;
        ui[1].gameObject.SetActive(true);
        ui[1].gameObject.GetComponent<Animator>().Play("MonsterTurn", -1, 0f);
        turnEnd.interactable = false;  
        StartCoroutine(NextMonster());
    }


    IEnumerator NextMonster()
    {
        yield return new WaitForSeconds(delay);

        if (currentMonsterIndex < monsters.Count - 1)
        {
            currentMonsterIndex++;
            monsters[currentMonsterIndex].GetComponent<MonsterMove>().MoveStart();
            
            // 몬스터 1차 순회
            if (currentMonsterIndex == monsters.Count - 1)
            {
                /* 몬스터 버프 시스템 적용 예정
                for (int i = 0; i < monsters.Count - 1; i++)
                {
                    
                }*/
                // 초기화 
                currentMonsterIndex = -1;
            }
        }
    }
}

