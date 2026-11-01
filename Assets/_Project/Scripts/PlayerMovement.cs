using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private InputActionReference moveAction;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        if (stats == null)
            stats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() => moveAction.action.Enable();
    private void OnDisable() => moveAction.action.Disable();

    private void Update()
    {
        // read input each frame
        // Keep diagonal speed same as straight
        moveInput = Vector2.ClampMagnitude(moveAction.action.ReadValue<Vector2>(), 1f);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * stats.Get(StatType.MoveSpeed);
    }
}