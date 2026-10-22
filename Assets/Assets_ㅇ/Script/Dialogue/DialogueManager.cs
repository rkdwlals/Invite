using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;          // 대화창 역할을 할 GameObject
    public GameObject nameBox;              // 이름이 출력될 배경 역할을 할 GameObject
    public TextMesh dialogueText;           // 대화 텍스트를 출력할 TextMesh (Legacy 3D Text)
    public TextMesh characterNameText;      // 캐릭터 이름을 출력할 TextMesh (Legacy 3D Text)
    public float typingSpeed = 0.05f;       // 글자 출력 간격 (초 단위)

    private int sentenceIndex;              // 현재 대화 진행 인덱스
    private bool isDialogueActive;          // 대화창 활성화 여부
    private bool isTyping;                  // 글자가 출력 중인지 여부
    private string currentSentence;         // 현재 출력 중인 문장
    private string[] currentDialogueSentences; // 현재 NPC의 대화 내용 저장
    private Coroutine typingCoroutine;      // 현재 실행 중인 타이핑 Coroutine 저장

    void Start()
    {
        dialogueBox.SetActive(false);
        nameBox.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        characterNameText.gameObject.SetActive(false);

        isDialogueActive = false;
        isTyping = false;
    }

    public void StartDialogue(NPCData npcData)
    {
        dialogueBox.SetActive(true);
        nameBox.SetActive(true);
        characterNameText.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        characterNameText.text = npcData.characterName; // NPC 이름 설정
        currentDialogueSentences = npcData.dialogueSentences; // 대사 내용 설정

        sentenceIndex = 0; // 대화 인덱스 초기화
        isDialogueActive = true;

        // 첫 번째 문장을 한 글자씩 출력
        DisplayNextSentence(true); // true로 호출하여 첫 문장 한 글자씩 출력
    }

    public void DisplayNextSentence(bool firstCall = false)
    {
        if (isTyping) // 타이핑 중이라면
        {
            // 현재 타이핑 중인 Coroutine을 중지하고 문장을 즉시 완성
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentSentence; // 현재 문장을 즉시 표시
            isTyping = false; // 타이핑 상태 종료
        }
        else
        {
            // 첫 번째 호출이 아닐 경우
            if (!firstCall && sentenceIndex < currentDialogueSentences.Length)
            {
                currentSentence = currentDialogueSentences[sentenceIndex]; // 현재 문장 저장
                sentenceIndex++; // 인덱스 증가

                // 한 글자씩 출력하는 Coroutine 시작
                typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
            }
            else if (sentenceIndex < currentDialogueSentences.Length) // 첫 호출이 아닌 경우 대사 출력
            {
                // 클릭 시 다음 문장 출력
                currentSentence = currentDialogueSentences[sentenceIndex]; // 현재 문장 저장
                dialogueText.text = currentSentence; // 즉시 출력
                sentenceIndex++; // 인덱스 증가
            }
            else
            {
                EndDialogue(); // 모든 대사 종료
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true; // 타이핑 상태 활성화
        dialogueText.text = ""; // 텍스트 초기화

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typingSpeed); // 출력 간격
        }

        isTyping = false; // 타이핑 상태 종료
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        nameBox.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        characterNameText.gameObject.SetActive(false);
        isDialogueActive = false;

        // 캐릭터 이름에 따라 씬 전환
        string characterName = characterNameText.text;
        Debug.Log("Character Name: " + characterName); // 디버깅 로그 추가

        // 미니게임에 들어가기 전에 플레이어 위치 저장 (돌아올 때 이 자리로 복귀)
        if (characterName != "???")
            ReturnPosition.Save();

        try
        {
            if (characterName == "인공지능 집사 루테인")
            {
                SceneManager.LoadScene("CardScene");
            }
            else if (characterName == "눈치를 보는 정비공")
            {
                SceneManager.LoadScene("PuzzleScene");
            }
            else if (characterName == "어딘가 이상한 인공지능")
            {
                SceneManager.LoadScene("SwordScene");
            }
            else if (characterName == "???")
            {
                SceneManager.LoadScene("End");
            }
            else
            {
                Debug.LogWarning("Unknown character name: " + characterName); // 알 수 없는 캐릭터 이름 경고
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading scene: " + e.Message); // 에러 메시지 출력
        }
    }



    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence(); // 마우스 클릭 시 다음 대사 출력
        }
    }
}