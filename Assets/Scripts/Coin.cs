using UnityEngine;

public class Coin : MonoBehaviour
{



    void Start()
    {

    }

    void Update()
    {

        transform.Rotate(0f, 360 * Time.deltaTime, 0f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Debug.Log("Coin collected!");
            this.gameObject.SetActive(false);
            CoinManager.Instance.AddCoins(1);
            
        }
        

    }
}
