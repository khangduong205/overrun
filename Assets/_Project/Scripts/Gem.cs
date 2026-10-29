using System;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private int xpValue = 1;
    [SerializeField] private float flySpeed = 9f;

    private Transform player;
    private PlayerStats stats;
    private Experience experience;
    private Action<Gem> releaseToPool;
    private bool isFlying;

    public void Init(Vector2 position, Transform playerTransform, PlayerStats playerStats,
                     Experience playerXp, Action<Gem> releaseCallback)
    {
        transform.position = position;
        player = playerTransform;
        stats = playerStats;
        experience = playerXp;
        releaseToPool = releaseCallback;
        isFlying = false;
    }

    private void Update()
    {
        Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;

        // Vào bán kính hút thì bắt đầu bay, đã bay thì không dừng nữa
        if (!isFlying && toPlayer.sqrMagnitude <= Sq(stats.Get(StatType.MagnetRadius)))
            isFlying = true;

        if (!isFlying) return;

        transform.position = Vector2.MoveTowards(transform.position, player.position,
                                                 flySpeed * Time.deltaTime);

        if (toPlayer.sqrMagnitude < 0.04f)   // cách Player dưới 0.2 đơn vị thì coi là nhặt
        {
            experience.AddXp(xpValue);
            releaseToPool?.Invoke(this);
        }
    }

    private static float Sq(float v) => v * v;
}