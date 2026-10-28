using UnityEngine;
using UnityEngine.Pool;

public class AutoWeapon : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float fireInterval = 0.5f;   // giây giữa 2 phát
    [SerializeField] private float range = 8f;            // tầm phát hiện
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask enemyLayer;

    private ObjectPool<Projectile> pool;
    private float fireTimer;

    // Buffer dùng lại, tránh tạo mảng mới mỗi lần quét (không sinh rác)
    private readonly Collider2D[] hitBuffer = new Collider2D[32];

    private void Awake()
    {
        pool = new ObjectPool<Projectile>(
            createFunc: () => Instantiate(projectilePrefab),
            actionOnGet: p => p.gameObject.SetActive(true),
            actionOnRelease: p => p.gameObject.SetActive(false),
            actionOnDestroy: p => Destroy(p.gameObject),
            collectionCheck: false,
            defaultCapacity: 30,
            maxSize: 200
        );
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer > 0f) return;

        Transform target = FindNearestEnemy();
        if (target == null) return;   // không có mục tiêu thì chưa bắn, giữ timer

        Fire(target.position);
        fireTimer = fireInterval;
    }

    private Transform FindNearestEnemy()
    {
        var filter = new ContactFilter2D { useLayerMask = true, layerMask = enemyLayer, useTriggers = true };
        int count = Physics2D.OverlapCircle(transform.position, range, filter, hitBuffer);

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
        Vector2 dir = targetPos - (Vector2)transform.position;
        Projectile p = pool.Get();
        p.Launch(transform.position, dir, damage, pool.Release);
    }
}