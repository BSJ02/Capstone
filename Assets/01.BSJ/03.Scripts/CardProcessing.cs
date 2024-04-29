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
    [HideInInspector] public bool waitForInput = false;  // 대기 상태 여부
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

    // 카드 사용 메서드
    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
    {
        StartCoroutine(WaitForTargetSelection(card));
    }

    // 대상 선택을 기다리는 코루틴
    private IEnumerator WaitForTargetSelection(Card card)
    {
        battleManager.isPlayerMove = false;
        TempActivePoint = player.playerData.activePoint;
        player.playerData.activePoint = 0;
        cardUseDistance = card.cardPower[2];    // 카드 거리 저장
        while (true)
        {
            waitForInput = true;    // 대기 상태로 전환

            
            // 대상 선택이 완료될 때까지 반복합니다.
            while (waitForInput)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    SelectTarget();
                
                }
                yield return null; // 다음 프레임까지 대기
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
                Debug.Log("대상을 다시 선택하세요.");
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
        // 선택된 대상에 따라 카드를 사용
        if (selectedTarget != null)
        {
            // cardName을 사용하는 로직을 호출
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
                    Debug.LogError("해당 카드 타입을 처리하는 코드가 없음");
                    break;
            }
           
        }
    }

    // 검을 들고 있을 때의 효과
    private void ApplySwordEffect(Card card, GameObject selectedTarget)
    {
        // 효과를 증폭시키는 처리
    }

    // 다른 무기를 들고 있을 때의 효과
    private void ApplyOtherWeaponEffect(Card card, GameObject selectedTarget)
    {
        // 효과를 약화시키는 처리
    }

    // 기본적인 효과 처리
    private void ApplyDefaultEffect(Card card, GameObject selectedTarget)
    {
        // 특별한 처리가 필요 없는 경우
    }
}