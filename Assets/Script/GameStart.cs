using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameStart : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource effectSound;
    public Collider2D buttonCollider; // 버튼에 Collider2D 추가

    void Start()
    {
        backgroundMusic.Play(); // 씬 시작 시 배경음악 재생
    }

    void Update()
    {
        // 마우스 좌클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 위치 변환
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // 클릭된 위치에 Collider가 있는지 확인

            if (hit.collider != null && hit.collider == buttonCollider) // 버튼 클릭 여부 확인
            {
                Debug.Log("버튼이 클릭됨");
                StartCoroutine(StartGame()); // 버튼이 클릭되면 게임 시작
            }
        }
    }

    IEnumerator StartGame()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop(); // 배경음악 멈춤
            Debug.Log("배경음악 멈춤");
        }

        if (effectSound != null)
        {
            Debug.Log("효과음 찾음");
            effectSound.Play(); // 효과음 재생
            Debug.Log("효과음 나오는 중임, 볼륨: " + effectSound.volume + ", 길이: " + effectSound.clip.length);
            yield return new WaitForSeconds(effectSound.clip.length); // 효과음 재생이 끝날 때까지 대기
        }
        else
        {
            Debug.LogError("효과음 못찾음");
        }

        SceneManager.LoadScene("Main"); // 씬 변경
    }
}
