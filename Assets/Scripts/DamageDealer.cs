using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damage;
    public float knockbackForce;

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
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            
            if (enemy != null)
            {
                Debug.Log("Hit");

                enemy.TakeDamage(damage);

                enemy.TakeKnockback(transform, knockbackForce);
            }
            
            hasHit = true;
        }
    }
}
