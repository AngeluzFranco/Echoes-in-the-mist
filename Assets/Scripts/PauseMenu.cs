using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseUI;
    private bool isPaused;
    [SerializeField] private bool manageCursor = false;
    [SerializeField] private bool manageAudio = true;
    public Volume blurVolume;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        if (pauseUI == gameObject)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (!_canvasGroup) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
        else if (pauseUI)
        {
            pauseUI.SetActive(false);
        }
        if (blurVolume) blurVolume.weight = 0f;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ResolveBlurVolume();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResolveBlurVolume();
    }

    private void ResolveBlurVolume()
    {
        if (blurVolume && blurVolume.gameObject) return;
        var vol = FindFirstObjectByType<Volume>(FindObjectsInactive.Include);
        if (vol) blurVolume = vol;
        if (blurVolume) blurVolume.weight = 0f;
    }

    void Update()
    {
        var esc = Input.GetKeyDown(KeyCode.Escape);
#if ENABLE_INPUT_SYSTEM
        if (!esc)
        {
            try
            {
                esc = UnityEngine.InputSystem.Keyboard.current != null &&
                      UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame;
            }
            catch { }
        }
#endif
        if (esc) TogglePause();

        if (isPaused && manageCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (_canvasGroup)
        {
            if (pauseUI) pauseUI.SetActive(true);
            _canvasGroup.alpha = isPaused ? 1f : 0f;
            _canvasGroup.interactable = isPaused;
            _canvasGroup.blocksRaycasts = isPaused;
        }
        else if (pauseUI)
        {
            pauseUI.SetActive(isPaused);
        }
        Time.timeScale = isPaused ? 0f : 1f;
        if (manageCursor)
        {
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPaused;
        }
        if (manageAudio) AudioListener.pause = isPaused;
        if (blurVolume) blurVolume.weight = isPaused ? 1f : 0f;
    }

    public void Resume()
    {
        if (isPaused) TogglePause();
    }

    public void HideImmediately()
    {
        isPaused = false;
        if (_canvasGroup)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
        else if (pauseUI)
        {
            pauseUI.SetActive(false);
        }
        Time.timeScale = 1f;
        if (manageAudio) AudioListener.pause = false;
    }

    public void ExitToMainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        if (!string.IsNullOrEmpty(sceneName)) SceneManager.LoadScene(sceneName);
    }

    public void ExitToDesktop()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}
