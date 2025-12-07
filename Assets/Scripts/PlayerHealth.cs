using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida del jugador")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Spawn del jugador")]
    public Transform spawnPoint;

    [Header("UI de vida")]
    public HealthUI healthUI;   // ← referencia al script UI

    [Header("Fade en muerte")]
    public FadeScreen fadeScreen;

    void Start()
    {
        currentHealth = maxHealth;

        // Buscar spawn automáticamente si no está asignado
        if (spawnPoint == null)
        {
            GameObject spawnObj = GameObject.Find("SpawnPoint");
            spawnPoint = spawnObj != null ? spawnObj.transform : transform;
        }

        // UI inicial
        if (healthUI != null)
            healthUI.UpdateHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        // Evitar llamadas cuando ya estamos en 0
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("🔥 Daño recibido. Vida actual: " + currentHealth);

        // Actualizar UI
        if (healthUI != null)
            healthUI.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth > 0)
        {
            RespawnToSpawn();  // NO cura, como pediste
        }
        else
        {
            StartCoroutine(DieAndReturnMenu());
        }
    }

    void RespawnToSpawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        // Si usas CharacterController (MUY IMPORTANTE)
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Si usas Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // <-- corrección: usar velocity
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }

        // Teletransporte seguro
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        yield return new WaitForSeconds(0.05f); // Espera un frame de física

        // Volver a activar
        if (cc != null) cc.enabled = true;

        Debug.Log("🔁 Respawn asegurado en spawnPoint sin fallar.");
    }

    IEnumerator DieAndReturnMenu()
    {
        Debug.Log("💀 Sin vida → iniciando fade y cargando MainMenu...");

        // Si hay FadeScreen, usamos su coroutine pública
        if (fadeScreen != null)
        {
            // asumimos que FadeAndLoadScene es IEnumerator y carga la escena
            yield return StartCoroutine(fadeScreen.FadeAndLoadScene("MainMenu"));
        }
        else
        {
            // Si no hay fade, esperar un pequeño delay y cargar la escena
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
