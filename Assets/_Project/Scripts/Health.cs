using System;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount);
}

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float current;   

    public event Action<float, float> OnHealthChanged;  
    public event Action OnDied;

    public event Action<float, Vector2> OnDamaged;

    public float Current => current;
    public float Max => maxHealth;
    public bool IsDead => current <= 0f;
    public bool IsInvulnerable { get; private set; }
    public void SetInvulnerable(bool value) => IsInvulnerable = value;
    private void Awake() => current = maxHealth;

    // Reinitialize pooled enemy on spawn
    public void Init(float max)
    {
        maxHealth = max;
        current = max;
        OnHealthChanged?.Invoke(current, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || IsInvulnerable) return;
        float before = current;
        current = Mathf.Max(0f, current - amount);
        OnHealthChanged?.Invoke(current, maxHealth);
        OnDamaged?.Invoke(before - current, transform.position);
        if (IsDead) OnDied?.Invoke();
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        current = Mathf.Min(maxHealth, current + amount);
        OnHealthChanged?.Invoke(current, maxHealth);
    }

    public void IncreaseMax(float amount)
    {
        maxHealth += amount;
        Heal(amount);
    }
}