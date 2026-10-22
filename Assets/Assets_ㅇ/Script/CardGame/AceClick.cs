using System.Collections;
using UnityEngine;

public class AceClick : MonoBehaviour
{
    public GameObject[] cards; // 1, 2, 3, ..., 10, J, Q, K, Joker 카드들
    public GameObject[] allAceCards; // 모든 에이스 카드 (하트, 클로버, 다이아몬드, 스페이드)
    public float delayBetweenCards = 0.5f; // 카드들이 순차적으로 나타나는 간격
    private bool isAceClicked = false; // 에이스가 클릭되었는지 여부

    void OnMouseDown()
    {
        // 에이스 카드가 클릭될 때마다 상태를 초기화
        if (!isAceClicked) // 에이스가 아직 클릭되지 않았을 때만 동작
        {
            isAceClicked = true; // 에이스 클릭 상태 표시
            StartCoroutine(ShowCards()); // 카드 활성화 코루틴 시작
        }
    }

    IEnumerator ShowCards()
    {
        // 카드 순차적 활성화
        foreach (GameObject card in cards)
        {
            // 카드가 카트에 들어가 있는지 체크하여 비활성화하지 않음
            if (!IsCardInCart(card))
            {
                card.SetActive(true); // 카드 활성화
                yield return new WaitForSeconds(delayBetweenCards); // 다음 카드 활성화 전에 대기
            }
        }

        // 모든 에이스 카드 비활성화
        foreach (GameObject ace in allAceCards)
        {
            ace.SetActive(false); // 모든 에이스 카드 비활성화
        }

        isAceClicked = false; // 클릭 상태 리셋
    }

    private bool IsCardInCart(GameObject card)
    {
        // 카드의 Collider2D가 비활성화되어 있으면 카트에 들어있는 것으로 간주
        Collider2D collider = card.GetComponent<Collider2D>();
        return collider != null && !collider.enabled; // 콜라이더가 비활성화되어 있으면 카트에 들어있다고 판단
    }
}
