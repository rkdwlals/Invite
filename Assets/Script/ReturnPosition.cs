using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 미니게임(CardScene/PuzzleScene/SwordScene)이 끝나고 Main으로 돌아올 때
// 처음 스폰 위치가 아니라, 미니게임에 들어가기 직전 플레이어 위치로 되돌린다.
// 씬에 따로 붙일 필요 없음 (게임 시작 시 자동 등록).
//
// v3: Main이 여러 번 로드돼도(미니게임 스크립트 중복 호출 등) 같은 위치로 복귀하도록
//     돌아갈 위치를 다음 미니게임 진입 전까지 유지한다.
// v2: DialogueManager에서 Save()를 부르지 못한 경우에도 동작하도록,
//     Main 씬에 있는 동안 플레이어 위치를 매 프레임 기록해 두고
//     미니게임 씬으로 넘어가는 순간 그 값을 저장한다.
public static class ReturnPosition
{
    private const string MainSceneName = "Main";
    private static readonly HashSet<string> MiniGameScenes =
        new HashSet<string> { "CardScene", "PuzzleScene", "SwordScene" };

    private static bool hasSaved;
    private static Vector3 savedPosition;
    private static float savedFacingX = 1f;

    // 미니게임에서 Main으로 돌아올 때 쓸 위치 (다음 미니게임 진입 전까지 유지)
    private static bool hasReturn;
    private static Vector3 returnPosition;
    private static float returnFacingX = 1f;

    // Main에 있는 동안 마지막으로 기록된 위치
    private static bool hasTracked;
    private static Vector3 trackedPosition;
    private static float trackedFacingX = 1f;

    // DialogueManager에서 미니게임 씬을 불러오기 직전에 호출 (기존 호출 유지)
    public static void Save()
    {
        var player = Object.FindFirstObjectByType<EthanTheHero.PlayerMovement>();
        if (player == null)
        {
            Debug.LogWarning("[ReturnPosition] Save: 플레이어를 찾지 못함");
            return;
        }

        savedPosition = player.transform.position;
        savedFacingX = player.transform.localScale.x;
        hasSaved = true;
        Debug.Log("[ReturnPosition] Save: " + savedPosition);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        hasSaved = false;
        hasTracked = false;
        hasReturn = false;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateTracker()
    {
        var go = new GameObject("[ReturnPositionTracker]");
        go.hideFlags = HideFlags.HideInHierarchy;
        Object.DontDestroyOnLoad(go);
        go.AddComponent<Tracker>();
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 미니게임으로 들어감: Save()가 안 불렸으면 Main에서 마지막으로 기록한 위치를 사용
        if (MiniGameScenes.Contains(scene.name))
        {
            if (hasSaved)
            {
                returnPosition = savedPosition;
                returnFacingX = savedFacingX;
                hasReturn = true;
            }
            else if (hasTracked)
            {
                returnPosition = trackedPosition;
                returnFacingX = trackedFacingX;
                hasReturn = true;
                Debug.Log("[ReturnPosition] " + scene.name + " 진입, 기록된 위치 사용: " + returnPosition);
            }
            hasSaved = false;
            hasTracked = false;
            return;
        }

        if (scene.name != MainSceneName)
        {
            // 시작 화면, 엔딩 등으로 가면 저장값을 버림
            hasSaved = false;
            hasTracked = false;
            hasReturn = false;
            return;
        }

        // Main으로 돌아옴 (값은 비우지 않음: Main이 연달아 다시 로드돼도 같은 위치 사용)
        if (!hasReturn)
        {
            Debug.Log("[ReturnPosition] Main 로드: 저장된 위치 없음, 기본 스폰 사용");
            return;
        }
        Vector3 pos = returnPosition;
        float facing = returnFacingX;

        var player = Object.FindFirstObjectByType<EthanTheHero.PlayerMovement>();
        if (player == null)
        {
            Debug.LogWarning("[ReturnPosition] Main 로드: 플레이어를 찾지 못함");
            return;
        }

        player.transform.position = pos;
        Vector3 s = player.transform.localScale;
        s.x = facing;
        player.transform.localScale = s;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.position = pos;
            rb.linearVelocity = Vector2.zero;
        }

        // 카메라가 처음 자리에서 미끄러져 오지 않도록 바로 맞춤
        var cam = Object.FindFirstObjectByType<CameraFollow>();
        if (cam != null)
            cam.transform.position = new Vector3(pos.x, pos.y, -10f);

        // 복원 직후 위치를 기록 위치로도 반영
        trackedPosition = pos;
        trackedFacingX = facing;
        hasTracked = true;

        Debug.Log("[ReturnPosition] Main 로드: " + pos + " 로 복귀");
    }

    // Main 씬에 있는 동안 플레이어 위치를 계속 기록
    private class Tracker : MonoBehaviour
    {
        private EthanTheHero.PlayerMovement player;

        private void LateUpdate()
        {
            if (SceneManager.GetActiveScene().name != MainSceneName) return;

            if (player == null)
                player = Object.FindFirstObjectByType<EthanTheHero.PlayerMovement>();
            if (player == null) return;

            trackedPosition = player.transform.position;
            trackedFacingX = player.transform.localScale.x;
            hasTracked = true;
        }
    }
}