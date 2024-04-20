using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;

public class CardData : MonoBehaviour
{
    [HideInInspector] public bool waitForInput = false;  // ��� ���� ����

    // ī�� ��� �޼���
    public void UseCardAndSelectTarget(Card card, GameObject gameObject)
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
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Monster"))
                    {
                        selectedTarget = hit.collider.gameObject;
                        Debug.Log(selectedTarget.name);
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
                    UseHealingSalve(card, selectedTarget);
                    break;
                case "Sprint":
                    // SwordSlash ī�带 ����ϴ� ������ ȣ��
                    UseSwordSlash(card, selectedTarget);
                    break;
                case "Basic Strike":
                    // SwordSlash ī�带 ����ϴ� ������ ȣ��
                    UseBasicStrike(card, selectedTarget);
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
    private void UseSwordSlash(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log(card.cardName + " ī�带 ���");
        
        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
        else
        {
            Debug.LogError("monsterData ����");
        }
    }

    // HealingSalve ī�带 ����ϴ� �޼���
    private void UseHealingSalve(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log("HealingSalve ī�带 ���");

        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
        else
        {
            Debug.LogError("monsterData ����");
        }
    }

    // HealingSalve ī�带 ����ϴ� �޼���
    private void UseBasicStrike(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log("BasicStrike ī�带 ���");

        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
        else
        {
            Debug.LogError("monsterData ����");
        }
    }
}