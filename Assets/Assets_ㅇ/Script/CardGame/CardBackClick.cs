using UnityEngine;

public class CardBackClick : MonoBehaviour
{
    public GameObject[] aceCards; // 하트, 클로버, 다이아몬드, 스페이드 에이스 카드
    public GameObject[] matchingCards; // 에이스와 같은 모양의 카드들

    void OnMouseDown()
    {
        // 카드 뒷면 클릭 시 에이스 카드들 표시
        foreach (GameObject ace in aceCards)
        {
            ace.SetActive(true); // 에이스 카드 활성화
        }

        // 카트에 있는 카드들을 제외하고 매칭 카드 비활성화
        foreach (GameObject card in matchingCards)
        {
            DragAndDrop cardComponent = card.GetComponent<DragAndDrop>();
            if (cardComponent != null && !cardComponent.isInCart) // 카트에 들어있지 않은 경우
            {
                card.SetActive(false); // 카드 비활성화
            }
        }
    }
}
