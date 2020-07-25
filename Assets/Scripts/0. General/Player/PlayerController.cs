using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private AudioManager AudioManager;
    [SerializeField]
    Rigidbody2D playerRB;
    SpriteRenderer playerSR;
    Collider2D mainCollider;
    private CharacterController controller;
    private PlayerConfiguration playerConfig;
    private PlayerControls controls;
    private LevelManager levelManager;
    public int playerIndex;
    [SerializeField]
    public ParticleSystem dustParticle;

    [Header("Movement variables")]
    public float moveSpeed;
    Vector2 moveInput = Vector2.zero;
    [SerializeField]
    private float moveDirection = 1;

    [Header("Jump variables")]
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private float jumpHeight;
    private float currentJumpHeight;
    private int jumpCount = 0;

    [Header("Dash variables")]
    [SerializeField]
    private bool dashEnabled;
    public float dashSpeed;
    [SerializeField]
    private float specialCooldown;
    [SerializeField]
    private float currentSpecialCooldown;
    private bool specialUsed;

    [Header("Push variables")]
    [SerializeField]
    private bool pushEnabled;
    [SerializeField]
    private GameObject pushCollider;
    public float pushForce;

    [Header("Super Jump variables")]
    [SerializeField]
    private bool superJumpEnabled;
    public float superJumpHeight;

    [Header("Float Jump variables")]
    [SerializeField]
    private bool floatJumpEnabled;
    [SerializeField]
    private float maxFloatTime;
    private float currentFloatTime;
    private bool isFloating;

    [Header("Power up variables")]
    [SerializeField]
    private float highJumpPowerUpHeight;
    public float magnetPowerUpMoveSpeed;
    public bool iceIsActive;


    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        controller = GetComponent<CharacterController>();
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        controls = new PlayerControls();
        currentFloatTime = maxFloatTime;
        SetJumpHeight();
        AudioManager = AudioManager.Instance;
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        playerConfig = pc;
        playerIndex = playerConfig.PlayerIndex;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
        //start only after countdown
        if (!levelManager.isStarted)
            return;


        if (PlayerPrefs.GetString("PlayersState") == "Playing")
        {
            //ice power up
            if (iceIsActive)
                return;

            if (obj.action.name == controls.Player.Movement.name)
                OnMove(obj);
            if (obj.action.name == controls.Player.Jump.name && obj.performed && (IsGrounded() || jumpCount == 1))
                Jump();
            if (obj.action.name == controls.Player.Special.name && obj.performed && currentSpecialCooldown <= 0)
            {
                if (dashEnabled)
                    Dash();
                if (pushEnabled)
                    Push();
                if (superJumpEnabled)
                    SuperJump();
                if (floatJumpEnabled)
                    FloatJump();
            }
        }
    }

    void Update()
    {
        //start only after countdown
        if (!levelManager.isStarted)
            return;
        Move();

        if (currentSpecialCooldown > 0)
            currentSpecialCooldown -= Time.deltaTime;

        if (isFloating)
            currentFloatTime -= Time.deltaTime;

        if (isFloating && currentFloatTime > 0)
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0);

        if (currentFloatTime <= 0)
        {
            isFloating = false;
            ResetFloatCooldown();
            ResetSpecialCooldown();
        }

    }

    void Move()
    {
        //ice power up
        if (iceIsActive) return;
        transform.position += new Vector3(moveInput.x * Time.deltaTime * moveSpeed, 0f, 0f);
        flipSprite(playerSR, moveDirection);
    }

    public void OnMove(CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();
        if (moveInput.x > 0)
            moveDirection = 1;
        else if (moveInput.x < 0)
            moveDirection = -1;

    }

    public void flipSprite(SpriteRenderer SR, float direction)
    {
        if (direction < 0) SR.flipX = true;
        if (direction > 0) SR.flipX = false;
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(mainCollider.bounds.center,
        mainCollider.bounds.size, 0f, Vector2.down, 1f, groundLayerMask);
        if (hit.collider != null)
            jumpCount = 0;
        return hit.collider != null;
    }


    public void Jump()
    {
        if (isFloating)
            isFloating = false;

        AudioManager.PlaySound("PlayerSkillJump");
        playerRB.velocity = (Vector2.up * currentJumpHeight);
        jumpCount++;


    }

    public void Dash()
    {
        AudioManager.PlaySound("PlayerSkillDash");
        playerRB.AddForce(Vector2.right * moveDirection * dashSpeed, ForceMode2D.Impulse);
        ResetSpecialCooldown();
    }

    public void Push()
    {
        AudioManager.PlaySound("PlayerSkillDash");
        pushCollider.SetActive(true);
        ResetSpecialCooldown();
    }

    public void DisablePush()
    {
        pushCollider.SetActive(false);
    }

    public void SuperJump()
    {
        AudioManager.PlaySound("PlayerSkillSuperJump");
        playerRB.velocity = (Vector2.up * superJumpHeight);
        ResetSpecialCooldown();
    }


    public void FloatJump()
    {
        AudioManager.PlaySound("PlayerFloatJump");
        isFloating = true;
        ResetSpecialCooldown();
    }

    public void ResetFloatCooldown()
    {
        currentFloatTime = maxFloatTime;
    }

    public void ResetSpecialCooldown()
    {
        currentSpecialCooldown = specialCooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PushCollider") && other.gameObject != transform.GetChild(0).gameObject)
            Ricochet(other.transform);
    }


    public void Ricochet(Transform otherObjectTransform)
    {
        var dir = (this.transform.position - otherObjectTransform.position).normalized;
        playerRB.AddForce(dir * pushForce, ForceMode2D.Impulse);

    }


    public void SetJumpHeight()
    {
        if (!this.GetComponent<PlayerPowerUpManager>().highJumpActive)
            currentJumpHeight = jumpHeight;
        else
            currentJumpHeight = highJumpPowerUpHeight;

    }

}
