using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState 
{
    Start,
    PlayerTurn,
    MonsterTurn,
    Won, // 다음 스테이지 씬 로드 
    Lost // 패배 시 UI 띄움
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public BattleState battleState;

    [Header("# 플레이어 및 몬스터")]
    public GameObject player;
    public List<GameObject> monsters = new List<GameObject>();

    private int currentMonsterIndex = -1;
    private float delay = 1.5f;


    [Header("# UI 오브젝트")]
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
        battleState = BattleState.Start;
        // Fade 애니메이션 재생

        // 플레이어 및 몬스터 활성화
        player.gameObject.SetActive(true);
        foreach (GameObject monster in monsters)
        {
            monster.gameObject.SetActive(true);
        }

        // 맵 생성
        MapGenerator.instatnce.CreateMap(MapGenerator.instatnce.garo, MapGenerator.instatnce.sero);

        // 플레이어 시작 
        PlayerTurn();
    }

    
    // 플레이어 턴
    public void PlayerTurn()
    {
        battleState = BattleState.PlayerTurn;
        ui[0].gameObject.SetActive(true);
        ui[0].gameObject.GetComponent<Animator>().Play("PlayerTurn", -1, 0f);
        turnEnd.interactable = true;  // 버튼 제어
    }


    // 몬스터 턴
    public void MonsterTurn()
    {
        battleState = BattleState.MonsterTurn;
        ui[1].gameObject.SetActive(true);
        ui[1].gameObject.GetComponent<Animator>().Play("MonsterTurn", -1, 0f);
        turnEnd.interactable = false;  // 버튼 제어
        StartCoroutine(NextMonster());
    }


    // 몬스터 행동 시작
    IEnumerator NextMonster()
    {
        // 잠시 대기
        yield return new WaitForSeconds(delay);

        if (currentMonsterIndex < monsters.Count - 1)
        {
            currentMonsterIndex++;
            monsters[currentMonsterIndex].GetComponent<MonsterMove>().MoveStart();

            // 인덱스 초기화
            if (currentMonsterIndex == monsters.Count - 1)
            {
                /*monsters[currentMonsterIndex].GetComponent<MonsterData>().IncreaseDamage(1);*/
                currentMonsterIndex = -1;
            }
        }
    }
}

