using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IInteractable, IMasked
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

    [Header("Mask Variables")]
    [SerializeField] private SpriteRenderer maskSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> maskVariants;
    [SerializeField] private SwipeController maskUI;
    public int maskType;
    public Mask currentMask;
    private GameManager gameManager;
    [SerializeField] private List<Sprite> spriteVariants;
    [SerializeField] private Animator anim;

    [Header("Input Variables")]
    private InputAction move;
    private InputAction jump;
    private InputAction sprint;

    [Header("Mask Powerup Variables")] 
    private bool hasFalconSuperJump;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
        if (maskType == 0)
        {
            anim.Play("Walk_Sad");
        }
        else if (maskType == 1)
        {
            anim.Play("Walk_Happy");
        }
        else if (maskType == 2)
        {
            anim.Play("Walk_Mad");
        }
    }

    void StopMoving(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.zero;
        if (maskType == 0)
        {
            anim.Play("Idle_Sad");
        }
        else if (maskType == 1)
        {
            anim.Play("Idle_Happy");
        }
        else if (maskType == 2)
        {
            anim.Play("Idle_Angry");
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && !hasFalconSuperJump)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (isGrounded && hasFalconSuperJump)
        {
            playerRb.AddForce(Vector2.up * jumpForce * 2, ForceMode2D.Impulse);
            SetFalconSuperJump(false);
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
            playerRb.gravityScale = 1.6f;
        }

        if (playerRb.linearVelocityY <= 0 && !isGrounded)
        {
            playerRb.gravityScale = gravity;
        }

        if (maskUI.currentPage == 1)
        {
            maskSprite.sprite = maskVariants[0];
            //spriteRenderer.sprite = spriteVariants[0];
            maskType = 0;
        }
        else if (maskUI.currentPage == 2)
        {
            maskSprite.sprite = maskVariants[1];
            //spriteRenderer.sprite = spriteVariants[1];
            maskType = 1;
        }
        else if (maskUI.currentPage == 3)
        {
            maskSprite.sprite = maskVariants[2];
            //spriteRenderer.sprite = spriteVariants[2];
            maskType = 2;
        }

        currentMask = gameManager.masks[maskType];
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

    public void TakeDamage(int damage)
    {
        
    }

    public void Interact()
    {

    }

    public void ChangeMask(int maskIndex)
    {
        maskType = currentMask.maskType;
    }

    public void SetFalconSuperJump(bool value)
    {
        hasFalconSuperJump = value;
    }
}
