using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damage = 10;

    Collider2D col;
    bool hasHit;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        hasHit = false;

        
        col.enabled = false;
        col.enabled = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            other.GetComponent<BaseEnemy>()?.TakeDamage(damage);
            hasHit = true;
        }
    }
}
