using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    public string characterName;        // NPC 캐릭터 이름
    [TextArea(3, 10)]
    public string[] dialogueSentences;  // 대사 목록
}
