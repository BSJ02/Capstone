using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState 
{
    Start,
    PlayerTurn,
    MonsterTurn,
    Won, // ���� �������� �� �ε� 
    Lost // �й� �� UI ���
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public BattleState battleState;

    [Header("# �÷��̾� �� ����")]
    public GameObject player;
    public List<GameObject> monsters = new List<GameObject>();

    private int currentMonsterIndex = -1;
    private float delay = 1.5f;


    [Header("# UI ������Ʈ")]
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
        // Fade �ִϸ��̼� ���

        // �÷��̾� �� ���� Ȱ��ȭ
        player.gameObject.SetActive(true);
        foreach (GameObject monster in monsters)
        {
            monster.gameObject.SetActive(true);
        }

        // �� ����
        MapGenerator.instatnce.CreateMap(MapGenerator.instatnce.garo, MapGenerator.instatnce.sero);

        // �÷��̾� ���� 
        PlayerTurn();
    }

    
    // �÷��̾� ��
    public void PlayerTurn()
    {
        battleState = BattleState.PlayerTurn;
        ui[0].gameObject.SetActive(true);
        ui[0].gameObject.GetComponent<Animator>().Play("PlayerTurn", -1, 0f);
        turnEnd.interactable = true;  // ��ư ����
    }


    // ���� ��
    public void MonsterTurn()
    {
        battleState = BattleState.MonsterTurn;
        ui[1].gameObject.SetActive(true);
        ui[1].gameObject.GetComponent<Animator>().Play("MonsterTurn", -1, 0f);
        turnEnd.interactable = false;  // ��ư ����
        StartCoroutine(NextMonster());
    }


    // ���� �ൿ ����
    IEnumerator NextMonster()
    {
        // ��� ���
        yield return new WaitForSeconds(delay);

        if (currentMonsterIndex < monsters.Count - 1)
        {
            currentMonsterIndex++;
            monsters[currentMonsterIndex].GetComponent<MonsterMove>().MoveStart();

            // �ε��� �ʱ�ȭ
            if (currentMonsterIndex == monsters.Count - 1)
            {
                /*monsters[currentMonsterIndex].GetComponent<MonsterData>().IncreaseDamage(1);*/
                currentMonsterIndex = -1;
            }
        }
    }
}

