using UnityEngine;

public class Practica : MonoBehaviour

  
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, 1) * (Time.deltaTime * speed);
      
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position += new Vector3(0, 1, 0);
        }

        var vertical = Input.GetAxis("Vertical");

      
        transform.position += new Vector3(0, 0, vertical) * (Time.deltaTime * speed);

        

    }
}
