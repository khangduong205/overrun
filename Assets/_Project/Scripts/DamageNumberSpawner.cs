using UnityEngine;
using UnityEngine.Pool;

public class DamageNumberSpawner : MonoBehaviour
{
    [SerializeField] private DamageNumber prefab;
    [SerializeField] private int maxVisible = 40;

    private ObjectPool<DamageNumber> pool;
    private int active;

    private void Awake()
    {
        pool = new ObjectPool<DamageNumber>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: d => { d.gameObject.SetActive(true); active++; },
            actionOnRelease: d => { d.gameObject.SetActive(false); active--; },
            actionOnDestroy: d => { if (d != null) Destroy(d.gameObject); },
            collectionCheck: false,
            defaultCapacity: 40,
            maxSize: 100
        );
    }

    private void OnEnable() => Enemy.Damaged += HandleDamaged;
    private void OnDisable() => Enemy.Damaged -= HandleDamaged;

    private void HandleDamaged(float amount, Vector2 position)
    {
        if (active >= maxVisible) return;   // chặn rối mắt và chi phí
        int rounded = Mathf.Max(1, Mathf.RoundToInt(amount));
        pool.Get().Show(position, rounded, pool.Release);
    }
}
