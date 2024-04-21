using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;

public class CardData : MonoBehaviour
{
    [HideInInspector] public bool waitForInput = false;  // ��� ���� ����
    [HideInInspector] public bool waitAnim = false;

    private PlayerAnimationEvents playerAnimationEvents;
    private PlayerMove playerMove;

    [Header("Animation ���� �� ĳ����")]
    public GameObject playerObject;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        playerAnimationEvents = playerObject.GetComponent<PlayerAnimationEvents>();
    }


    private Coroutine currentCoroutine; // ���� ���� ���� �ڷ�ƾ

    // ī�� ��� �޼���
    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
    {
        StartCoroutine(WaitForTargetSelection(card));
    }


    // ��� ������ ��ٸ��� �ڷ�ƾ
    private IEnumerator WaitForTargetSelection(Card card)
    {
        while (true)
        {
            waitForInput = true;    // ��� ���·� ��ȯ

            GameObject selectedTarget = null;   // ���õ� ����� ������ ����

            // ��� ������ �Ϸ�� ������ �ݺ��մϴ�.
            while (waitForInput)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Tile"))
                        {
                            selectedTarget = hit.collider.gameObject;
                            waitForInput = false;
                        }
                    }
                }
                yield return null; // ���� �����ӱ��� ���
            }
            UseCard(card, selectedTarget);
            if (waitForInput)
            {
                Debug.Log("����� �ٽ� �����ϼ���.");
                continue;
            }
            else
            {
                break;
            }
        }
    }

    private void UseCard(Card card, GameObject selectedTarget)
    {
        // ���õ� ��� ���� ī�带 ���
        if (selectedTarget != null)
        {
            waitAnim = true;
            // cardName�� ����ϴ� ������ ȣ��
            switch (card.cardName)
            {
                case "Sword Slash":
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Healing Salve":
                    UseHealingSalve(card, selectedTarget);
                    break;
                case "Sprint":
                    UseSprint(card, selectedTarget);
                    break;
                case "Basic Strike":
                    UseBasicStrike(card, selectedTarget);
                    break;
                case "Shield Block":
                    UseShieldBlock(card, selectedTarget);
                    break;
                case "Ax Slash":
                    UseShieldBlock(card, selectedTarget);
                    break;
                default:
                    Debug.LogError("�ش� ī�� Ÿ���� ó���ϴ� �ڵ尡 ����");
                    break;
            }
        }
    }

    // Base Cards --------------------------------
    // Sword Slash ī�� (���� Į�� �����մϴ�.)
    private void UseSwordSlash(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();

        if (monster != null)
        {
            Debug.Log(card.cardName + " ī�带 ��� / " + monster + " Hp: " + monster.monsterData.Hp);

            monster.GetHit(card.cardPower[0]);

            Debug.Log(monster + " Hp: " + monster.monsterData.Hp);

            // ī�� ��� �ִϸ��̼�
            playerAnimationEvents.SlashAnim();
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Healing Salve ī�� (���ʸ� ����Ͽ� ü���� ȸ���մϴ�.)
    private void UseHealingSalve(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Player player = selectedTarget.GetComponent<Player>();

        if (player != null)
        {
            Debug.Log(card.cardName + " ī�带 ��� / " + player + " Hp: " + player.playerData.Hp);
            player.playerData.Hp += card.cardPower[0];
            Debug.Log(player +  "Hp: " + player.playerData.Hp);

            // ī�� ��� �ִϸ��̼�
            playerAnimationEvents.ChargeAnim();
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Sprint ī�� (������ �̵��Ͽ� ���� ������ ���մϴ�.)
    private void UseSprint(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();

        if (monster != null)
        {
            Debug.Log("Sprint ī�带 ���");

            // �÷��̾� �߰� �̵�
            //playerMove.isMoving = true;

            // ī�� ��� �ִϸ��̼�
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Basic Strike ī�� (������ ������ ���� ���� �����մϴ�.)
    private void UseBasicStrike(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " ī�带 ��� / " + monster + " Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log(monster + " Hp: " + monster.monsterData.Hp);

            // ī�� ��� �ִϸ��̼�
            playerAnimationEvents.StabAnim();
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Shield Block ī�� (���з� ������ ���� �޴� ���ظ� ���ҽ�ŵ�ϴ�.)
    private void UseShieldBlock(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log(card.cardName + " ī�带 ��� / " + player + " Armor: " + player.playerData.Armor);
            player.playerData.Armor += card.cardPower[0];
            Debug.Log(player + "Armor: " + player.playerData.Armor);

            // ī�� ��� �ִϸ��̼�
            playerAnimationEvents.DefendAnim();
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Common Cards --------------------------------
    // Ax Slash ī�� (���� ������ �����մϴ�.)
    private void UseAxSlash(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " ī�带 ��� / " + monster + " Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log(monster + " Hp: " + monster.monsterData.Hp);

            // ī�� ��� �ִϸ��̼�
            playerAnimationEvents.SlashAnim();
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Heal!! ī�� (�ູ�� �޾� ü���� ȸ���մϴ�.)
    private void UseHeal(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log(card.cardName + " ī�带 ��� / " + player + " Hp: " + player.playerData.Hp);
            player.playerData.Hp += card.cardPower[0];
            Debug.Log(player + "Hp: " + player.playerData.Hp);

            // ī�� ��� �ִϸ��̼�
            playerAnimationEvents.ChargeAnim();
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Teleport ī�� (���ϴ� ��ġ�� �����̵��Ͽ� �̵��մϴ�.)
    private void UseTeleport(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Teleport ī�带 ���");

            // �÷��̾� �߰� �̵�

            // ī�� ��� �ִϸ��̼�
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Swift Strike ī�� (������ ������ ������ ���� ���� �����մϴ�.)
    private void UseSwiftStrike(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Swift Strike ī�带 ���");

            Debug.Log(card.cardName + " ī�带 ��� / " + monster + " Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log(monster + " Hp: " + monster.monsterData.Hp);

            // ī�� ��� �ִϸ��̼�
            playerAnimationEvents.StabAnim();
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Thunderstorm ī�� (�ֺ��� ������ ���� ��� ������ �������� �����ϴ�.)
    private void UseThunderstorm(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Thunderstorm ī�带 ���");

            Debug.Log(card.cardName + " ī�带 ��� / " + monster + " Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log(monster + " Hp: " + monster.monsterData.Hp);

            // ī�� ��� �ִϸ��̼�
            playerAnimationEvents.ChargeAnim();
            waitAnim = false;
        }
        else
        {
            waitForInput = true;
        }
    }

    // Rare Cards --------------------------------
}