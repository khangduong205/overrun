using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ScreenShake : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private float hurtForce = 0.5f;

    private CinemachineImpulseSource source;
    private float lastHealth;

    private void Awake() => source = GetComponent<CinemachineImpulseSource>();

    private void OnEnable()
    {
        lastHealth = playerHealth.Current;
        playerHealth.OnHealthChanged += HandleHealth;
    }

    private void OnDisable() => playerHealth.OnHealthChanged -= HandleHealth;

    private void HandleHealth(float current, float max)
    {
        if (current < lastHealth) source.GenerateImpulse(hurtForce);
        lastHealth = current;
    }
}