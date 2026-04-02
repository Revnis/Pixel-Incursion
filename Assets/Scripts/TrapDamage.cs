using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [Header("Trap Settings")]
    public int damageAmount = 10;
    public string trapName = "Spike_Trap";
    public float damageDelay = 1f;

    private float lastDamageTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageDelay)
            {
                var player = collision.GetComponent<PlayerStats>();

                if (player != null)
                {
                    player.TakeDamage(damageAmount, trapName);

                    lastDamageTime = Time.time;
                    Debug.Log($"Hit {trapName}");
                }
            }
        }
    }
}