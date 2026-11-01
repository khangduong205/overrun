using System;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Instance { get; private set; }

    public GameState Current { get; private set; } = GameState.Playing;

    public event Action<GameState, GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Time.timeScale = 1f;  
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void ChangeState(GameState next)
    {
        if (!IsTransitionAllowed(Current, next)) return;

        GameState previous = Current;
        Current = next;


        Time.timeScale = next == GameState.Playing ? 1f : 0f;

        OnStateChanged?.Invoke(previous, next);
    }

    // Win and Lose are final states (can only reload scene)
    private static bool IsTransitionAllowed(GameState from, GameState to)
    {
        if (from == to) return false;
        if (from == GameState.Win || from == GameState.Lose) return false;
        return true;
    }
}