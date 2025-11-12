using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    private int coins;
    private int maxCoins = 100;
    public GameObject coinPrefab;


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
    void Start()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                var instance = Instantiate(coinPrefab);
                instance.transform.position = new Vector3(x, 1.5f, y);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
