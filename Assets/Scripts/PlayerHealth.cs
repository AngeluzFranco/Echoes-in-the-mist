using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        else
            Debug.LogWarning($"{name}: 'healthBar' no asignado en PlayerHealth. Asigna el componente HealthBar en el Inspector para ver la UI.");
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Jugador muerto");
        }
    }
}
