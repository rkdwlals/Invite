using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public Button resetButton; // 리셋 버튼
    private DragAndDrop[] cards; // 모든 카드 저장 배열

    void Start()
    {
        // 모든 DragAndDrop 컴포넌트를 가진 카드 오브젝트 찾기
        cards = FindObjectsOfType<DragAndDrop>();

        // 버튼 클릭 시 리셋 메서드 호출
        resetButton.onClick.AddListener(ResetAllCards);
    }

    // 모든 카드의 위치를 초기화하는 메서드
    private void ResetAllCards()
    {
        foreach (var card in cards)
        {
            // 각 카드의 ResetPosition() 메서드 호출
            card.ResetPosition();
        }

        // 카트 초기화: 카트 상태를 초기화하고 관련 메서드 호출
        GameManager.Instance.ResetCart();
    }
}