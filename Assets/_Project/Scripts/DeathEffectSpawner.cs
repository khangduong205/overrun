using UnityEngine;
using UnityEngine.Pool;

public class DeathEffectSpawner : MonoBehaviour
{
    [SerializeField] private DeathEffect prefab;
    [SerializeField] private Color color = new Color(1f, 1f, 1f, 0.8f);

    private ObjectPool<DeathEffect> pool;

    private void Awake()
    {
        pool = new ObjectPool<DeathEffect>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: e => e.gameObject.SetActive(true),
            actionOnRelease: e => e.gameObject.SetActive(false),
            actionOnDestroy: e => { if (e != null) Destroy(e.gameObject); },
            collectionCheck: false,
            defaultCapacity: 40,
            maxSize: 120
        );
    }

    private void OnEnable() => Enemy.Died += HandleEnemyDied;
    private void OnDisable() => Enemy.Died -= HandleEnemyDied;

    private void HandleEnemyDied(Enemy enemy) =>
        pool.Get().Play(enemy.transform.position, color, pool.Release);
}