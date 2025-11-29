using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    private int coins;
    private int maxCoins = 5;
    public GameObject coinPrefab;
    public Vector3[] coinPositions;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance) Destroy(Instance.gameObject);
        Instance = this;

    }

    public void AddCoins(int amount)
    {
        coins += Mathf.Min(maxCoins, coins + amount);
        Debug.Log($"Total coins: {coins}");

       
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
    }

    public void ResetCoins()
    {
        coins = 0;
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
