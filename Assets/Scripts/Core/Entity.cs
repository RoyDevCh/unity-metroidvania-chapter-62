using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float moveSpeed = 7f;
    [SerializeField] protected float knockbackDuration = 0.18f;

    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Color baseColor = Color.white;
    protected float health;
    protected float knockbackTimer;
    protected float flashTimer;
    protected float damageFeedbackTimer;
    protected int facingDirection = 1;
    protected bool dead;

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public bool IsDead { get { return dead; } }
    public int FacingDirection { get { return facingDirection; } }
    public bool WasDamagedRecently { get { return damageFeedbackTimer > 0f; } }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null) baseColor = spriteRenderer.color;
        health = maxHealth;
    }

    protected virtual void Update()
    {
        if (knockbackTimer > 0f)
            knockbackTimer -= Time.deltaTime;
        if (damageFeedbackTimer > 0f)
            damageFeedbackTimer -= Time.deltaTime;

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            if (spriteRenderer != null)
                spriteRenderer.color = Color.Lerp(baseColor, Color.red, flashTimer / 0.12f);
        }
        else if (spriteRenderer != null && !dead)
        {
            spriteRenderer.color = baseColor;
        }
    }

    public bool IsKnocked { get { return knockbackTimer > 0f; } }

    public virtual void Damage(float amount, Vector2 knockback)
    {
        if (dead) return;
        health = Mathf.Max(0f, health - amount);
        flashTimer = 0.12f;
        damageFeedbackTimer = 0.85f;
        if (rb != null)
            rb.velocity = new Vector2(knockback.x, knockback.y);
        knockbackTimer = knockbackDuration;
        if (health <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        dead = true;
        if (rb != null) rb.velocity = Vector2.zero;
        if (spriteRenderer != null) spriteRenderer.color = new Color(0.25f, 0.25f, 0.25f);
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
        Destroy(gameObject, 0.65f);
    }

    public void SetVelocity(float x, float y)
    {
        if (rb != null && !IsKnocked && !dead)
            rb.velocity = new Vector2(x, y);
    }

    public void SetZeroVelocity()
    {
        if (rb != null && !IsKnocked) rb.velocity = Vector2.zero;
    }

    public void Face(int direction)
    {
        if (direction == 0) return;
        facingDirection = direction < 0 ? -1 : 1;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        transform.localScale = scale;
    }

    public bool IsAlive() { return !dead && health > 0f; }
}
