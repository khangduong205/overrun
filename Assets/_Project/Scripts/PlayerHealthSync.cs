using UnityEngine;

[RequireComponent(typeof(Health), typeof(PlayerStats))]
public class PlayerHealthSync : MonoBehaviour
{
    private Health health;
    private PlayerStats stats;
    private float lastMax;

    private void Awake()
    {
        health = GetComponent<Health>();
        stats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        lastMax = stats.Get(StatType.MaxHealth);
        health.Init(lastMax);
    }

    private void OnEnable() => stats.OnStatChanged += HandleStatChanged;
    private void OnDisable() => stats.OnStatChanged -= HandleStatChanged;

    private void HandleStatChanged(StatType type)
    {
        if (type != StatType.MaxHealth) return;
        float newMax = stats.Get(StatType.MaxHealth);
        health.IncreaseMax(newMax - lastMax);   // Add to max HP and heal by the same amount
        lastMax = newMax;
    }
}