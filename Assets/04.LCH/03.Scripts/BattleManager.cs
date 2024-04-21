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

    private Player playerScript;
    private CardManager cardManager;
    private PlayerData playerData;

    public BattleState battleState;

    [Header("# 플레이어 및 몬스터")]
    public GameObject player;
    public List<GameObject> monsters = new List<GameObject>();

    private int currentMonsterIndex = -1;
    private float delay = 1.5f;

    public bool isPlayerMove = false;
    public bool isPlayerTurn = false;

    [Header("# UI")]
    public GameObject[] ui;
    public Button turnEnd;


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
        cardManager = FindObjectOfType<CardManager>();
        playerScript = FindObjectOfType<Player> ();

        battleState = BattleState.Start;

        player.gameObject.SetActive(true);
        foreach (GameObject monster in monsters)
        {
            monster.gameObject.SetActive(true);
        }


        MapGenerator.instance.CreateMap(MapGenerator.instance.garo, MapGenerator.instance.sero);


        MonsterTurn();
    }


    public void PlayerTurn()
    {
        cardManager.CreateRandomCard();
        isPlayerTurn = true;

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


            if (currentMonsterIndex == monsters.Count - 1)
            {
                /*monsters[currentMonsterIndex].GetComponent<MonsterData>().IncreaseDamage(1);*/
                currentMonsterIndex = -1;
            }
        }
    }
}

