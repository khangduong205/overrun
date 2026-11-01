using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class DamageNumber : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.6f;
    [SerializeField] private float riseSpeed = 1.5f;

    private TextMeshPro text;
    private float timer;
    private Color baseColor;
    private Action<DamageNumber> releaseToPool;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        baseColor = text.color;
    }

    public void Show(Vector2 position, int amount, Action<DamageNumber> releaseCallback)
    {
        releaseToPool = releaseCallback;
        // Add slight random X offset to avoid exact overlap
        transform.position = position + new Vector2(UnityEngine.Random.Range(-0.3f, 0.3f), 0.4f);
        text.text = amount.ToString();
        text.color = baseColor;
        timer = lifetime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        transform.position += Vector3.up * (riseSpeed * Time.deltaTime);

        // Fade out during the second half of lifetime
        float alpha = Mathf.Clamp01(timer / (lifetime * 0.5f));
        text.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

        if (timer <= 0f) releaseToPool?.Invoke(this);
    }
}