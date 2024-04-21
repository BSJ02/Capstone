using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    public GameObject cardPrefab; // 카드 프리팹
    public Transform cardSpawnPoint; // 카드가 생성될 위치
    public float cardOverlapOffset = 1f; // 카드 간의 겹침 간격

    private int cardCount = 0; // 생성된 카드 수

    // 버튼 클릭 시 호출되는 함수
    public void SpawnCard()
    {
        // 카드 생성
        GameObject newCard = Instantiate(cardPrefab, cardSpawnPoint.position, Quaternion.identity);
    
        // 카드 위치 조정
        Vector3 offset = new Vector3(cardOverlapOffset * cardCount, 0f, 0f);
        newCard.transform.position += offset;

        // 생성된 카드 수 증가
        cardCount++;
    }
}
