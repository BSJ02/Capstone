using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UIElements.UxmlAttributeDescription;


//public enum CardState
//{
//    SwordSlash,
//    HealingSalve,
//    Sprint,
//    BasicStrike,
//    ShieldBlock,
//    AxSlash,
//    Heal,
//    Teleport,
//    GuardianSpirit,
//    HolyNova,
//    Fireball,
//    LightningStrike,
//    ExcalibursWrath,
//    DivineIntervention,
//    SoulSiphon
//}

public class CardData : MonoBehaviour
{
    [HideInInspector] public bool waitForInput = false;  // ��� ���� ����
    [HideInInspector] public bool waitAnim = false;
    [HideInInspector] public bool usingCard = false;
    [HideInInspector] public bool coroutineStop = false;
    [HideInInspector] public int TempActivePoint;

    private PlayerMoveTest playerMoveTest;
    private BattleManager battleManager;

    [Header(" # Player Scripts")] public Player player;
    [Header(" # Map Scripts")] public MapGenerator mapGenerator;

    private PlayerState playerState;

    [Header(" # Player Object")] public GameObject playerObject;


    private GameObject selectedTarget = null;
    private float cardUseDistance = 0;

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
            mapGenerator.CardUseRange(playerObject.transform.position, (int)cardUseDistance);
        }
    }

    // ī�� ��� �޼���
    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
    {
        StartCoroutine(WaitForTargetSelection(card));
    }


    // ��� ������ ��ٸ��� �ڷ�ƾ
    private IEnumerator WaitForTargetSelection(Card card)
    {
        battleManager.isPlayerMove = false;
        TempActivePoint = player.playerData.activePoint;
        player.playerData.activePoint = 0;
        cardUseDistance = card.cardPower[2];    // ī�� �Ÿ� ����
        while (true)
        {
            waitForInput = true;    // ��� ���·� ��ȯ

            // ��� ������ �Ϸ�� ������ �ݺ��մϴ�.
            while (waitForInput)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    SelectTarget();
                
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

            usingCard = false;
            mapGenerator.ClearHighlightedTiles();

            if (!waitForInput)
            {
                break;
            }

        }
    }

    private void SelectTarget() // ��� ����
    {
        selectedTarget = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Monster") || hit.collider.CompareTag("Tile"))
            {

                selectedTarget = hit.collider.gameObject;            
                if (Vector3.Distance(player.transform.position, selectedTarget.transform.position) <= cardUseDistance)
                {
                    waitForInput = false;
                }
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
            // ī�� ��� �ִϸ��̼�
            player.AttackTwoAnim();

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            //cardUseDistance = card.cardPower[1];
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
            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();

            player.playerData.Hp += card.cardPower[0];
            cardUseDistance = card.cardPower[1];
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

            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();

            // �÷��̾� �߰� �̵�
            player.playerData.activePoint += (int)card.cardPower[0] + TempActivePoint;
            cardUseDistance = card.cardPower[0];
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
            // ī�� ��� �ִϸ��̼�
            player.StabAnim();

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
            cardUseDistance = card.cardPower[1];
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
            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();

            Debug.Log(card.cardName + " ī�带 ��� / " + player + " Armor: " + player.playerData.Armor);
            player.playerData.Armor += card.cardPower[0];
            Debug.Log(player + "Armor: " + player.playerData.Armor);
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
            // ī�� ��� �ִϸ��̼�
            player.SpinAttackAnim();

            Debug.Log(card.cardName + " / TargetName: " + monster);
            monster.GetHit(card.cardPower[0]);
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
            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();

            Debug.Log(card.cardName + " ī�带 ��� / " + player + " Hp: " + player.playerData.Hp);
            player.playerData.Hp += card.cardPower[0];
            Debug.Log(player + "Hp: " + player.playerData.Hp);
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
            // ī�� ��� �ִϸ��̼�
            player.ChargeAnim();

            Debug.Log("Teleport ī�带 ���");

            // �÷��̾� �߰� �̵�
            player.playerData.activePoint = TempActivePoint;

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
            player.ChargeAnim();
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
            player.ChargeAnim();
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
            player.MacigAttack01Anim();
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
            player.MacigAttack02Anim();
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
            player.MacigAttack03Anim();
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
            player.ChargeAnim();
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
            player.MacigAttack03Anim();
        }
        else
        {
            waitForInput = true;
        }
    }
}