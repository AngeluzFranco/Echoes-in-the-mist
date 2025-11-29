using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Referencia al relleno de la vida")]
    public Image vidaRelleno;  // Debe ser un Image con Image Type = Filled

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float fillAmount = currentHealth / maxHealth;
        vidaRelleno.fillAmount = fillAmount;
    }
}
