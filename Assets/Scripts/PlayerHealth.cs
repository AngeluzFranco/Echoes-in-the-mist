using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public HealthUI healthUI;

    [Header("Respawn")]
    public Transform spawnPoint;
    public float invulnerabilitySeconds = 2f;

    // Internal
    private bool isRespawning = false;
    private bool isInvulnerable = false;
    private CharacterController characterController;
    private Rigidbody rb;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        if (spawnPoint != null)
            MoveToSpawnImmediate();

        UpdateUI();
    }

    public void TakeDamage(float amount)
    {
        if (isRespawning || isInvulnerable) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateUI();

        if (currentHealth <= 0f && !isRespawning)
        {
            StartCoroutine(HandleRespawn());
        }
    }

    private IEnumerator HandleRespawn()
    {
        isRespawning = true;

        // Opcional: reproducir animación de muerte, sonido, desactivar controles, etc.
        // Desactivar control de movimiento (tu sistema de control debe respetar esto)
        var movement = GetComponent<MonoBehaviour>(); // reemplaza si tienes un script de control
        // if (movement != null) movement.enabled = false; // Comenta/descomenta según tu proyecto

        // Espera un frame para que otros OnTrigger/OnCollision terminen
        yield return null;
        yield return new WaitForSeconds(0.05f);

        // Teletransportar de forma segura según el componente usado
        MoveToSpawnImmediate();

        // Restaurar salud
        currentHealth = maxHealth;
        UpdateUI();

        // Limpiar velocidades si hay Rigidbody
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Breve invulnerabilidad para no reaparecer y morir instantáneamente
        isInvulnerable = true;
        float timer = 0f;
        while (timer < invulnerabilitySeconds)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        isInvulnerable = false;

        // Reactivar controles si fue desactivado
        // if (movement != null) movement.enabled = true;

        isRespawning = false;
    }

    private void MoveToSpawnImmediate()
    {
        if (spawnPoint == null) return;

        // Si tiene CharacterController, desactívalo antes de mover
        if (characterController != null)
        {
            characterController.enabled = false;
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            characterController.enabled = true;
            // también limpiar el "vertical velocity" u otras variables en tu controlador de movimiento si aplica
            return;
        }

        // Si tiene Rigidbody, usar MovePosition si está en modo cinemático, o set position directo
        if (rb != null)
        {
            rb.position = spawnPoint.position;
            rb.rotation = spawnPoint.rotation;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            return;
        }

        // Caso general
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    private void UpdateUI()
    {
        if (healthUI != null)
            healthUI.UpdateHealth(currentHealth, maxHealth);
    }

    // Método público para forzar respawn desde otro script (ej: GameManager)
    public void ForceRespawnNow()
    {
        if (!isRespawning) StartCoroutine(HandleRespawn());
    }
}
