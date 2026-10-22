using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameControl : MonoBehaviour
{
    [SerializeField]
    private Transform[] pictures; //퍼즐 그림

    [SerializeField]
    private GameObject winText; //이기면 나올 텍스트
    public static bool youwin; //이겼다는 판단
    public float ClearDelay = 2f; // 클리어 후 씬 전환까지 기다리는 시간(초)
    private bool isClearing = false; // 씬 전환 대기 중인지 (중복 실행 방지)
    // Use this for initialization
    void Start()
    {
        winText.SetActive(false); //이기면 나올 텍스트 꺼놓기
        youwin = false; //이겼다는 판단 꺼놓기
        isClearing = false;
    }

    private void Update()
    {
        if (isClearing) return; // 이미 클리어해서 씬 전환을 기다리는 중
        // Update is called once per frame void Update () {
        if (pictures[0].rotation.z == 0 &&
            pictures[1].rotation.z == 0 &&
            pictures[2].rotation.z == 0 &&
            pictures[3].rotation.z == 0 &&
            pictures[4].rotation.z == 0 &&
            pictures[5].rotation.z == 0)
        {//대충 모든 퍼즐의 회전 각도가 0일 때<쉽게 말해서 회전이 되지 않은 똑바로 보이는 상태>
            youwin = true;
            winText.SetActive(true);//이기면 보이는 텍스트 & 이겼다는 판단 트루로 활성화
            isClearing = true;
            StartCoroutine(LoadMainAfterDelay()); //3초 뒤 다시 씬으로 돌아감

        }
    }

    // 클리어 후 잠깐 기다렸다가 Main으로 돌아감
    private IEnumerator LoadMainAfterDelay()
    {
        yield return new WaitForSeconds(ClearDelay);
        SceneManager.LoadScene("Main");
    }
}
