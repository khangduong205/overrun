using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string gameSceneName = "Game";

    private void Awake()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene(gameSceneName));
        quitButton.onClick.AddListener(Quit);
    }

    private static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // trong Editor thì dừng Play
#else
        Application.Quit();
#endif
    }
}