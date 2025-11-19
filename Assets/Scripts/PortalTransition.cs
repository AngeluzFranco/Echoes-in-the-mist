using UnityEngine;
using UnityEngine.SceneManagement; // ← Agrega esto arriba

public class PortalTransition : MonoBehaviour
{
    private int requiredCoins;
    public string nextSceneName;

    private void Start()
    {
        if (CoinManager.Instance != null && CoinManager.Instance.coinPositions != null)
        {
            requiredCoins = CoinManager.Instance.coinPositions.Length;
        }
        else
        {
            requiredCoins = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CoinManager.Instance != null && CoinManager.Instance.GetCoinCount() >= requiredCoins)
            {
                Debug.Log("¡Nivel completado! Transición al siguiente nivel.");
                SceneManager.LoadScene("Navigation");
            }
            else
            {
                Debug.Log("No tienes suficientes piezas para pasar el nivel.");
            }
        }
    }
}