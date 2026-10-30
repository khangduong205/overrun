using UnityEngine;

public class DifficultyScaler : MonoBehaviour
{
    [SerializeField] private GameSession session;

    // Trục X của curve: 0 = lúc bắt đầu, 1 = lúc thắng (đủ 5 phút)
    [SerializeField] private AnimationCurve healthCurve = AnimationCurve.Linear(0f, 1f, 1f, 4f);
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.Linear(0f, 1f, 1f, 1.35f);
    [SerializeField] private AnimationCurve damageCurve = AnimationCurve.Linear(0f, 1f, 1f, 2f);

    private float Progress => Mathf.Clamp01(session.Elapsed / session.WinTime);

    public float HealthMultiplier => healthCurve.Evaluate(Progress);
    public float SpeedMultiplier => speedCurve.Evaluate(Progress);
    public float DamageMultiplier => damageCurve.Evaluate(Progress);
}