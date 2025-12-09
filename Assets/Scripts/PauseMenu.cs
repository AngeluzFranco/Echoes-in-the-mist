using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseUI;

    [Header("Input System")]
    [Tooltip("InputSystem_Actions → Player → Pause")]
    [SerializeField] private InputActionReference pauseAction;

    [Header("Options")]
    [SerializeField] private bool manageCursor = true;
    [SerializeField] private bool manageAudio = true;

    [Header("Post Processing")]
    [SerializeField] private Volume blurVolume;

    private bool isPaused;
    private CanvasGroup canvas;

    private void Awake()
    {
        Time.timeScale = 1f;

        if (pauseUI)
        {
            canvas = pauseUI.GetComponent<CanvasGroup>();
            if (!canvas) canvas = pauseUI.AddComponent<CanvasGroup>();
            HideUI();
        }

        if (blurVolume)
            blurVolume.weight = 0f;
    }

    private void OnEnable()
    {
        if (pauseAction == null)
        {
            Debug.LogError("PauseMenu → pauseAction NO asignado");
            return;
        }

        pauseAction.action.Enable();
        pauseAction.action.performed += OnPause;
    }

    private void OnDisable()
    {
        if (pauseAction == null) return;

        pauseAction.action.performed -= OnPause;
        pauseAction.action.Disable();
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
            ShowUI();
        else
            HideUI();

        Time.timeScale = isPaused ? 0f : 1f;

        if (manageCursor)
        {
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPaused;
        }

        if (manageAudio)
            AudioListener.pause = isPaused;

        if (blurVolume)
            blurVolume.weight = isPaused ? 1f : 0f;
    }

    private void ShowUI()
    {
        pauseUI.SetActive(true);
        canvas.alpha = 1f;
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

    private void HideUI()
    {
        pauseUI.SetActive(false);
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }

    public void HideImmediately()
{
    isPaused = false;
    Time.timeScale = 1f;

    if (pauseUI)
    {
        pauseUI.SetActive(false);

        if (canvas)
        {
            canvas.alpha = 0f;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }
    }

    AudioListener.pause = false;

    if (blurVolume)
        blurVolume.weight = 0f;

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}

}
