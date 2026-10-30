using System;
using UnityEngine;

public readonly struct EnemyScaling
{
    public readonly float Health, Speed, Damage;

    public EnemyScaling(float health, float speed, float damage)
    {
        Health = health;
        Speed = speed;
        Damage = damage;
    }

    public static EnemyScaling None => new EnemyScaling(1f, 1f, 1f);
}

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
    private float speed;
    private float contactDamage;

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
                     Action<Enemy> releaseCallback, EnemyScaling scaling)
    {
        Data = data;
        target = chaseTarget;
        releaseToPool = releaseCallback;
        contactTimer = 0f;

        speed = data.MoveSpeed * scaling.Speed;
        contactDamage = data.ContactDamage * scaling.Damage;

        transform.position = position;
        rb.position = position;
        rb.linearVelocity = Vector2.zero;
        health.Init(data.MaxHealth * scaling.Health);
    }

    private void Update() => contactTimer -= Time.deltaTime;

    private void FixedUpdate()
    {
        if (target == null) return;
        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.linearVelocity = dir * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (contactTimer > 0f) return;
        if (!collision.collider.CompareTag("Player")) return;

        if (collision.collider.TryGetComponent<IDamageable>(out var player))
        {
            player.TakeDamage(contactDamage);
            contactTimer = Data.ContactInterval;
        }
    }

    private void HandleDied()
    {
        Died?.Invoke(this);
        releaseToPool?.Invoke(this);
    }
}