using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    Collider2D collider2D;
    Vector3 offset;
    public string destinationTag = "Cart";
    public string fakeTag = "Fake"; // Fake 태그 추가
    public string goodTagPrefix = "Good"; // Good 태그 프리픽스 추가
    public float dragZPosition = -1.0f; // 드래그 시 Z-축 위치를 고정할 값
    public bool isInCart = false; // 카트 안에 있는지 여부 (public으로 변경)
    private bool isInNonCountZone = false; // Non-Count 존에 있는지 여부
    private bool isFake = false; // Fake 물품인지 여부
    private bool isGood = false; // Good 물품인지 여부
    private string goodTag = ""; // Good 태그를 저장할 변수

    private static Vector3 nextOffset = Vector3.zero; // 카트 내에서의 다음 오브젝트 배치 오프셋
    private Vector3 originalPosition; // 카드의 초기 위치

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        originalPosition = transform.position; // 카드의 초기 위치 저장

        // 태그에 따라 물품 상태 설정
        if (CompareTag(fakeTag))
        {
            isFake = true;
        }
        else if (tag.StartsWith(goodTagPrefix)) // Good1, Good2 등 태그 확인
        {
            isGood = true;
            goodTag = tag; // Good 태그 저장
        }

        // GameManager에 드래그 가능한 오브젝트 등록
        GameManager.Instance.RegisterDraggableObject(this);
    }

    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {
        Vector3 newPosition = MouseWorldPosition() + offset;
        newPosition.z = dragZPosition; // Z-축 위치를 고정
        transform.position = newPosition;
    }

    void OnMouseUp()
    {
        collider2D.enabled = false;

        Vector3 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rayDirection = Vector3.forward;

        RaycastHit2D hitInfo = Physics2D.Raycast(rayOrigin, rayDirection);
        if (hitInfo)
        {
            // 카트 안에 들어갔을 때
            if (hitInfo.transform.CompareTag(destinationTag) && !isInNonCountZone)
            {
                Vector3 newPosition = hitInfo.transform.position + new Vector3(-3, 0, -0.01f) + nextOffset;
                newPosition.z = dragZPosition;
                transform.position = newPosition;

                nextOffset += new Vector3(1.3f, 0f, 0); // 다음 카드의 위치 오프셋 증가

                if (!isInCart)
                {
                    isInCart = true;
                    GameManager.Instance.ObjectEnteredCart();

                    // 각 상태에 따라 처리
                    if (isFake)
                    {
                        GameManager.Instance.FakeItemEnteredCart();
                    }
                    else if (isGood)
                    {
                        // Good 카드는 태그에 따라 처리
                        GameManager.Instance.GoodItemEnteredCart(goodTag); // 저장된 Good 태그 정보를 전달
                    }
                }
            }
            else
            {
                // 카트에서 빠져나갔을 때
                if (isInCart)
                {
                    isInCart = false;
                    GameManager.Instance.ObjectExitedCart();

                    if (isFake)
                    {
                        GameManager.Instance.FakeItemExitedCart();
                    }
                    else if (isGood)
                    {
                        GameManager.Instance.GoodItemExitedCart();
                    }

                    // 원래 위치로 되돌리기
                    ResetPosition();
                }
            }
        }

        collider2D.enabled = true;
    }

    // Non-Count Zone 상태 업데이트
    public void SetNonCountZone(bool inNonCountZone)
    {
        isInNonCountZone = inNonCountZone;

        // Non-Count Zone에 들어가면 카트에서 빠져나가는 것으로 처리
        if (isInNonCountZone && isInCart)
        {
            isInCart = false;
            GameManager.Instance.ObjectExitedCart();

            if (isFake)
            {
                GameManager.Instance.FakeItemExitedCart();
            }
            else if (isGood)
            {
                GameManager.Instance.GoodItemExitedCart();
            }

            // 원래 위치로 되돌리기
            ResetPosition();
        }
    }

    // 원래 위치로 되돌리기 메서드
    public void ResetPosition() // public으로 변경
    {
        transform.position = originalPosition; // 원래 위치로 되돌리기
        nextOffset = Vector3.zero; // 다음 오프셋 초기화
        isInCart = false; // 카트 상태 초기화
    }

    // 마우스 월드 위치를 가져오는 메서드
    private Vector3 MouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.nearClipPlane; // 카메라와의 거리 설정
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}
