using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public float damageAmount = 10f;
    public float damageInterval = 1f; // Daño cada 1 segundo
    private float nextDamageTime = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageInterval; // Reiniciar timer de daño
            }
        }
    }
}
