using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScreenUI : MonoBehaviour
{
    [SerializeField] private GameStateMachine stateMachine;
    [SerializeField] private GameSession session;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject upgradePanel;
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
        int coins = session.AwardCoins(next == GameState.Win);
        bool newRecord = PlayerProfile.SubmitRun(session.Elapsed, session.Kills);
        statsText.text =
            $"Time: {sec / 60:00}:{sec % 60:00}\n" +
            $"Kills: {session.Kills}\n" +
            $"Level: {session.Level}\n" +
            $"Coins: +{coins}\n" +
            (newRecord ? "NEW RECORD!\n" : "") +
            $"Best: {(int)PlayerProfile.BestTime / 60:00}:{(int)PlayerProfile.BestTime % 60:00} | {PlayerProfile.BestKills} kills";
    }

    private void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    private void GoToMenu() => SceneManager.LoadScene(menuSceneName);
}