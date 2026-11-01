using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private TMP_Text bestText;
    private void Awake()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene(gameSceneName));
        quitButton.onClick.AddListener(Quit);
        int t = (int)PlayerProfile.BestTime;
        bestText.text = $"Best: {t / 60:00}:{t % 60:00}  |  {PlayerProfile.BestKills} kills";
    }

    private static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // Exit Play mode in Editor
#else
        Application.Quit();
#endif
    }
}