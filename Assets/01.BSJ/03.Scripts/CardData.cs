using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;

public class CardData : MonoBehaviour
{
    [HideInInspector] public bool waitForInput = false;  // ��� ���� ����

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


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
    }

    // Healing Salve ī�� (���ʸ� ����Ͽ� ü���� ȸ���մϴ�.)
    private void UseHealingSalve(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log(card.cardName + " ī�带 ���");

        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // �÷��̾� ü�� ȸ��
        }
    }

    // Sprint ī�� (������ �̵��Ͽ� ���� ������ ���մϴ�.)
    private void UseSprint(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log("Sprint ī�带 ���");

        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // �÷��̾� �߰� �̵�
        }
    }

    // Basic Strike ī�� (������ ������ ���� ���� �����մϴ�.)
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
    }

    // Shield Block ī�� (���з� ������ ���� �޴� ���ظ� ���ҽ�ŵ�ϴ�.)
    private void UseShieldBlock(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log("ShieldBlock ī�带 ���");

        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // �÷��̾� ���� ����
        }
    }

    // Common Cards --------------------------------
    // Ax Slash ī�� (���� ������ �����մϴ�.)
    private void UseAxSlash(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log("Shield Block ī�带 ���");

        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
    }

    // Heal!! ī�� (�ູ�� �޾� ü���� ȸ���մϴ�.)
    private void UseHeal(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log("Heal!! ī�带 ���");

        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // �÷��̾� ü�� ȸ��
        }
    }

    // Teleport ī�� (���ϴ� ��ġ�� �����̵��Ͽ� �̵��մϴ�.)
    private void UseTeleport(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log("Teleport ī�带 ���");

        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            // �÷��̾� �߰� �̵�
        }
    }

    // Swift Strike ī�� (������ ������ ������ ���� ���� �����մϴ�.)
    private void UseSwiftStrike(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log("Swift Strike ī�带 ���");

        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
    }

    // Thunderstorm ī�� (�ֺ��� ������ ���� ��� ������ �������� �����ϴ�.)
    private void UseThunderstorm(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        Debug.Log("Thunderstorm ī�带 ���");

        // ����� ���� ����
        Monster monster = selectedTarget.GetComponent<Monster>();
        if (monster != null)
        {
            Debug.Log("Hp: " + monster.monsterData.Hp);
            monster.monsterData.Hp -= card.cardPower[0];
            Debug.Log("Hp: " + monster.monsterData.Hp);
        }
    }

    // Rare Cards --------------------------------
}