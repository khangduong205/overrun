using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    private class SpawnEntry
    {
        public EnemyData data;
        public float unlockTime;      // giây kể từ lúc bắt đầu, sau đó loại này mới xuất hiện
        public float weight = 1f;     // trọng số chọn ngẫu nhiên
    }

    [SerializeField] private Transform player;
    [SerializeField] private List<SpawnEntry> entries = new();
    [SerializeField] private DifficultyScaler difficulty;

    [Header("Nhịp spawn")]
    [SerializeField] private float startInterval = 1.2f;
    [SerializeField] private float minInterval = 0.15f;
    [SerializeField] private float rampDuration = 240f;   // sau bao lâu thì đạt nhịp nhanh nhất
    [SerializeField] private int maxAlive = 250;

    [Header("Vị trí spawn")]
    [SerializeField] private float spawnRadius = 12f;     // ngoài tầm nhìn camera
    [SerializeField] private Vector2 arenaHalfSize = new(14f, 9f);

    private readonly Dictionary<EnemyData, ObjectPool<Enemy>> pools = new();
    private float elapsed;
    private float spawnTimer;
    private int aliveCount;

    private void Awake()
    {
        // Mỗi loại quái có pool riêng
        foreach (var entry in entries)
        {
            var data = entry.data;
            ObjectPool<Enemy> pool = null;
            pool = new ObjectPool<Enemy>(
                createFunc: () => Instantiate(data.Prefab),
                actionOnGet: e => { e.gameObject.SetActive(true); aliveCount++; },
                actionOnRelease: e => { e.gameObject.SetActive(false); aliveCount--; },
                actionOnDestroy: e => Destroy(e.gameObject),
                collectionCheck: false,
                defaultCapacity: 50,
                maxSize: 300
            );
            pools[data] = pool;
        }
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        spawnTimer -= Time.deltaTime;
        if (spawnTimer > 0f) return;

        float t = Mathf.Clamp01(elapsed / rampDuration);
        spawnTimer = Mathf.Lerp(startInterval, minInterval, t);

        if (aliveCount >= maxAlive) return;
        Spawn();
    }

    private void Spawn()
    {
        EnemyData data = PickEnemy();
        if (data == null) return;

        // Điểm ngẫu nhiên trên vòng tròn quanh Player, kẹp lại trong đấu trường
        Vector2 offset = UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
        Vector2 pos = (Vector2)player.position + offset;
        pos.x = Mathf.Clamp(pos.x, -arenaHalfSize.x, arenaHalfSize.x);
        pos.y = Mathf.Clamp(pos.y, -arenaHalfSize.y, arenaHalfSize.y);

        var pool = pools[data];
        Enemy enemy = pool.Get();

        var scaling = new EnemyScaling(
            difficulty.HealthMultiplier,
            difficulty.SpeedMultiplier,
            difficulty.DamageMultiplier
        );

        enemy.Init(data, pos, player, pool.Release, scaling);
    }

    // Chọn ngẫu nhiên theo trọng số trong các loại đã mở khoá (không cấp phát bộ nhớ)
    private EnemyData PickEnemy()
    {
        float total = 0f;
        foreach (var e in entries)
            if (elapsed >= e.unlockTime) total += e.weight;
        if (total <= 0f) return null;

        float roll = UnityEngine.Random.value * total;
        foreach (var e in entries)
        {
            if (elapsed < e.unlockTime) continue;
            roll -= e.weight;
            if (roll <= 0f) return e.data;
        }
        return null;
    }
}