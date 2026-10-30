using System;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Instance { get; private set; }

    public GameState Current { get; private set; } = GameState.Playing;

    // (trạng thái cũ, trạng thái mới)
    public event Action<GameState, GameState> OnStateChanged;

    private void Awake()
    {
        // Singleton cho scene: chỉ một state machine tồn tại
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Time.timeScale = 1f;   // scene vừa nạp (kể cả Restart) luôn bắt đầu chạy bình thường
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

        // Toàn bộ logic tạm dừng nằm ở đây, một chỗ duy nhất
        Time.timeScale = next == GameState.Playing ? 1f : 0f;

        OnStateChanged?.Invoke(previous, next);
    }

    // Win và Lose là trạng thái cuối, không thoát ra được (chỉ nạp lại scene)
    private static bool IsTransitionAllowed(GameState from, GameState to)
    {
        if (from == to) return false;
        if (from == GameState.Win || from == GameState.Lose) return false;
        return true;
    }
}