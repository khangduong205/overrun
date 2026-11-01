using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Health))]
public class HitFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float duration = 0.08f;

    private SpriteRenderer sr;
    private Health health;
    private Color baseColor;
    private float timer;
    private float lastHealth;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        baseColor = sr.color;   // lưu màu gốc 1 lần
    }

    private void OnEnable()
    {
        sr.color = baseColor;
        timer = 0f;
        lastHealth = health.Current;
        health.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable() => health.OnHealthChanged -= HandleHealthChanged;

    private void HandleHealthChanged(float current, float max)
    {
        // Chỉ chớp khi mất máu, không chớp khi hồi hoặc Init
        if (current < lastHealth) { timer = duration; sr.color = flashColor; }
        lastHealth = current;
    }

    private void Update()
    {
        if (timer <= 0f) return;
        timer -= Time.deltaTime;
        if (timer <= 0f) sr.color = baseColor;
    }
}