using System.Collections;
using UnityEngine;

public class PlatformTeleporter : MonoBehaviour
{
    public Transform targetPlatform;  // 연결할 발판 위치
    public float teleportCooltime = 0.00000000000001f; //텔레포트 쿨타임 시간

    private bool canTeleport = true;  // 텔레포트 가능한지 여부

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 발판에 닿았고, 텔레포트 가능할 때
        if (other.CompareTag("Player") && canTeleport)
        {
            // 텔레포트 쿨타임 시작
            StartCoroutine(TeleportCooltime(other));
        }
    }

    // 텔레포트 쿨타임 처리
    private IEnumerator TeleportCooltime(Collider2D player)
    {
        canTeleport = false;  // 텔레포트 비활성화
        // 플레이어를 목적지로 이동
        player.transform.position = targetPlatform.position;

        // 플레이어 충돌 감지 비활성화
        targetPlatform.GetComponent<Collider2D>().enabled = false;

        // 쿨타임 동안 대기
        yield return new WaitForSeconds(teleportCooltime);

        // 플레이어 충돌 감지 다시 활성화
        targetPlatform.GetComponent<Collider2D>().enabled = true;

        canTeleport = true;  // 다시 텔레포트 활성화
    }
}
