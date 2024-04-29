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

public class CardProcessing : MonoBehaviour
{
    [HideInInspector] public bool waitForInput = false;  // ��� ���� ����
    [HideInInspector] public bool usingCard = false;
    [HideInInspector] public bool coroutineStop = false;
    [HideInInspector] public int TempActivePoint;

    private WeaponController weaponController;
    private BattleManager battleManager;
    private CardData cardData;

    [Header(" # Player Scripts")] public Player player;
    [Header(" # Map Scripts")] public MapGenerator mapGenerator;

    private PlayerState playerState;

    [Header(" # Player Object")] public GameObject playerObject;

    private GameObject selectedTarget = null;
    [HideInInspector] public float cardUseDistance = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        weaponController = playerObject.GetComponent<WeaponController>();
        battleManager = FindObjectOfType<BattleManager>();
        player = playerObject.GetComponent<Player>();
        cardData = FindObjectOfType<CardData>();
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

    private void SelectTarget()
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
            // cardName�� ����ϴ� ������ ȣ��
            switch (card.cardName)
            {
                case "Sword Slash":
                    cardData.UseSwordSlash(card, selectedTarget);
                    break;
                case "Healing Salve":
                    cardData.UseHealingSalve(card, selectedTarget);
                    break;
                case "Sprint":
                    cardData.UseSprint(card, selectedTarget);
                    break;
                case "Basic Strike":
                    cardData.UseBasicStrike(card, selectedTarget);
                    break;
                case "Shield Block":
                    cardData.UseShieldBlock(card, selectedTarget);
                    break;
                case "Ax Slash":
                    cardData.UseAxSlash(card, selectedTarget);
                    break;
                case "Heal!!":
                    cardData.UseHeal(card, selectedTarget);
                    break;
                case "Teleport":
                    cardData.UseTeleport(card, selectedTarget);
                    break;
                case "Guardian Spirit":
                    cardData.UseGuardianSpirit(card, selectedTarget);
                    break;
                case "Holy Nova":
                    cardData.UseHolyNova(card, selectedTarget);
                    break;
                case "Fireball":
                    cardData.UseFireball(card, selectedTarget);
                    break;
                case "Lightning Strike":
                    cardData.UseLightningStrike(card, selectedTarget);
                    break;
                case "Excalibur's Wrath":
                    cardData.UseExcalibursWrath(card, selectedTarget);
                    break;
                case "Divine Intervention":
                    cardData.UseDivineIntervention(card, selectedTarget);
                    break;
                case "Soul Siphon":
                    cardData.UseSoulSiphon(card, selectedTarget);
                    break;
                default:
                    Debug.LogError("�ش� ī�� Ÿ���� ó���ϴ� �ڵ尡 ����");
                    break;
            }
           
        }
    }

    // ���� ��� ���� ���� ȿ��
    private void ApplySwordEffect(Card card, GameObject selectedTarget)
    {
        // ȿ���� ������Ű�� ó��
    }

    // �ٸ� ���⸦ ��� ���� ���� ȿ��
    private void ApplyOtherWeaponEffect(Card card, GameObject selectedTarget)
    {
        // ȿ���� ��ȭ��Ű�� ó��
    }

    // �⺻���� ȿ�� ó��
    private void ApplyDefaultEffect(Card card, GameObject selectedTarget)
    {
        // Ư���� ó���� �ʿ� ���� ���
    }
}