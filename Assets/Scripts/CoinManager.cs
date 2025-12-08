using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private EngraneUI engraneUI;
    public static CoinManager Instance { get; private set; }
    private int coins;
    private int maxCoins = 5;
    public GameObject coinPrefab;
    public Vector3[] coinPositions;

   
    private void Awake()
    {
        if (Instance) Destroy(Instance.gameObject);
        Instance = this;

        if (engraneUI == null)
        {
            engraneUI = FindObjectOfType<EngraneUI>();
        }
    }

    public void AddCoins(int amount)
    {
        coins = Mathf.Min(maxCoins, coins + amount);
        Debug.Log($"Total coins: {coins}");
        if (engraneUI != null)
            engraneUI.UpdateCoins(coins, maxCoins);
    }

    public int GetCoinCount()
    {
        return coins;
    }

    public int GetMaxCoins()
    {
        return maxCoins;
    }

    public void SetMaxCoins(int value)
    {
        maxCoins = Mathf.Max(0, value);
        if (coins > maxCoins) coins = maxCoins;
        if (engraneUI != null)
            engraneUI.UpdateCoins(coins, maxCoins);
    }

    public void ResetCoins()
    {
        coins = 0;
        if (engraneUI != null)
            engraneUI.UpdateCoins(coins, maxCoins);
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

        if (engraneUI != null)
            engraneUI.UpdateCoins(coins, maxCoins);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}