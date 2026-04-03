using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using UnityEngine.InputSystem.UI;
using static UnityEngine.Rendering.DebugUI;


public class PlayerController : MonoBehaviour
{
    [Header("Physics Variables")]
    [SerializeField] private Rigidbody2D playerRb;
    public Vector2 playerVelocity;
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
    public bool isPhasing;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashUses;
    [SerializeField] private float dashTime;
    [SerializeField] private BoxCollider2D dashCollider;
    public bool isDead;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject knifeProjectile;

    private Vector3 pauseStoredVelocity;
    private float pauseStoredAngularVelocity;

    [Header("Mask Variables")]
    [SerializeField] private SpriteRenderer maskSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> maskVariants;
    public int maskType;
    public Mask currentMask;
    [SerializeField] private List<Sprite> spriteVariants;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator maskAnim;
    private Coroutine maskCoroutine, maskTimeRoutine;

    [Header("Events")]
    [SerializeField] private UnityEvent checkDashes;

    [Header("Input Variables")]
    private InputAction move;
    private InputAction jump;
    private InputAction sprint;
    private InputAction ability;
    private InputAction pause;

    [Header("Mask Powerup Variables")]
    public bool hasFalconSuperJump;
    [SerializeField] private bool hasFoxMask;
    public bool hasPhaseMask;
    public bool hasSkullMask;
    public bool hasThrowingKnife;
    [SerializeField] private float falconJumpMultiplier;
    [SerializeField] private float falconGravityMultiplier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        maskCoroutine = null;
        maskTimeRoutine = null;
    }

    private void OnEnable()
    {
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        sprint = InputSystem.actions.FindAction("Sprint");
        ability = InputSystem.actions.FindAction("Ability");
        pause = InputSystem.actions.FindAction("Pause");

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

        pause.Enable();
        pause.performed += Pause;

        PauseManager.OnGamePaused += OnPause;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }

    void Move(InputAction.CallbackContext context)
    {
        if (PauseManager.isLevelPaused && LevelManager.Instance.GetCurrentLevelCompleted() == true) return;
       
    
        if (PauseManager.isLevelPaused && LevelManager.Instance.GetCurrentLevelCompleted() != true)
        {
            PauseManager.ResumeLevel();
        }       
        
       
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
            playerRb.AddForce(Vector2.up * jumpForce * falconJumpMultiplier, ForceMode2D.Impulse);
        }
    }

    void CancelJump(InputAction.CallbackContext context)
    {
        if (!isDashing)
        {
            playerRb.gravityScale = fallingGravity;

            if (playerRb.linearVelocityY > 0 && !hasSkullMask)
            {
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
        }
        else
        {
            moveSpeed = 10;
        }
    }

    void Ability(InputAction.CallbackContext context)
    {
        if (hasPhaseMask && !isDashing && dashUses > 0)
        {
            dashUses = dashUses - 1;
            StartCoroutine(SetDashingTimer());
            playerRb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
            UIManager.Instance.SetMaskUses(dashUses);
        }
        else if (hasThrowingKnife && dashUses > 0)
        {
            dashUses = dashUses - 1;
            Rigidbody2D projectileRb = Instantiate(knifeProjectile, firePoint.position, firePoint.rotation).GetComponent<Rigidbody2D>();
            projectileRb.AddForce(dashDirection.normalized * 15f, ForceMode2D.Impulse);
            UIManager.Instance.SetMaskUses(dashUses);
        }
        DashCheck();
    }

    public void DashCheck()
    {
        if (dashUses == 0)
        {
            currentMask = null;
        }
    }

    public void PhaseWallPush(float forceMultipler)
    {
        playerRb.linearVelocity = playerVelocity.normalized * (dashForce * forceMultipler);
    }

    private void Pause(InputAction.CallbackContext context)
    {
        if (PauseManager.isGamePaused)
        {
            // EndPause();
            PauseManager.ResumeGame();
        }
        else
        {
            // StartPause();
            PauseManager.PauseGame();
        }
    }

    IEnumerator SetDashingTimer()
    {
        isDashing = true;
        playerRb.linearVelocity = Vector2.zero;
        playerRb.gravityScale = 0;
        dashCollider.size = SetColliderSize(1, 2.5f);
        dashCollider.offset = SetColliderOffset(0.6f, 0);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            StartCoroutine(Die());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.isGamePaused) return;

        isGrounded = GroundCheck();

        if (isGrounded && !isDashing && !hasFalconSuperJump)
        {
            playerRb.gravityScale = normalGravity;
        }
        else if (isGrounded && !isDashing && hasFalconSuperJump)
        {
            playerRb.gravityScale = normalGravity * falconGravityMultiplier;
        }

        if (playerRb.linearVelocityY <= 0 && !isGrounded && !isDashing && !hasFalconSuperJump)
        {
            playerRb.gravityScale = fallingGravity;
        }
        else if (playerRb.linearVelocityY <= 0 && !isGrounded && !isDashing && hasFalconSuperJump)
        {
            playerRb.gravityScale = fallingGravity * falconGravityMultiplier;
        }

        if (moveDirection.x > 0 && Time.timeScale != 0)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1);
        }
        else if (moveDirection.x < 0 && Time.timeScale != 0)
        {
            transform.localScale = new Vector3(-1.2f, 1.2f, 1);
        }

        SetMaskSprite(currentMask);
        if (moveDirection.x != 0 && Time.timeScale != 0 && !isSprinting)
        {
            anim.Play("Unmasked_Walk");
            maskAnim.Play("Mask_Walk");
        }
        else if (moveDirection.x != 0 && Time.timeScale != 0 && isSprinting)
        {
            anim.Play("Unmasked_Run");
            maskAnim.Play("Mask_Run");
        }
        else
        {
            anim.Play("Unmasked_Idle");
            maskAnim.Play("Mask_Idle");
        }
    }

    private void FixedUpdate()
    {
        if (PauseManager.isGamePaused) return;
        playerVelocity = playerRb.linearVelocity;

        smoothedDirection = Vector2.SmoothDamp(
            smoothedDirection,
            moveDirection,
            ref moveDirectionSmoothedVelocity,
            smoothDampTime);

        if (!isDashing && !isDead)
        {
            playerRb.linearVelocity = new Vector2(smoothedDirection.x * moveSpeed, playerRb.linearVelocityY);
        }
        else if (isDead)
        {
            playerRb.linearVelocity = Vector2.zero;
        }

        if (!hasFoxMask && isSprinting)
        {
            moveSpeed = 9;
        }
        else if (hasFoxMask && isSprinting)
        {
            moveSpeed = 13;
        }
    }

    public void SetMaskSprite(Mask mask)
    {
        if (mask != null)
        {
            maskSprite.sprite = mask.maskSprite;
        }
        else
        {
            maskSprite.sprite = UIManager.Instance.masks[3].maskSprite;
        }
    }

    public void OnPause(bool isGamePaused)
    {
        if (isGamePaused)
        {
            pauseStoredVelocity = playerRb.linearVelocity;
            pauseStoredAngularVelocity = playerRb.angularVelocity;
            playerRb.bodyType = RigidbodyType2D.Kinematic;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
            anim.speed = 0f;
        }
        else
        {
            playerRb.linearVelocity = pauseStoredVelocity;
            playerRb.angularVelocity = pauseStoredAngularVelocity;
            playerRb.bodyType = RigidbodyType2D.Dynamic;
            anim.speed = 1;
        }
    }

    public IEnumerator Die()
    {
        isDead = true;
        LevelManager.Instance.ReloadScene();
        yield return new WaitForSeconds(0.5f);
        //transform.position = LevelManager.Instance.levelOrigin;
        //UIManager.Instance.StopFadeTransition();
        isDead = false;
    }

    public void MaskAbility(Mask mask)
    {
        if (mask.maskType == 0 || mask.maskType == 1)
        {
            CoroutineCheck(maskCoroutine);
            CoroutineCheck(maskTimeRoutine);
            SetMaskVariable(currentMask, false);
            currentMask = mask;
            maskTimeRoutine = StartCoroutine(MaskTimer(mask, mask.breakTime));
        }
        else
        {
            CoroutineCheck(maskCoroutine);
            CoroutineCheck(maskTimeRoutine);
            SetMaskVariable(currentMask, false);
            currentMask = mask;
            SetMaskVariable(currentMask, true);
            dashUses = mask.breakTime;
            UIManager.Instance.SetMaskUses(dashUses);
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
        maskCoroutine = StartCoroutine(UIManager.Instance.SetMaskTime(mask));
        SetMaskVariable(mask, true);
        yield return new WaitForSeconds(time);
        SetMaskVariable(mask, false);
        currentMask = null;
        UIManager.Instance.ZeroMaskTime();
        UIManager.Instance.SetMaskIcon(UIManager.Instance.masks[3]);
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
        else if (mask == UIManager.Instance.masks[5])
        {
            hasThrowingKnife = value;
        }
        else if (mask == null)
        {

        }
    }
}
