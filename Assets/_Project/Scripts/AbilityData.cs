using UnityEngine;

// Ngữ cảnh truyền cho skill: những thứ skill được phép tác động
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
    [SerializeField] private string unlockId; // để trống = mở sẵn

    public string UnlockId => unlockId;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public float Cooldown => cooldown;
    public float Duration => duration;

    public abstract void Activate(AbilityContext ctx);
    public abstract void Deactivate(AbilityContext ctx);

    // Cooldown tính từ lúc kích hoạt nên không được ngắn hơn thời lượng
    private void OnValidate() => cooldown = Mathf.Max(cooldown, duration);
}