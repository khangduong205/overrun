using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference moveAction;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        // Lấy component 1 lần ở Awake, KHÔNG gọi GetComponent trong Update
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() => moveAction.action.Enable();
    private void OnDisable() => moveAction.action.Disable();

    private void Update()
    {
        // Đọc input mỗi frame
        // ClampMagnitude giữ chéo không nhanh hơn thẳng, nhưng vẫn giữ analog của gamepad
        moveInput = Vector2.ClampMagnitude(moveAction.action.ReadValue<Vector2>(), 1f);
    }

    private void FixedUpdate()
    {
        // Vật lý chạy trong FixedUpdate
        rb.linearVelocity = moveInput * moveSpeed;
    }
}