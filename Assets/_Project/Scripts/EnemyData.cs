using UnityEngine;

[CreateAssetMenu(menuName = "Overrun/Enemy Data", fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private Enemy prefab;
    [SerializeField] private float maxHealth = 20f;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float contactDamage = 8f;
    [SerializeField] private float contactInterval = 0.8f;   // giây giữa 2 lần gây sát thương

    public Enemy Prefab => prefab;
    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float ContactDamage => contactDamage;
    public float ContactInterval => contactInterval;
}