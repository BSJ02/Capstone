using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Card;

public class CardData : MonoBehaviour
{
    public CardInform cardInform;

    public void UseCard(Card card, GameObject target)
    {
        // ī�� �̸��� ���� ó���� ������ ����.
        switch (card.cardName)
        {
            case "Sword Slash":
                // SwordSlash ī�带 ����ϴ� ������ ȣ��
                UseSwordSlash(card, target);
                break;
            // �ٸ� ī�� Ÿ�Կ� ���� ó���� �߰�
            default:
                Debug.LogError("�ش� ī�� Ÿ���� ó���ϴ� �ڵ尡 ����");
                break;
        }
    }

    // SwordSlash ī�带 ����ϴ� �޼���
    private void UseSwordSlash(Card card, GameObject target)
    {
        // SwordSlash ī���� ó�� �ڵ带 �ۼ�
        Debug.Log("SwordSlash ī�带 ���");

        // ����� ���� �����մϴ�. (��: �������� ����)
        if (target != null)
        {
            float hp = target.transform.GetComponent<MonsterData>().Hp;
            hp -= card.cardPower[0];
        }
    }

    // ī�� ���� �ִϸ��̼�
    private void UseCardAnimation()
    {

    }
}