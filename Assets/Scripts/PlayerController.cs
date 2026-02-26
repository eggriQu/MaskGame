using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour, IInteractable, IMasked
{
    [Header("Physics Variables")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Vector2 moveDirection = Vector2.zero;
    Vector2 smoothedDirection;
    Vector2 moveDirectionSmoothedVelocity;
    [SerializeField] private float smoothDampTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallingGravity;
    [SerializeField] private float normalGravity;
    public bool isGrounded;
    [SerializeField] private bool isSprinting;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;
    public LayerMask groundLayer;
    [SerializeField] private Vector2 dashDirection;
    [SerializeField] public bool isDashing;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashUses;
    [SerializeField] private float dashTime;
    [SerializeField] private BoxCollider2D dashCollider;

    [Header("Mask Variables")]
    [SerializeField] private SpriteRenderer maskSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> maskVariants;
    [SerializeField] private SwipeController maskUI;
    public int maskType;
    public Mask currentMask;
    [SerializeField] private List<Sprite> spriteVariants;
    [SerializeField] private Animator anim;
    private Coroutine jumpCoroutine, jumpTimeRoutine;
    private Coroutine sprintCoroutine, sprintTimeRoutine;

    [Header("Input Variables")]
    private InputAction move;
    private InputAction jump;
    private InputAction sprint;
    private InputAction ability;

    [Header("Mask Powerup Variables")]
    [SerializeField] public bool hasFalconSuperJump;
    [SerializeField] private bool hasFoxMask;
    [SerializeField] public bool hasPhaseMask;
    [SerializeField] public bool hasSkullMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        maskUI = GameObject.Find("Select UI").GetComponent<SwipeController>();
        jumpCoroutine = null;
        jumpTimeRoutine = null;
        sprintCoroutine = null;
        sprintTimeRoutine = null;
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
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Move(InputAction.CallbackContext context)
    {
        if (!hasSkullMask)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else
        {
            moveDirection = context.ReadValue<Vector2>() * 0.75f;
        }
        dashDirection = moveDirection;
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
            playerRb.AddForce(Vector2.up * jumpForce * 1.75f, ForceMode2D.Impulse);
        }
    }

    void CancelJump(InputAction.CallbackContext context)
    {
        if (!isDashing)
        {
            playerRb.gravityScale = fallingGravity;
            if (playerRb.linearVelocityY > 0 && !hasSkullMask)
            {
                //playerRb.AddForce(new Vector2(0, -playerRb.linearVelocityY), ForceMode2D.Impulse);
                playerRb.linearVelocityY = 0;
            }
            else if ((playerRb.linearVelocityY > 0 || playerRb.linearVelocityY <= 0) && hasSkullMask)
            {
                playerRb.gravityScale = 0;
                dashCollider.size = SetColliderSize(1.4f, 0.4f);
                dashCollider.offset = SetColliderOffset(0, -1.2f);
                playerRb.AddForce(Vector2.down * 35, ForceMode2D.Impulse);
            }
        }
    }

    void Sprint(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }

    void StopSprinting(InputAction.CallbackContext context)
    {
        isSprinting = false;
        if (!hasFoxMask)
        {
            moveSpeed = 7;
            smoothDampTime = 0.1f;
        }
        else
        {
            moveSpeed = 10;
            smoothDampTime = 0.1f;
        }
    }

    void Ability(InputAction.CallbackContext context)
    {
        if (hasPhaseMask && !isDashing && dashUses > 0)
        {
            dashUses = dashUses - 1;
            StartCoroutine(SetDashingTimer());
            playerRb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
            UIManager.Instance.SetDashMaskUses(dashUses);
        }
    }

    IEnumerator SetDashingTimer()
    {
        isDashing = true;
        playerRb.linearVelocity = Vector2.zero;
        playerRb.gravityScale = 0;
        dashCollider.size = SetColliderSize(1, 2.5f);
        if (dashDirection.x > 0)
        {
            dashCollider.offset = SetColliderOffset(0.6f, 0);
        }
        else if (dashDirection.x < 0)
        {
            dashCollider.offset = SetColliderOffset(-0.6f, 0);
        }
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        dashCollider.size = SetColliderSize(0.2f, 0.2f);
        dashCollider.offset = SetColliderOffset(0, 0);
        playerRb.gravityScale = fallingGravity;
    }

    private Vector2 SetColliderSize(float x, float y)
    {
        return new Vector2(x, y);
    }

    private Vector2 SetColliderOffset(float x, float y)
    {
        return new Vector2(x, y);
    }

    public bool GroundCheck()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            if (!isDashing)
            {
                dashCollider.size = SetColliderSize(0.2f, 0.2f);
                dashCollider.offset = SetColliderOffset(0, 0);
            }
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
            playerRb.gravityScale = normalGravity;
        }

        if (playerRb.linearVelocityY <= 0 && !isGrounded && !isDashing)
        {
            playerRb.gravityScale = fallingGravity;
        }

//        if (maskUI.currentPage == 1)
//        {
//           maskSprite.sprite = maskVariants[0];
//            //spriteRenderer.sprite = spriteVariants[0];
//            maskType = 0;
//        }
//        else if (maskUI.currentPage == 2)
//        {
//            maskSprite.sprite = maskVariants[1];
//            //spriteRenderer.sprite = spriteVariants[1];
//            maskType = 1;
//        }
//        else if (maskUI.currentPage == 3)
//       {
//           maskSprite.sprite = maskVariants[2];
//            //spriteRenderer.sprite = spriteVariants[2];
//            maskType = 2;
//        }
//        currentMask = UIManager.Instance.masks[maskType];

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
            anim.Play(ChosenAnimation(1, false));
        }
        else
        {
            anim.Play(ChosenAnimation(1, true));
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
            playerRb.linearVelocity = new Vector2(smoothedDirection.x * moveSpeed, playerRb.linearVelocityY);
        }
        else
        {
            //playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, playerRb.linearVelocity.y);
        }

        if (!hasFoxMask && isSprinting)
        {
            moveSpeed = 9;
            smoothDampTime = 0.1f;
        }
        else if (hasFoxMask && isSprinting)
        {
            moveSpeed = 13;
            smoothDampTime = 0.1f;
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
            case (0): // Jump Masks
                if (mask == UIManager.Instance.masks[0]) // Falcon
                {
                    CoroutineCheck(jumpCoroutine);
                    CoroutineCheck(jumpTimeRoutine);
                    jumpTimeRoutine = StartCoroutine(MaskTimer(mask, mask.breakTime));
                }
                else if (mask == UIManager.Instance.masks[4]) // Skull
                {
                    CoroutineCheck(jumpCoroutine);
                    CoroutineCheck(jumpTimeRoutine);
                    jumpTimeRoutine = StartCoroutine(MaskTimer(mask, mask.breakTime));
                }
                break;
            case (1): // Sprint Masks
                if (mask == UIManager.Instance.masks[1]) // Fox
                {
                    CoroutineCheck(sprintCoroutine);
                    CoroutineCheck(sprintTimeRoutine);
                    sprintTimeRoutine = StartCoroutine(MaskTimer(mask, mask.breakTime));
                }
                break;
            case (2): // Dash Masks
                if (mask == UIManager.Instance.masks[2]) // Phase
                {
                    hasPhaseMask = true;
                    dashUses = mask.breakTime;
                    UIManager.Instance.SetDashMaskUses(dashUses);
                }
                break;
        }
    }

    private void CoroutineCheck(Coroutine routine)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        else
        {

        }
    }

    public IEnumerator MaskTimer(Mask mask, float time)
    {
        if (mask.maskType == 0 ||  mask.maskType == 1)
        {
            if (mask.maskType == 0)
            {
                jumpCoroutine = StartCoroutine(UIManager.Instance.SetMaskTime(mask));
            }
            else
            {
                sprintCoroutine = StartCoroutine(UIManager.Instance.SetMaskTime(mask));
            }
            SetMaskVariable(mask, true);
            yield return new WaitForSeconds(time);
            SetMaskVariable(mask, false);
            UIManager.Instance.ZeroMaskTime(mask);
            if (mask.maskType == 0)
            {
                UIManager.Instance.SetJumpMaskPage(UIManager.Instance.masks[3]);
            }
            else if (mask.maskType == 1)
            {
                UIManager.Instance.SetSprintMaskPage(UIManager.Instance.masks[3]);
            }
        }
    }

    public void SetMaskVariable(Mask mask, bool value)
    {
        if (mask == UIManager.Instance.masks[0])
        {
            hasFalconSuperJump = value;
        }
        else if (mask == UIManager.Instance.masks[1])
        {
            hasFoxMask = value;
        }
        else if (mask == UIManager.Instance.masks[2])
        {
            hasPhaseMask = value;
        }
        else if (mask == UIManager.Instance.masks[4])
        {
            hasSkullMask = value;
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
