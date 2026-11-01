using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DeathEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float startScale = 0.6f;
    [SerializeField] private float endScale = 0.2f;        // co nhỏ lại thay vì nở ra
    [SerializeField, Range(0f, 1f)] private float startAlpha = 0.35f;

    private SpriteRenderer sr;
    private float timer;
    private Action<DeathEffect> releaseToPool;

    private void Awake() => sr = GetComponent<SpriteRenderer>();

    public void Play(Vector2 position, Color color, Action<DeathEffect> releaseCallback)
    {
        releaseToPool = releaseCallback;
        transform.position = position;
        sr.color = new Color(color.r, color.g, color.b, startAlpha);
        timer = 0f;
        transform.localScale = Vector3.one * startScale;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = duration > 0f ? timer / duration : 1f;   // tránh chia cho 0

        transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, t);

        float fade = (1f - t) * (1f - t);
        Color c = sr.color;
        c.a = startAlpha * fade;
        sr.color = c;

        if (t >= 1f) releaseToPool?.Invoke(this);
    }
}