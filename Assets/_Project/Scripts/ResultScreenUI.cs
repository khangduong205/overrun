using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScreenUI : MonoBehaviour
{
    [SerializeField] private GameStateMachine stateMachine;
    [SerializeField] private GameSession session;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject upgradePanel;   // để tắt nếu Player chết giữa lúc chọn nâng cấp
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    [SerializeField] private string menuSceneName = "MainMenu";

    private void OnEnable()
    {
        stateMachine.OnStateChanged += HandleStateChanged;
        restartButton.onClick.AddListener(Restart);
        menuButton.onClick.AddListener(GoToMenu);
    }

    private void OnDisable()
    {
        stateMachine.OnStateChanged -= HandleStateChanged;
        restartButton.onClick.RemoveListener(Restart);
        menuButton.onClick.RemoveListener(GoToMenu);
    }

    private void HandleStateChanged(GameState previous, GameState next)
    {
        if (next != GameState.Win && next != GameState.Lose) return;

        upgradePanel.SetActive(false);
        resultPanel.SetActive(true);
        titleText.text = next == GameState.Win ? "YOU WIN" : "GAME OVER";

        int sec = (int)session.Elapsed;
        statsText.text =
            $"Time: {sec / 60:00}:{sec % 60:00}\n" +
            $"Kills: {session.Kills}\n" +
            $"Level: {session.Level}";
    }

    private void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    private void GoToMenu() => SceneManager.LoadScene(menuSceneName);
}