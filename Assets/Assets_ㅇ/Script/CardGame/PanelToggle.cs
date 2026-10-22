using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public GameObject textPanel; // 텍스트 패널
    public GameObject text01; // 텍스트 오브젝트
    private bool isPanelActive = false; // 패널의 활성화 상태

    void OnMouseDown()
    {
        TogglePanel(); // 패널 토글 메서드 호출
    }

    private void TogglePanel()
    {
        isPanelActive = !isPanelActive; // 현재 상태를 반전시킴
        textPanel.SetActive(isPanelActive); // 텍스트 패널 활성화/비활성화
        text01.SetActive(isPanelActive); // 텍스트 오브젝트 활성화/비활성화
    }
}
