using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 2f;

    private Rigidbody2D rb;
    private float lifeTimer;

    // Pool đăng ký callback này để biết khi nào đạn muốn "trả về"
    private Action<Projectile> releaseToPool;

    public float Damage { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Gọi bởi pool ngay sau khi lấy đạn ra
    public void Launch(Vector2 position, Vector2 direction, float damage,
                       Action<Projectile> releaseCallback)
    {
        transform.position = position;
        Damage = damage;
        releaseToPool = releaseCallback;
        lifeTimer = lifetime;
        rb.linearVelocity = direction.normalized * speed;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) ReleaseSelf();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // Ngày 3 sẽ thay đoạn này bằng interface IDamageable
        if (other.CompareTag("Enemy"))
        {
            ReleaseSelf();
        }
    }

    private void ReleaseSelf()
    {
        rb.linearVelocity = Vector2.zero;
        releaseToPool?.Invoke(this);
    }
}