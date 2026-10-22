using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        // 에디터에서 플레이 모드일 때는 에디터를 종료하지 않음
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 플레이 모드 종료
#else
        Application.Quit(); // 빌드에서 게임 종료
#endif
    }
}

