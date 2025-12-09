using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("WaitToEnd", 165);
    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKey(KeyCode.Escape)){
        SceneManager.LoadScene("MainMenu");
     }   
    }

    public void WaitToEnd(){
        SceneManager.LoadScene("MainMenu");
    }
}
