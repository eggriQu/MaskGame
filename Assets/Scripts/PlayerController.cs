using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;
    public LayerMask groundLayer;
    [SerializeField] private Vector2 dashDirection;
    [SerializeField] private bool isDashing;

    [Header("Mask Variables")]
    [SerializeField] private SpriteRenderer maskSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> maskVariants;
    [SerializeField] private SwipeController maskUI;
    public int maskType;
    public Mask currentMask;
    [SerializeField] private List<Sprite> spriteVariants;
    [SerializeField] private Animator anim;

    [Header("Input Variables")]
    private InputAction move;
    private InputAction jump;
    private InputAction sprint;
    private InputAction ability;

    [Header("Mask Powerup Variables")] 
    private bool hasFalconSuperJump;
    private bool hasFoxMask;
    private bool hasPhaseMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

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
        if (moveDirection.x > 0 && moveDirection.y > 0)
        {
            dashDirection = new Vector2(1, 1);
        }
        else if (moveDirection.x > 0 && moveDirection.y < 0)
        {
            dashDirection = new Vector2(1, -1);
        }
        else if (moveDirection.x < 0 && moveDirection.y > 0)
        {
            dashDirection = new Vector2(-1, 1);
        }
        else if (moveDirection.x < 0 && moveDirection.y < 0)
        {
            dashDirection = new Vector2(-1, -1);
        }
        else
        {
            dashDirection = moveDirection;
        }
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
            UIManager.Instance.SetJumpMaskPage(UIManager.Instance.masks[3]);
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
        if (!hasFoxMask)
        {
            isSprinting = true;
            moveSpeed = 7f;
            smoothDampTime = smoothDampTime * 1.4f;
        }
        else
        {
            isSprinting = true;
            moveSpeed = 10f;
            smoothDampTime = smoothDampTime * 2f;
        }
    }

    void StopSprinting(InputAction.CallbackContext context)
    {
        if (!hasFoxMask)
        {
            isSprinting = false;
            moveSpeed = 5f;
            smoothDampTime = smoothDampTime / 1.4f;
        }
        else
        {
            isSprinting = false;
            moveSpeed = 5f;
            smoothDampTime = smoothDampTime / 2f;
            hasFoxMask = false;
            UIManager.Instance.SetSprintMaskPage(UIManager.Instance.masks[3]);
        }
    }

    void Ability(InputAction.CallbackContext context)
    {
        if (hasPhaseMask)
        {
            StartCoroutine(SetDashingTimer());
            playerRb.AddForce(dashDirection * 20, ForceMode2D.Impulse);
        }
    }

    IEnumerator SetDashingTimer()
    {
        isDashing = true;
        playerRb.linearVelocity = Vector2.zero;
        playerRb.gravityScale = 1.8f;
        yield return new WaitForSeconds(0.3f);
        isDashing = false;
    }

    public bool GroundCheck()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = GroundCheck();

        if (isGrounded)
        {
            playerRb.gravityScale = 1.8f;
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
        currentMask = UIManager.Instance.masks[maskType];

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

        if (!isDashing)
        {
            playerRb.linearVelocity = new Vector2(smoothedDirection.x * moveSpeed, playerRb.linearVelocity.y);
        }
        else
        {
            //playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, playerRb.linearVelocity.y);
        }
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

    public void MaskAbility(Mask mask)
    {
        switch (mask.maskType)
        {
            case (0):
                if (mask == UIManager.Instance.masks[0])
                {
                    hasFalconSuperJump = true;
                }
                break;
            case (1):
                if (mask == UIManager.Instance.masks[1])
                {
                    hasFoxMask = true;
                }
                break;
            case (2):
                if (mask == UIManager.Instance.masks[2])
                {
                    hasPhaseMask = true;
                }
                break;
        }
    }

    public void ChangeMask(int maskIndex)
    {
        //maskType = currentMask.maskType;
    }

    public void SetFalconSuperJump(bool value)
    {
        hasFalconSuperJump = value;
    }
}
