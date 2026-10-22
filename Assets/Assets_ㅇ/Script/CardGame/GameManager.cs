using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshProUGUI를 사용하기 위해 추가
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int objectsInCart = 0;
    private int fakeItemCount = 0;  // Fake 아이템 카운트 추가
    private int goodItemCount = 0;  // Good 아이템 카운트 추가
    private List<DragAndDrop> draggableObjects = new List<DragAndDrop>();

    public TextMeshProUGUI warningText;  // 가짜 물품이 있을 때 보여줄 경고 메시지 UI 텍스트

    // 클리어 조건을 위한 태그 순서
    private string[] correctOrder = { "Good1", "Good2", "Good3", "Good4" }; // Good1 = 스페이드 퀸, Good2 = 스페이드 킹, Good3 = 클로버 잭, Good4 = 하트 에이스
    private int currentOrderIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 경고 메시지 초기화 (시작할 때 비활성화)
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);  // 경고 메시지를 비활성화
        }
    }

    // 드래그 가능한 오브젝트를 등록하는 메서드
    public void RegisterDraggableObject(DragAndDrop obj)
    {
        if (!draggableObjects.Contains(obj))
        {
            draggableObjects.Add(obj);
        }
    }

    // 모든 드래그 가능한 오브젝트를 초기화하는 메서드
    public void ResetAllObjects()
    {
        foreach (DragAndDrop obj in draggableObjects)
        {
            obj.ResetPosition(); // 모든 오브젝트를 원래 위치로 초기화
        }
    }

    // 카트에 오브젝트가 들어갔을 때 호출되는 메서드
    public void ObjectEnteredCart()
    {
        objectsInCart++;
    }

    // 카트에서 오브젝트가 나갔을 때 호출되는 메서드
    public void ObjectExitedCart()
    {
        objectsInCart--;
    }

    // 가짜 물품이 카트에 들어갔을 때 호출되는 메서드
    public void FakeItemEnteredCart()
    {
        fakeItemCount++;
        UpdateWarningText();  // 경고 메시지 업데이트
    }

    // 가짜 물품이 카트에서 나갔을 때 호출되는 메서드
    public void FakeItemExitedCart()
    {
        fakeItemCount--;
        UpdateWarningText();  // 경고 메시지 업데이트
    }

    // Good 물품이 카트에 들어갔을 때 호출되는 메서드
    public void GoodItemEnteredCart(string goodTag)
    {
        // Good 물품이 정해진 순서대로 들어갔는지 확인
        if (correctOrder[currentOrderIndex] == goodTag)
        {
            currentOrderIndex++;
            if (currentOrderIndex >= correctOrder.Length)
            {
                SceneManager.LoadScene("Main");  // 조건을 만족하면 씬 리로드
            }
        }
        else
        {
            // 잘못된 순서로 들어가면 초기화
            currentOrderIndex = 0;
        }
    }

    // Good 물품이 카트에서 나갔을 때 호출되는 메서드
    public void GoodItemExitedCart()
    {
        goodItemCount--;
        currentOrderIndex = 0; // Good 순서도 초기화
    }

    // 가짜 물품이 있을 때 경고 메시지를 보여주는 메서드
    private void UpdateWarningText()
    {
        if (fakeItemCount > 0 && warningText != null)
        {
            warningText.gameObject.SetActive(true);  // 경고 메시지 활성화
        }
        else if (warningText != null)
        {
            warningText.gameObject.SetActive(false);  // 경고 메시지 비활성화
        }
    }

    // **카트 초기화 메서드 추가**
    public void ResetCart()
    {
        // 모든 드래그 가능한 오브젝트들의 위치를 초기화
        ResetAllObjects();

        // 카트 상태 초기화
        objectsInCart = 0;
        fakeItemCount = 0;
        goodItemCount = 0;
        currentOrderIndex = 0;

        // 경고 메시지 초기화
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
    }
}
