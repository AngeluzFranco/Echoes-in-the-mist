using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;


    
    void Start()
    {
      
       
    }
    
    void Update()
    {
        if (target != null)
        {
           
            transform.position = Vector3.Lerp(transform.position, target.position, 5f * Time.deltaTime);
        }
    }
}
