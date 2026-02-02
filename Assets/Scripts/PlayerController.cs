using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    private InputAction ability;

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
        ability = InputSystem.actions.FindAction("Ability");

        move.Enable();
        move.performed += Move;
        move.canceled += StopMoving;

        jump.Enable();
        jump.performed += Jump;
        jump.canceled += CancelJump;

        sprint.Enable();
        sprint.performed += Sprint;
        sprint.canceled += StopSprinting;

        ability.Enable();
        ability.performed += Ability;
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

    void Ability(InputAction.CallbackContext context)
    {
        if (maskType == 0)
        {
            Debug.Log("WAA");
        }
        else if (maskType == 1)
        {
            Debug.Log("YAY");
        }
        else
        {
            Debug.Log("GRR");
        }
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

        if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (moveDirection.x != 0)
        {
            anim.Play(ChosenAnimation(maskType, false));
        }
        else
        {
            anim.Play(ChosenAnimation(maskType, true));
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

    string ChosenAnimation(int maskType, bool isIdle)
    {
        string animationName = "";
        if (!isIdle)
        {
            switch (maskType)
            {
                case 0:
                    animationName = "Walk_Sad";
                    break;
                case 1:
                    animationName = "Walk_Happy";
                    break;
                case 2:
                    animationName = "Walk_Mad";
                    break;
            }
        }
        else
        {
            switch (maskType)
            {
                case 0:
                    animationName = "Idle_Sad";
                    break;
                case 1:
                    animationName = "Idle_Happy";
                    break;
                case 2:
                    animationName = "Idle_Angry";
                    break;
            }
        }
        return animationName;
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
