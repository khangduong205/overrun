using UnityEngine;

[CreateAssetMenu(menuName = "Overrun/Upgrade Data", fileName = "UpgradeData")]
public class UpgradeData : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private StatType stat;
    [SerializeField] private ModifierMode mode;
    [SerializeField] private float value;

    public string DisplayName => displayName;
    public string Description => description;

    public void Apply(PlayerStats stats) => stats.AddModifier(stat, value, mode);
}