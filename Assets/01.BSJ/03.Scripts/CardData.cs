using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;

public class CardData : MonoBehaviour
{
    private bool waitForInput = false;  // ��� ���� ����
    private float animationDuration = 1.0f; // ī�� �ִϸ��̼� �ð�


    // ī�� ��� �޼���
    public void UseCardAndSelectTarget(Card card)
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


    // ī�� ������� �ִϸ��̼�   
    private IEnumerator AnimateCardDisappearance(GameObject cardObject)
    {
        float timer = 0;
        Vector3 initialScale = cardObject.transform.localScale;

        while (timer < animationDuration)
        {
            // �ð��� ���� �������� �ٿ��� ī�带 ������� ��
            float scale = Mathf.Lerp(1.0f, 0.0f, timer / animationDuration);
            cardObject.transform.localScale = initialScale * scale;

            // �ð� ������Ʈ
            timer += Time.deltaTime;

            yield return null;
        }

        // ī�� ������Ʈ ��Ȱ��ȭ
        cardObject.SetActive(false);
    }


    // SwordSlash ī�带 ����ϴ� �޼���
    private void UseSwordSlash(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        // SwordSlash ī���� ó�� �ڵ带 �ۼ�
        Debug.Log("SwordSlash ī�带 ���");

        // ����� ���� �����մϴ�. (��: �������� ����)
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

    private void UseHealingSalve(Card card, GameObject selectedTarget)
    {
        // ī�� ��� �ִϸ��̼�

        // SwordSlash ī���� ó�� �ڵ带 �ۼ�
        Debug.Log("SwordSlash ī�带 ���");

        // ����� ���� �����մϴ�. (��: �������� ����)
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