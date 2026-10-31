using UnityEngine;

[CreateAssetMenu(menuName = "Overrun/Weapon Data", fileName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private float damageMultiplier = 1f;      // nhân với chỉ số Damage của Player
    [SerializeField] private float fireRateMultiplier = 1f;    // nhân với chỉ số FireRate của Player
    [SerializeField, Min(1)] private int projectilesPerShot = 1;
    [SerializeField] private float spreadAngle = 0f;           // tổng góc quạt (độ)
    [SerializeField, Min(0)] private int pierceCount = 0;      // số quái đạn xuyên qua được
    [SerializeField] private float range = 8f;

    public float DamageMultiplier => damageMultiplier;
    public float FireRateMultiplier => fireRateMultiplier;
    public int ProjectilesPerShot => projectilesPerShot;
    public float SpreadAngle => spreadAngle;
    public int PierceCount => pierceCount;
    public float Range => range;
}