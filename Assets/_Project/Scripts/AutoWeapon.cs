using UnityEngine;
using UnityEngine.Pool;

public class AutoWeapon : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private ShopCatalog catalog;     // để tra vũ khí đang trang bị
    [SerializeField] private LayerMask enemyLayer;

    public event System.Action OnFired;

    private WeaponData weapon;
    private ObjectPool<Projectile> pool;
    private float fireTimer;
    private readonly Collider2D[] hitBuffer = new Collider2D[32];

    private void Awake()
    {
        weapon = catalog.GetEquippedWeapon();   // đọc một lần lúc vào game

        pool = new ObjectPool<Projectile>(
            createFunc: () => Instantiate(projectilePrefab),
            actionOnGet: p => p.gameObject.SetActive(true),
            actionOnRelease: p => p.gameObject.SetActive(false),
            actionOnDestroy: p => { if (p != null) Destroy(p.gameObject); },
            collectionCheck: false,
            defaultCapacity: 60,
            maxSize: 400
        );
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer > 0f) return;

        Transform target = FindNearestEnemy();
        if (target == null) return;

        Fire(target.position);
        fireTimer = 1f / (stats.Get(StatType.FireRate) * weapon.FireRateMultiplier);
    }

    private Transform FindNearestEnemy()
    {
        var filter = new ContactFilter2D { useLayerMask = true, layerMask = enemyLayer, useTriggers = true };
        int count = Physics2D.OverlapCircle(transform.position, weapon.Range, filter, hitBuffer);

        Transform nearest = null;
        float bestSqrDist = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            float sqrDist = ((Vector2)hitBuffer[i].transform.position - (Vector2)transform.position).sqrMagnitude;
            if (sqrDist < bestSqrDist)
            {
                bestSqrDist = sqrDist;
                nearest = hitBuffer[i].transform;
            }
        }
        return nearest;
    }

    private void Fire(Vector2 targetPos)
    {
        Vector2 baseDir = targetPos - (Vector2)transform.position;
        float damage = stats.Get(StatType.Damage) * weapon.DamageMultiplier;
        int count = weapon.ProjectilesPerShot;
        float half = weapon.SpreadAngle * 0.5f;

        for (int i = 0; i < count; i++)
        {
            // Rải đều các viên trong góc quạt; 1 viên thì bắn thẳng
            float angle = count == 1 ? 0f : Mathf.Lerp(-half, half, i / (float)(count - 1));
            Vector2 dir = Quaternion.Euler(0f, 0f, angle) * baseDir;

            Projectile p = pool.Get();
            p.Launch(transform.position, dir, damage, weapon.PierceCount, pool.Release);
        }

        OnFired?.Invoke();   // một tiếng bắn cho cả loạt đạn
    }
}