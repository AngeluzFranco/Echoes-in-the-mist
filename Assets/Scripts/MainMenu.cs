using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private string gameSceneName;

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
        
    }
}