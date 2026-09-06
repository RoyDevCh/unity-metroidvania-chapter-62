using UnityEngine;

public class Player : Entity
{
    [Header("Movement")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.16f;
    [SerializeField] private float dashCooldown = 0.65f;
    [SerializeField] private LayerMask groundLayer = 1 << 8;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    [Header("Combat")]
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius = 1.15f;
    [SerializeField] private float attackDamage = 25f;

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    public bool IsGrounded { get { return groundCheck != null && Physics2D.OverlapCircle(groundCheck.position, 0.12f, groundLayer); } }
    public bool IsTouchingWall { get { return wallCheck != null && Physics2D.OverlapCircle(wallCheck.position, 0.12f, groundLayer); } }
    public float JumpForce { get { return jumpForce; } }
    public float DashSpeed { get { return dashSpeed; } }
    public float DashDuration { get { return dashDuration; } }
    public float DashCooldown { get { return dashCooldown; } }
    public Transform AttackCheck { get { return attackCheck; } }
    public float AttackRadius { get { return attackRadius; } }
    public float AttackDamage { get { return attackDamage; } }
    public bool CounterWindow { get; private set; }
    public string StateLabel { get { return StateMachine == null || StateMachine.CurrentState == null ? "None" : StateMachine.CurrentState.GetType().Name; } }
    private float dashCooldownTimer;
    private float comboTimer;
    private int comboStage;

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        AirState = new PlayerAirState(this, StateMachine);
        DashState = new PlayerDashState(this, StateMachine);
        WallSlideState = new PlayerWallSlideState(this, StateMachine);
        WallJumpState = new PlayerWallJumpState(this, StateMachine);
        AttackState = new PlayerAttackState(this, StateMachine);
        CounterAttackState = new PlayerCounterAttackState(this, StateMachine);
    }

    private void Start() { StateMachine.Initialize(IdleState); }

    protected override void Update()
    {
        base.Update();
        dashCooldownTimer = Mathf.Max(0f, dashCooldownTimer - Time.deltaTime);
        comboTimer -= Time.deltaTime;
        if (comboTimer <= 0f) comboStage = 0;
        if (!IsDead) StateMachine.Update();
    }

    public void Jump()
    {
        if (rb != null) rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void BeginCounterWindow() { CounterWindow = true; }
    public void EndCounterWindow() { CounterWindow = false; }

    public void PerformAttack(float damageMultiplier)
    {
        if (attackCheck == null) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            Enemy enemy = hits[i].GetComponentInParent<Enemy>();
            if (enemy != null && enemy.IsAlive())
                enemy.Damage(attackDamage * damageMultiplier, new Vector2(facingDirection * 7f, 3f));
        }
    }

    public bool TryCounter()
    {
        if (attackCheck == null) return false;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius + 0.25f);
        for (int i = 0; i < hits.Length; i++)
        {
            Enemy enemy = hits[i].GetComponentInParent<Enemy>();
            if (enemy != null && enemy.CanBeStunned())
            {
                enemy.Stun(1.25f);
                return true;
            }
        }
        return false;
    }

    public Vector2 InputVector() { return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); }

    public bool TryStartDash()
    {
        if (dashCooldownTimer > 0f) return false;
        dashCooldownTimer = dashCooldown;
        StateMachine.ChangeState(DashState);
        return true;
    }

    public void BeginAttack()
    {
        comboStage = comboTimer > 0f ? (comboStage + 1) % 3 : 0;
        comboTimer = 0.65f;
        AttackState.SetComboStage(comboStage);
        StateMachine.ChangeState(AttackState);
    }

    public void ConfigureRuntimeReferences(Transform ground, Transform wall, Transform attack)
    {
        groundCheck = ground;
        wallCheck = wall;
        attackCheck = attack;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackCheck != null) { Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(attackCheck.position, attackRadius); }
        if (groundCheck != null) { Gizmos.color = Color.green; Gizmos.DrawWireSphere(groundCheck.position, 0.12f); }
    }
}
