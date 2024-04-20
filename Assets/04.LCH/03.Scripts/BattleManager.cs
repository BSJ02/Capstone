using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject player;
    public List<GameObject> monsters = new List<GameObject>();

    private int currentMonsterIndex = -1;
    private float delay = 1.5f;

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

    private void Start()
    {
        battleState = BattleState.Start;
        SetupBattle();
    }

    void SetupBattle()
    {
        // 플레이어 및 몬스터 생성
        Instantiate(player); 
        foreach (GameObject monster in monsters)
        {
            Instantiate(monster);
        }

        // 맵 생성 
        MapGenerator.instatnce.CreateMap(MapGenerator.instatnce.garo, MapGenerator.instatnce.sero);

    }

    public void StartMonsterSequence()
    {
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
            monsters[currentMonsterIndex].GetComponent<MonsterMove>().ButtonClick();

            // 인덱스 초기화
            if (currentMonsterIndex == monsters.Count - 1)
            {
            /*    monsters[currentMonsterIndex].GetComponent<MonsterData>().IncreaseDamage(1);*/
                currentMonsterIndex = -1;
            }
        }
    }
}

