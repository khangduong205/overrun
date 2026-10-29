using UnityEngine;
using UnityEngine.Pool;

public class GemSpawner : MonoBehaviour
{
    [SerializeField] private Gem gemPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Experience experience;

    private ObjectPool<Gem> pool;

    private void Awake()
    {
        pool = new ObjectPool<Gem>(
            createFunc: () => Instantiate(gemPrefab),
            actionOnGet: g => g.gameObject.SetActive(true),
            actionOnRelease: g => g.gameObject.SetActive(false),
            actionOnDestroy: g => Destroy(g.gameObject),
            collectionCheck: false,
            defaultCapacity: 100,
            maxSize: 500
        );
    }

    private void OnEnable() => Enemy.Died += HandleEnemyDied;
    private void OnDisable() => Enemy.Died -= HandleEnemyDied;

    private void HandleEnemyDied(Enemy enemy)
    {
        Gem gem = pool.Get();
        gem.Init(enemy.transform.position, player, stats, experience, pool.Release);
    }
}