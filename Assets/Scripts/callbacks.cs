using UnityEngine;

public class callbacks : MonoBehaviour
{
    // Start se llama una vez antes de la primera ejecución de Update después de que se crea el MonoBehaviour
    private void Start()
    {
       
    }

    // Update se llama una vez por frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        

        transform.Translate(horizontal, 0, vertical);



        /*
        // Rota el objeto si se mantiene presionada la tecla de flecha derecha
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(0.05f, 0f, 0f);
        }

        //   transform.position += new Vector3(x: 0.05f, y: 0f, z: 0f);

        */
    }
}
