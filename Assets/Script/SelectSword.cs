using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSword : MonoBehaviour
{
    [SerializeField]
    private GameObject WrongText;
    [SerializeField] // SwordText를 Inspector에서 할당할 수 있도록 설정
    private GameObject SwordText;
    public static bool Correct;
    public float ClearDelay = 2f; // 클리어 후 씬 전환까지 기다리는 시간(초)
    public float Wrongtime = 0.5f; // 텍스트가 켜져 있을 시간 (0.5초로 예시)

    // Start is called before the first frame update
    void Start()
    {
        WrongText.SetActive(false); // 잘못된 검 누르면 나올 텍스트 비활성화
        SwordText.SetActive(false); // 맞는 검 누르면 나올 텍스트 비활성화
        Correct = false; // 맞는 검 선택했는지 초기화
    }

    // Update is called once per frame
    void Update()
    {
        if (Correct) return; // 이미 정답 처리됨: 스크립트가 여러 개라 Main을 중복 로드하는 것 방지
        // 마우스 클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭한 좌표값 가져오기
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 해당 좌표에 있는 오브젝트 찾기
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            // 충돌 감지되면 태그 확인하기
            if (hit.collider != null) // 충돌 오브젝트가 있는지 확인
            {
                if (hit.transform.CompareTag("NotSword"))
                {
                    // 코루틴 호출하여 텍스트를 일정 시간동안만 표시
                    StartCoroutine(ShowWrongText());
                }
                else if (hit.transform.CompareTag("Sword"))
                {
                    Correct = true;
                    SwordText.SetActive(true); // 맞는 검을 눌렀을 때 텍스트 활성화
                    StartCoroutine(LoadMainAfterDelay()); // 3초 뒤 씬 변경
                }
            }
        }
    }

    // 클리어 후 잠깐 기다렸다가 Main으로 돌아감
    IEnumerator LoadMainAfterDelay()
    {
        yield return new WaitForSeconds(ClearDelay);
        SceneManager.LoadScene("Main"); // 씬 변경
    }

    // 잘못된 검을 눌렀을 때 텍스트를 보여주는 코루틴 함수
    IEnumerator ShowWrongText()
    {
        WrongText.SetActive(true); // 텍스트 활성화
        yield return new WaitForSeconds(Wrongtime); // 일정 시간 기다림
        WrongText.SetActive(false); // 텍스트 비활성화
    }
}

