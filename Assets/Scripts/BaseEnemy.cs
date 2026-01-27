using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 30f;
    public float currentHP;

    [Header("Reward")]
    [SerializeField] int minExp;
    [SerializeField] int maxExp;

    [Header("Loot Drop")]
    [Range(0, 100)] public float dropChance = 30f;

    [Header("Attack")]
    public float attackDamage = 2f;
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

    public AudioClip takeDamageSound;
    private AudioSource audioSource;
    
    [Header("Appearance")]
    public Color defaultColor = Color.white;

    protected bool isKnockedBack = false;

    Vector3 originalScale;

    protected bool playerInRange = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        originalScale = transform.localScale;
        
    }

    protected virtual void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (sr != null)
        {
            sr.color = defaultColor;
        }
    }

    protected virtual void Update()
    {
        if (player == null) return;

        PlayerStats targetStats = player.GetComponent<PlayerStats>();
        if (targetStats != null && targetStats.isDead)
        {
            player = null;
            anim.SetBool("isMoving", false);
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);
        playerInRange = dist <= detectRange;

        anim.SetBool("isMoving", playerInRange && dist > attackRange);
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        PlayerStats targetStats = player.GetComponent<PlayerStats>();

        if (targetStats != null)
        {
            targetStats.TakeDamage((int)attackDamage);
        }

        lastAttackTime = Time.time;

        anim.SetTrigger("attack");
        anim.SetBool("isMoving", false); // สำคัญมาก
    }

    protected virtual void FixedUpdate()
    {
        if (isKnockedBack) return;

        if (!playerInRange || player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

         float dist = Vector2.Distance(transform.position, player.position);

         if (dist <= attackRange)
        {
             rb.linearVelocity = Vector2.zero;
             TryAttack();
             return;
        }

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;

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

        //anim.SetTrigger("hit");

        if (takeDamageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(takeDamageSound);
        }
        if (currentHP <= 0)
        {
            Die();
        }
        if (damage > 0)
        {
            StartCoroutine(FlashRedEffect());
        }
    }
    IEnumerator FlashRedEffect()
    {
        sr.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        sr.color = defaultColor;
    }

    public void TakeKnockback(Transform damageSource, float knockbackForce)
    {
        isKnockedBack = true;

        Vector2 direction = (transform.position - damageSource.position).normalized;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        StartCoroutine(ResetKnockbackRoutine());
    }

    System.Collections.IEnumerator ResetKnockbackRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    protected virtual void Die()
    {
        //anim.SetTrigger("die");
        rb.linearVelocity = Vector2.zero;

        if (player != null)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                int randomExp = UnityEngine.Random.Range(minExp, maxExp + 1);

                playerStats.GainExp(randomExp);

                Debug.Log($"EXP : {randomExp}");
            }

            float randomVal = UnityEngine.Random.Range(0f, 100f);
            if (randomVal <= dropChance)
            {
                PlayerInventory inventory = player.GetComponent<PlayerInventory>();

                if (inventory != null)
                {
                    inventory.GetHealingPotion();

                    Debug.Log("Potion +1");
                }
            }
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 0.5f);
    }

    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
