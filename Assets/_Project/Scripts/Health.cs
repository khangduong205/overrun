using System;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount);
}

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float current;   // private nhưng xem được trong Inspector ở chế độ Debug

    public event Action<float, float> OnHealthChanged;   // (current, max)
    public event Action OnDied;

    public float Current => current;
    public float Max => maxHealth;
    public bool IsDead => current <= 0f;

    private void Awake() => current = maxHealth;

    // Quái dùng pool nên cần reset lại mỗi lần được lấy ra
    public void Init(float max)
    {
        maxHealth = max;
        current = max;
        OnHealthChanged?.Invoke(current, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;   // chặn nhận sát thương sau khi đã chết
        current = Mathf.Max(0f, current - amount);
        OnHealthChanged?.Invoke(current, maxHealth);
        if (IsDead) OnDied?.Invoke();
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        current = Mathf.Min(maxHealth, current + amount);
        OnHealthChanged?.Invoke(current, maxHealth);
    }

    // Dùng cho nâng cấp Max HP ở Ngày 4
    public void IncreaseMax(float amount)
    {
        maxHealth += amount;
        Heal(amount);
    }
}