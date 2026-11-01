using UnityEngine;

[CreateAssetMenu(menuName = "Overrun/Weapon Data", fileName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private float damageMultiplier = 1f;      
    [SerializeField] private float fireRateMultiplier = 1f;    
    [SerializeField, Min(1)] private int projectilesPerShot = 1;
    [SerializeField] private float spreadAngle = 0f;           // Shotgun spread angle (degrees)
    [SerializeField, Min(0)] private int pierceCount = 0;      // Number of enemies a projectile can pierce
    [SerializeField] private float range = 8f;

    public float DamageMultiplier => damageMultiplier;
    public float FireRateMultiplier => fireRateMultiplier;
    public int ProjectilesPerShot => projectilesPerShot;
    public float SpreadAngle => spreadAngle;
    public int PierceCount => pierceCount;
    public float Range => range;
}