using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
    [Header("Physics Variables")]
    [SerializeField] private Rigidbody2D playerRb;
    Vector2 moveDirection = Vector2.zero;
    Vector2 smoothedDirection;
    Vector2 moveDirectionSmoothedVelocity;
    [SerializeField] private float smoothDampTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    public bool isGrounded;
    [SerializeField] private bool isSprinting;

    [Header("Input Variables")]
    private InputAction move;
    private InputAction jump;
    private InputAction sprint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

    }

    private void OnEnable()
    {
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        sprint = InputSystem.actions.FindAction("Sprint");

        move.Enable();
        move.performed += Move;
        move.canceled += StopMoving;

        jump.Enable();
        jump.performed += Jump;
        jump.canceled += CancelJump;

        sprint.Enable();
        sprint.performed += Sprint;
        sprint.canceled += StopSprinting;
    }

    void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    void StopMoving(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.zero;
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void CancelJump(InputAction.CallbackContext context)
    {
        if (!isGrounded)
        {
            playerRb.gravityScale = gravity;
        }
    }

    void Sprint(InputAction.CallbackContext context)
    {
        isSprinting = true;
        moveSpeed = moveSpeed * 2;
        smoothDampTime = smoothDampTime * 3f;
    }

    void StopSprinting(InputAction.CallbackContext context)
    {
        isSprinting = false;
        moveSpeed = moveSpeed / 2;
        smoothDampTime = smoothDampTime / 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            playerRb.gravityScale = 1;
        }

        if (playerRb.linearVelocityY <= 0 && !isGrounded)
        {
            playerRb.gravityScale = gravity;
        }
    }

    private void FixedUpdate()
    {
        smoothedDirection = Vector2.SmoothDamp(
            smoothedDirection,
            moveDirection,
            ref moveDirectionSmoothedVelocity,
            smoothDampTime);

        playerRb.linearVelocity = new Vector2(smoothedDirection.x * moveSpeed, playerRb.linearVelocity.y);
    }
}
