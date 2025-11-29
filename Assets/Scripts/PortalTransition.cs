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
            if (CoinManager.Instance != null && CoinManager.Instance.GetCoinCount() >= CoinManager.Instance.GetMaxCoins())
            {
                if (LevelManager.Instance != null)
                {
                    LevelManager.Instance.CompleteLevel(nextSceneName);
                }
                else
                {
                    SceneManager.LoadScene(nextSceneName);
                }
            }
            else
            {
                int current = CoinManager.Instance ? CoinManager.Instance.GetCoinCount() : 0;
                int required = CoinManager.Instance ? CoinManager.Instance.GetMaxCoins() : 0;
                Debug.Log($"Necesitas recolectar todos los engranes para pasar. Tienes {current}/{required}");
            }
        }
    }
}