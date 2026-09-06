using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private float detectionRange = 7f;
    [SerializeField] private float attackRange = 1.45f;
    [SerializeField] private float attackDamage = 12f;
    [SerializeField] private float patrolDistance = 3f;
    private Player player;
    private Vector3 startPosition;
    private float stunTimer;
    private float attackTimer;
    private bool attacking;

    public string StateLabel { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateLabel = "Patrol";
        startPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        if (dead) return;
        if (player == null) player = FindObjectOfType<Player>();
        if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
            StateLabel = "Stunned";
            SetZeroVelocity();
            return;
        }
        if (attackTimer > 0f) attackTimer -= Time.deltaTime;
        float distance = player == null ? 999f : Vector2.Distance(transform.position, player.transform.position);
        if (distance <= attackRange && player != null)
        {
            StateLabel = attacking ? "Attack" : "Battle";
            SetZeroVelocity();
            Face(player.transform.position.x >= transform.position.x ? 1 : -1);
            if (attackTimer <= 0f) { attacking = true; attackTimer = 1.1f; Invoke(nameof(DealAttack), 0.45f); }
        }
        else if (distance <= detectionRange && player != null)
        {
            StateLabel = "Chase";
            int dir = player.transform.position.x >= transform.position.x ? 1 : -1;
            Face(dir);
            SetVelocity(dir * moveSpeed * 0.55f, rb == null ? 0f : rb.velocity.y);
        }
        else
        {
            StateLabel = "Patrol";
            float offset = transform.position.x - startPosition.x;
            int dir = offset >= patrolDistance ? -1 : (offset <= -patrolDistance ? 1 : facingDirection);
            Face(dir);
            SetVelocity(dir * moveSpeed * 0.28f, rb == null ? 0f : rb.velocity.y);
        }
    }

    private void DealAttack()
    {
        attacking = false;
        if (dead || player == null || Vector2.Distance(transform.position, player.transform.position) > attackRange + 0.35f) return;
        if (player.CounterWindow) return;
        player.Damage(attackDamage, new Vector2(facingDirection * 5f, 3f));
    }

    public bool CanBeStunned()
    {
        if (dead || stunTimer > 0f || player == null) return false;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        bool attackIsActive = attacking;
        bool attackIsAboutToStart = !attacking && attackTimer <= 0.05f && distance <= attackRange + 1f;
        bool counterWindowTarget = player.CounterWindow && distance <= attackRange + 1f;
        return attackIsActive || attackIsAboutToStart || counterWindowTarget;
    }
    public void Stun(float duration) { stunTimer = Mathf.Max(stunTimer, duration); attacking = false; CancelInvoke(nameof(DealAttack)); SetZeroVelocity(); }
}
