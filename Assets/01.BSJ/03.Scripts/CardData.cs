using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;

public class CardData : MonoBehaviour
{
    private CardManager cardManager;
    public LayerMask targetLayer; // ������� �ν��� ���̾� ����ũ
    private GameObject target;  // ī�� ȿ�� ������ ���

    private bool waitForInput = false;  // ��� ���� ����

    // ī�� ��� �޼���
    public void UseCardToSelectTarget(Card card)
    {
        StartCoroutine(WaitForTargetSelection(card));   // ��� �ڷ�ƾ ����
    }

    // ��� ������ ��ٸ��� �ڷ�ƾ
    private IEnumerator WaitForTargetSelection(Card card)
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
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
                    {
                        selectedTarget = hit.collider.gameObject;

                        waitForInput = false;
                        break;
                    }
                        
                }
            }
            
            yield return null; // ���� �����ӱ��� ���
        }

        // ���õ� ��� ���� ī�带 ���
        if (selectedTarget != null)
        {
            // ��� ���� ó���� �����մϴ�.
            switch (card.cardName)
            {
                case "Sword Slash":
                    // SwordSlash ī�带 ����ϴ� ������ ȣ��
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Healing Salve":
                    // SwordSlash ī�带 ����ϴ� ������ ȣ��
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Sprint":
                    // SwordSlash ī�带 ����ϴ� ������ ȣ��
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Basic Strike":
                    // SwordSlash ī�带 ����ϴ� ������ ȣ��
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Shield Block":
                    // SwordSlash ī�带 ����ϴ� ������ ȣ��
                    UseSwordSlash(card, selectedTarget);
                    break;
                // �ٸ� ī�� Ÿ�Կ� ���� ó���� �߰�
                default:
                    Debug.LogError("�ش� ī�� Ÿ���� ó���ϴ� �ڵ尡 ����");
                    break;
            }
        }
    }

    // SwordSlash ī�带 ����ϴ� �޼���
    private void UseSwordSlash(Card card, GameObject target)
    {
        // SwordSlash ī���� ó�� �ڵ带 �ۼ�
        Debug.Log("SwordSlash ī�带 ���");
        UseCardAnimation();

        // ����� ���� �����մϴ�. (��: �������� ����)
        MonsterData monsterData = target.GetComponent<MonsterData>();
        if (monsterData != null)
        {
            monsterData.Hp -= card.cardPower[0];
        }
        else
        {
            Debug.LogError("monsterData ����");
        }
    }

    // ī�� ���� �ִϸ��̼�
    private void UseCardAnimation()
    {

    }
}