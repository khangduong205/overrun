using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Overrun/Abilities/Stat Boost", fileName = "StatBoostAbility")]
public class StatBoostAbility : AbilityData
{
    [Serializable]
    private struct Modifier
    {
        public StatType stat;
        public ModifierMode mode;
        public float value;
    }

    [SerializeField] private Modifier[] modifiers;

    public override void Activate(AbilityContext ctx)
    {
        foreach (var m in modifiers) ctx.Stats.AddModifier(m.stat, m.value, m.mode);
    }

    public override void Deactivate(AbilityContext ctx)
    {
        foreach (var m in modifiers) ctx.Stats.RemoveModifier(m.stat, m.value, m.mode);
    }
}