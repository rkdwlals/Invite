using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 빌드에서 루테인 클릭이 안 되던 문제 수정:
// NPC 위에 바닥 타일맵(groundlayer)의 트리거 콜라이더가 같은 z에 겹쳐 있어서,
// OnMouseDown이 NPC 대신 타일맵을 잡는 경우가 있었다(에디터/빌드에서 결과가 다를 수 있음).
// 그래서 클릭 지점이 이 NPC의 콜라이더 안에 있는지 직접 확인한다.
// DialogueManager.Update보다 먼저 실행되게 해서 기존 OnMouseDown과 같은 순서를 유지한다.
[DefaultExecutionOrder(-100)]
public class CubeClick : MonoBehaviour
{
    public DialogueManager dialogueManager; // DialogueManager 참조
    public NPCData npcData;                 // 각 NPC의 대화 데이터를 저장

    private bool hasBeenClicked = false;    // NPC가 클릭되었는지 여부
    private Collider2D clickCollider;       // 클릭 판정에 쓸 이 NPC의 콜라이더

    void Awake()
    {
        clickCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // 처음 클릭되었을 때만 대화 시작
        if (hasBeenClicked || clickCollider == null || !clickCollider.enabled) return;
        if (!Input.GetMouseButtonDown(0)) return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Vector2 clickPos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (clickCollider.OverlapPoint(clickPos))
        {
            dialogueManager.StartDialogue(npcData);  // NPC의 데이터를 전달
            hasBeenClicked = true; // 이후로는 클릭이 무시됨
        }
    }
}
