using UnityEngine;

public class AbilityContext
{
    public PlayerStats Stats;
    public Health Health;
}

public abstract class AbilityData : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private float cooldown = 10f;
    [SerializeField] private float duration = 3f;
    [SerializeField] private string unlockId; // If unlockId is left blank, it means the skill is available by default (already unlocked).

    public string UnlockId => unlockId;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public float Cooldown => cooldown;
    public float Duration => duration;

    public abstract void Activate(AbilityContext ctx);
    public abstract void Deactivate(AbilityContext ctx);

    // cooldown is started from the moment of activation, so it cannot be shorter than the duration.
    private void OnValidate() => cooldown = Mathf.Max(cooldown, duration);
}