using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 30f;
    public float currentHP;

    [Header("Attack")]
    public float attackRange = 0.8f;
    public float attackCooldown = 1f;
    float lastAttackTime;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float detectRange = 3f;

    protected Transform player;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;

    Vector3 originalScale;

    protected bool playerInRange = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

         originalScale = transform.localScale;
        
    }

    protected virtual void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        playerInRange = dist <= detectRange;

        anim.SetBool("isMoving", playerInRange && dist > attackRange);
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        anim.SetTrigger("attack");
        anim.SetBool("isMoving", false); // สำคัญมาก
    }

    protected virtual void FixedUpdate()
    {
        if (!playerInRange || player == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

         float dist = Vector2.Distance(transform.position, player.position);

         if (dist <= attackRange)
        {
             rb.velocity = Vector2.zero;
             TryAttack();
             return;
        }

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * moveSpeed;

        if (dir.x > 0.05f)
        transform.localScale = new Vector3(
         Mathf.Abs(originalScale.x),
        originalScale.y,
        originalScale.z
        );

        else if (dir.x < -0.05f)
        transform.localScale = new Vector3(
        -Mathf.Abs(originalScale.x),
        originalScale.y,
        originalScale.z
        );

        anim.SetBool("isMoving", true);
    }

    // ================== DAMAGE ==================
    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;

        anim.SetTrigger("hit");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        anim.SetTrigger("die");
        rb.velocity = Vector2.zero;

        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 0.5f);
    }

    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
