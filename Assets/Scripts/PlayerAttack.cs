using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;
    PlayerController2D controller;

    public GameObject hitSide;
    public GameObject hitUp;
    public GameObject hitDown;
    public DamageDealer sideDamage;
    public DamageDealer upDamage;
    public DamageDealer downDamage;

    bool isAttacking;

    public AudioClip attackSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // ปุ่มฟัน
        {
            Attack();
        }
    }

    void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;

        Vector2 dir = controller.lastDir;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            anim.SetTrigger("AttackSide");
            
            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
        else if (dir.y > 0)
        {
            anim.SetTrigger("AttackUp");

            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
        else
        {
            anim.SetTrigger("AttackDown");

            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
    }
    public bool IsAttacking()
    {
       return isAttacking;
    }


    public void BackToMove()
    {
        isAttacking = false;
        DisableAllHitbox();
       anim.Play("Move");
    }
    public void EndAttack()
    {
        isAttacking = false;
    }




    // ===== Animation Events =====

    public void EnableSideHitbox()
    {
        DisableAllHitbox();
        hitSide.SetActive(true);
        
    }

    public void EnableUpHitbox()
    {
        DisableAllHitbox();
        hitUp.SetActive(true);
        
    }

    public void EnableDownHitbox()
    {
        DisableAllHitbox();
        hitDown.SetActive(true);
        
    }

    public void DisableAllHitbox()
    {
        hitSide.SetActive(false);
        hitUp.SetActive(false);
        hitDown.SetActive(false);

         
    }

    void OnDrawGizmos()
    {
        BoxCollider2D col = GetComponentInChildren<BoxCollider2D>();
        if (col == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}
