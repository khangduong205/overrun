using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 2f;

    private Rigidbody2D rb;
    private float lifeTimer;
    private bool isActive;
    private int pierceLeft;
    private Action<Projectile> releaseToPool;

    public float Damage { get; private set; }

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    public void Launch(Vector2 position, Vector2 direction, float damage, int pierce,
                       Action<Projectile> releaseCallback)
    {
        transform.position = position;
        Damage = damage;
        pierceLeft = pierce;
        releaseToPool = releaseCallback;
        lifeTimer = lifetime;
        isActive = true;
        rb.linearVelocity = direction.normalized * speed;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) ReleaseSelf();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(Damage);

            // Còn lượt xuyên thì bay tiếp, hết thì trả về pool
            if (pierceLeft > 0) pierceLeft--;
            else ReleaseSelf();
        }
    }

    private void ReleaseSelf()
    {
        if (!isActive) return;
        isActive = false;
        rb.linearVelocity = Vector2.zero;
        releaseToPool?.Invoke(this);
    }
}