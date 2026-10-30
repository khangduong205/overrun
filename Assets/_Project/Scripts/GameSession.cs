using System;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private float winTimeSeconds = 300f;   // sống đủ 5 phút
    [SerializeField] private Health playerHealth;
    [SerializeField] private Experience experience;

    public float Elapsed { get; private set; }
    public int Kills { get; private set; }
    public int Level => experience.Level;
    public float WinTime => winTimeSeconds;

    private void OnEnable()
    {
        playerHealth.OnDied += HandlePlayerDied;
        Enemy.Died += HandleEnemyDied;
    }

    private void OnDisable()
    {
        playerHealth.OnDied -= HandlePlayerDied;
        Enemy.Died -= HandleEnemyDied;
    }

    private void Update()
    {
        // Chỉ đếm giờ khi đang chơi, tự dừng ở LevelUp/Win/Lose
        if (GameStateMachine.Instance.Current != GameState.Playing) return;

        Elapsed += Time.deltaTime;
        if (Elapsed >= winTimeSeconds)
        {
            Elapsed = winTimeSeconds;
            GameStateMachine.Instance.ChangeState(GameState.Win);
        }
    }

    private void HandleEnemyDied(Enemy _) => Kills++;

    private void HandlePlayerDied() => GameStateMachine.Instance.ChangeState(GameState.Lose);
}