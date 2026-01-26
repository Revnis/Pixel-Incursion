using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed;

    Vector2 input;
    public Vector2 lastDir = Vector2.down;

    Rigidbody2D rb;
    Animator anim;

    Vector3 originalScale;

    private void Start()
    {
        Debug.Log($"Move Speed : {moveSpeed}");
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // lastDir
        if (input.x != 0)
            lastDir = new Vector2(Mathf.Sign(input.x), 0);
        else if (input.y != 0)
            lastDir = new Vector2(0, Mathf.Sign(input.y));

        // flip (HitBox + Child)
        if (lastDir.x > 0.05f)
            transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        else if (lastDir.x < -0.05f)
            transform.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );

        // animator
        anim.SetFloat("moveX", input.x);
        anim.SetFloat("moveY", input.y);
        anim.SetFloat("speed", input.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * moveSpeed;
    }
}
