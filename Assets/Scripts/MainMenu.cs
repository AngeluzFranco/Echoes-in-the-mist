using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private string gameSceneName;

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
        
    }

    public void Credits(){
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}