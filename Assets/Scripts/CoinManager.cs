using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    private int coins;
    private int maxCoins = 1;
    public GameObject coinPrefab;
    public Vector3[] coinPositions;

    public int GetCoinCount()
    {
        return coins;
    }
    
    public int GetMaxCoins()
    {
        return maxCoins;
    }
    
    public void SetMaxCoins(int max)
    {
        maxCoins = max;
        Debug.Log($"Límite de coins ajustado a: {maxCoins}");
        UpdateUI();
    }
    
    public void ResetCoins()
    {
        coins = 0;
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        var engraneUI = FindFirstObjectByType<EngraneUI>();
        if (engraneUI != null)
        {
            engraneUI.UpdateCoins(coins, maxCoins);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance) Destroy(Instance.gameObject);
        Instance = this;

    }

    public void AddCoins(int amount)
    {
        coins = Mathf.Min(maxCoins, coins + amount);
        Debug.Log($"Total coins: {coins}/{maxCoins}");
        
        UpdateUI();
        
        // Verificar si se completaron todos los engranes
        if (coins >= maxCoins)
        {
            Debug.Log("¡Todos los engranes recolectados! Busca el portal para el siguiente nivel.");
        }
    }
    void Start()
    {
        if (coinPositions != null && coinPositions.Length > 0)
        {
            foreach (var pos in coinPositions)
            {
                var instance = Instantiate(coinPrefab);
                instance.transform.position = pos;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
