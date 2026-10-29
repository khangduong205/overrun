using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Health))]
public class Enemy : MonoBehaviour
{
    // Hệ XP, thống kê, âm thanh sẽ lắng nghe event này
    public static event Action<Enemy> Died;

    private Rigidbody2D rb;
    private Health health;
    private Transform target;
    private Action<Enemy> releaseToPool;
    private float contactTimer;

    public EnemyData Data { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    // Đăng ký/huỷ event theo OnEnable/OnDisable: bắt buộc với object dùng pool
    private void OnEnable() => health.OnDied += HandleDied;
    private void OnDisable() => health.OnDied -= HandleDied;

    public void Init(EnemyData data, Vector2 position, Transform chaseTarget,
                     Action<Enemy> releaseCallback)
    {
        Data = data;
        target = chaseTarget;
        releaseToPool = releaseCallback;
        contactTimer = 0f;

        transform.position = position;
        rb.position = position;
        rb.linearVelocity = Vector2.zero;
        health.Init(data.MaxHealth);
    }

    private void Update() => contactTimer -= Time.deltaTime;

    private void FixedUpdate()
    {
        if (target == null) return;
        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.linearVelocity = dir * Data.MoveSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (contactTimer > 0f) return;
        if (!collision.collider.CompareTag("Player")) return;

        if (collision.collider.TryGetComponent<IDamageable>(out var player))
        {
            player.TakeDamage(Data.ContactDamage);
            contactTimer = Data.ContactInterval;
        }
    }

    private void HandleDied()
    {
        Died?.Invoke(this);
        releaseToPool?.Invoke(this);
    }
}