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

    Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogError("Ciel: มาสเตอร์คะ! ชิเอลหา Main Camera ใน Scene ไม่เจอค่ะ!");
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (isAttacking || cam == null) return;

        isAttacking = true;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 dirToMouse = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(dirToMouse.y, dirToMouse.x) * Mathf.Rad2Deg;

        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        if (angle > 60 && angle < 120)
        {
            anim.SetTrigger("AttackUp");
        }
        else if (angle < -60 && angle > -120)
        {
            anim.SetTrigger("AttackDown");
        }
        else
        {
            anim.SetTrigger("AttackSide");
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

    void FlipTowardsMouse()
    {
        if (isAttacking || cam == null) return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
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
