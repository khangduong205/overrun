
using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { Damage, FireRate, MoveSpeed, MaxHealth, MagnetRadius }

public enum ModifierMode { Additive, Multiplicative }

public class PlayerStats : MonoBehaviour
{
    [Serializable]
    private struct BaseStat
    {
        public StatType type;
        public float value;
    }

    [SerializeField]
    private List<BaseStat> baseStats = new()
    {
        new BaseStat { type = StatType.Damage,       value = 10f },
        new BaseStat { type = StatType.FireRate,     value = 2f },
        new BaseStat { type = StatType.MoveSpeed,    value = 5f },
        new BaseStat { type = StatType.MaxHealth,    value = 100f },
        new BaseStat { type = StatType.MagnetRadius, value = 2f },
    };

    private readonly Dictionary<StatType, float> additive = new();
    private readonly Dictionary<StatType, float> multiplier = new();
    private readonly Dictionary<StatType, float> baseValues = new();

    public event Action<StatType> OnStatChanged;

    private void Awake()
    {
        foreach (var s in baseStats)
        {
            baseValues[s.type] = s.value;
            additive[s.type] = 0f;
            multiplier[s.type] = 1f;
        }
    }

    public float Get(StatType type) =>
        (baseValues[type] + additive[type]) * multiplier[type];

    public void AddModifier(StatType type, float value, ModifierMode mode)
    {
        if (mode == ModifierMode.Additive) additive[type] += value;
        else multiplier[type] *= value;

        OnStatChanged?.Invoke(type);
    }
}
