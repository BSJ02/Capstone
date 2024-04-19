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
        // 카드 이름에 따라 처리할 내용을 구현.
        switch (card.cardName)
        {
            case "Sword Slash":
                // SwordSlash 카드를 사용하는 로직을 호출
                UseSwordSlash(card, target);
                break;
            // 다른 카드 타입에 대한 처리를 추가
            default:
                Debug.LogError("해당 카드 타입을 처리하는 코드가 없음");
                break;
        }
    }

    // SwordSlash 카드를 사용하는 메서드
    private void UseSwordSlash(Card card, GameObject target)
    {
        // SwordSlash 카드의 처리 코드를 작성
        Debug.Log("SwordSlash 카드를 사용");

        // 대상의 값을 변경합니다. (예: 데미지를 적용)
        if (target != null)
        {
            float hp = target.transform.GetComponent<MonsterData>().Hp;
            hp -= card.cardPower[0];
        }
    }

    // 카드 사용시 애니메이션
    private void UseCardAnimation()
    {

    }
}