using UnityEngine;
using UnityEngine.SceneManagement;

// 빌드 화면을 에디터(1920x1080)와 같은 16:9 비율로 고정한다.
// 모니터 비율이 16:9가 아니면(울트라와이드 등) 남는 부분을 검은 여백으로 채운다.
// 씬에 따로 붙일 필요 없음 (게임 시작 시 자동 생성, 모든 씬에 적용).
public class AspectRatioEnforcer : MonoBehaviour
{
    private const float TargetAspect = 16f / 9f;

    private static AspectRatioEnforcer instance;
    private Camera backgroundCamera;
    private int lastWidth;
    private int lastHeight;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Create()
    {
        if (instance != null) return;
        var go = new GameObject("[AspectRatioEnforcer]");
        DontDestroyOnLoad(go);
        instance = go.AddComponent<AspectRatioEnforcer>();
    }

    private void Awake()
    {
        // 여백을 검게 칠하는 배경 카메라: 아무것도 그리지 않고 화면 전체를 검정으로 지운다.
        backgroundCamera = gameObject.AddComponent<Camera>();
        backgroundCamera.clearFlags = CameraClearFlags.SolidColor;
        backgroundCamera.backgroundColor = Color.black;
        backgroundCamera.cullingMask = 0;
        backgroundCamera.depth = -100;
        backgroundCamera.orthographic = true;
        backgroundCamera.rect = new Rect(0f, 0f, 1f, 1f);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Apply();
    }

    private void Update()
    {
        // 창 크기가 바뀌면 다시 계산
        if (Screen.width != lastWidth || Screen.height != lastHeight)
            Apply();
    }

    private void Apply()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        if (lastWidth <= 0 || lastHeight <= 0) return;

        // 1) 게임 카메라를 화면 가운데 16:9 영역에만 그리게 한다.
        Rect viewRect = CalculateViewRect();
        foreach (Camera cam in Camera.allCameras)
        {
            if (cam == backgroundCamera || cam.targetTexture != null) continue;
            cam.rect = viewRect;
        }

        // 2) 화면 전체를 덮는 UI(Overlay, 카메라 없는 Camera 모드)는 카메라 영역을 따르도록 바꾼다.
        //    그대로 두면 UI만 검은 여백까지 늘어난다.
        Camera mainCam = Camera.main;
        if (mainCam == null) return;
        int topSortingLayer = SortingLayer.layers[SortingLayer.layers.Length - 1].id;

        foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (!canvas.isRootCanvas) continue;
            bool isOverlay = canvas.renderMode == RenderMode.ScreenSpaceOverlay;
            bool cameraModeWithoutCamera = canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null;
            if (!isOverlay && !cameraModeWithoutCamera) continue;

            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = mainCam;
            if (isOverlay) canvas.planeDistance = 1f;
            // Overlay처럼 항상 스프라이트 위에 보이도록 가장 위 정렬 레이어로 올린다.
            canvas.sortingLayerID = topSortingLayer;
            canvas.sortingOrder = Mathf.Clamp(canvas.sortingOrder + 1000, -32768, 32767);
        }
    }

    private static Rect CalculateViewRect()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        if (screenAspect > TargetAspect)
        {
            // 화면이 더 넓음: 좌우에 검은 여백
            float width = TargetAspect / screenAspect;
            return new Rect((1f - width) / 2f, 0f, width, 1f);
        }
        else
        {
            // 화면이 더 높음: 위아래에 검은 여백
            float height = screenAspect / TargetAspect;
            return new Rect(0f, (1f - height) / 2f, 1f, height);
        }
    }
}
