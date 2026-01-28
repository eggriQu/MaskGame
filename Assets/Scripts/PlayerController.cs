using NUnit.Framework;
using System.Collections.Generic;
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

    [Header("Other Variables")]
    [SerializeField] private SpriteRenderer maskSprite;
    [SerializeField] private List<Sprite> maskVariants;
    [SerializeField] private SwipeController maskUI;
    public int maskType;

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
        playerRb.gravityScale = gravity;
        if (playerRb.linearVelocityY > 0)
        {
            playerRb.AddForce(new Vector2(0, -playerRb.linearVelocityY), ForceMode2D.Impulse);
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
            playerRb.gravityScale = 1.2f;
        }

        if (playerRb.linearVelocityY <= 0 && !isGrounded)
        {
            playerRb.gravityScale = gravity;
        }

        if (maskUI.currentPage == 1)
        {
            maskSprite.sprite = maskVariants[0];
            maskType = 1;
        }
        else if (maskUI.currentPage == 2)
        {
            maskSprite.sprite = maskVariants[1];
            maskType = 2;
        }
        else if (maskUI.currentPage == 3)
        {
            maskSprite.sprite = maskVariants[2];
            maskType = 3;
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
