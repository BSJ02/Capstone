using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class CardData : MonoBehaviour
{
    [HideInInspector] public bool waitForInput = false;  // ��� ���� ����
    [HideInInspector] public bool waitAnim = false;

    private PlayerMoveTest playerMoveTest;
    private BattleManager battleManager;
    [Header(" # Player Scripts")] public Player player;
    [Header(" # Map Scripts")] public MapGenerator mapGenerator;

    private PlayerState playerState;

    [Header(" # Player Object")] public GameObject playerObject;

    //private float maxAttackRange = 3.0f;

    [HideInInspector] public bool usingCard = false;
    [HideInInspector] public bool coroutineStop = false;

    [HideInInspector] public int TempActivePoint;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        player = playerObject.GetComponent<Player>();
    }

    private void Update()
    {
        if (usingCard)
        {
            mapGenerator.PlayerAttackRange(playerObject.transform.position, 1);
        }

    }

    // ī�� ��� �޼���
    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
    {
        StartCoroutine(WaitForTargetSelection(card));
    }

    //if (Vector3.Distance(player.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) <= maxAttackRange)
    //            {


    // ��� ������ ��ٸ��� �ڷ�ƾ
    private IEnumerator WaitForTargetSelection(Card card)
    {
        battleManager.isPlayerMove = false;
        TempActivePoint = player.playerData.activePoint;
        player.playerData.activePoint = 0;
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
            if (coroutineStop)
            {
                coroutineStop = false;
                mapGenerator.ClearHighlightedTiles();
                yield break;
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
                    UseAxSlash(card, selectedTarget);
                    break;
                case "Heal!!":
                    UseHeal(card, selectedTarget);
                    break;
                case "Teleport":
                    UseTeleport(card, selectedTarget);
                    break;
                case "Guardian Spirit":
                    UseGuardianSpirit(card, selectedTarget);
                    break;
                case "Holy Nova":
                    UseHolyNova(card, selectedTarget);
                    break;
                case "Fireball":
                    UseFireball(card, selectedTarget);
                    break;
                case "Lightning Strike":
                    UseLightningStrike(card, selectedTarget);
                    break;
                case "Excalibur's Wrath":
                    UseExcalibursWrath(card, selectedTarget);
                    break;
                case "Divine Intervention":
                    UseDivineIntervention(card, selectedTarget);
                    break;
                case "Soul Siphon":
                    UseSoulSiphon(card, selectedTarget);
                    break;
                default:
                    Debug.LogError("�ش� ī�� Ÿ���� ó���ϴ� �ڵ尡 ����");
                    break;
            }
            usingCard = false;
            mapGenerator.ClearHighlightedTiles();
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
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�
            player.AttackTwoAnim();
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
            player.playerData.Hp += card.cardPower[0];

            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();
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
        Player player = selectedTarget.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Sprint ī�带 ���");

            // �÷��̾� �߰� �̵�
            player.playerData.activePoint += (int)card.cardPower[0];

            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();
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
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            player.StabAnim();
            
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
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�

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

        }
        else
        {
            waitForInput = true;
        }
    }

    // Guardian Spirit ī�� (��ȣ ������ ��ȯ�Ͽ� �÷��̾��� ������ ������ŵ�ϴ�.)
    private void UseGuardianSpirit(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�

        }
        else
        {
            waitForInput = true;
        }
    }

    // Rare Cards --------------------------------
    // Holy Nova ī�� (�ֺ� ������ �ż��� ���� ������ ������ �������� �����ϴ�.)
    private void UseHolyNova(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�

        }
        else
        {
            waitForInput = true;
        }
    }

    // Fireball ī�� (ȭ������ �߻��Ͽ� ������ ������ �������� �����ϴ�.)
    private void UseFireball(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�

        }
        else
        {
            waitForInput = true;
        }
    }

    // Lightning Strike (�տ� ������ ��� ������ �ϰ��� ���� ������ �������� �����ϴ�.)
    private void UseLightningStrike(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�

        }
        else
        {
            waitForInput = true;
        }
    }

    // Epic Cards --------------------------------
    // Excalibur's Wrath (�������� ���� �г�� ���� �����մϴ�.)
    private void UseExcalibursWrath(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�

        }
        else
        {
            waitForInput = true;
        }
    }

    // Divine Intervention (���� �������� �÷��̾ ��ȣ�ϰ� ȸ����ŵ�ϴ�.)
    private void UseDivineIntervention(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�

        }
        else
        {
            waitForInput = true;
        }
    }

    // Legend Cards --------------------------------
    // Soul Siphon (��ȥ�� ����Ͽ� �ֺ� ���� ������� ����ϰ� ȸ���մϴ�.)
    private void UseSoulSiphon(Card card, GameObject selectedTarget)
    {
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);

            // ī�� ��� �ִϸ��̼�

        }
        else
        {
            waitForInput = true;
        }
    }
}