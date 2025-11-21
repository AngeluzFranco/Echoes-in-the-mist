using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
    [Header("Level Settings")]
    public float playerMaxHealth = 100f;
    
    [Header("UI Prefabs")]
    public GameObject pauseMenuPrefab;
    private GameObject pauseMenuInstance;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        EnsurePauseMenu();
    }
    
    private void Start()
    {
        InitializeLevel();
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeLevel();
    }
    
    public void InitializeLevel()
    {
        // Esperar un frame para que todos los objetos se inicialicen
        Invoke(nameof(SetupLevel), 0.1f);
    }
    
    private void SetupLevel()
    {
        EnsurePauseMenu();
        
        // Eliminar duplicados si los hay
        var menus = FindObjectsByType<PauseMenu>(FindObjectsSortMode.None);
        foreach (var m in menus)
        {
            if (pauseMenuInstance && m.gameObject != pauseMenuInstance)
            {
                Destroy(m.gameObject);
            }
        }
        int gearsInLevel = 0;
        
        // Método 1: Intentar encontrar por tag "Coin" (si existe)
        try
        {
            GameObject[] gears = GameObject.FindGameObjectsWithTag("Coin");
            gearsInLevel = gears.Length;
            Debug.Log($"Encontrados {gearsInLevel} engranes con tag 'Coin'");
        }
        catch (UnityException)
        {
            Debug.LogWarning("Tag 'Coin' no está definido. Usando método alternativo.");
        }
        
        // Método 2: Si no hay tag o no se encontraron, usar las posiciones del CoinManager
        if (gearsInLevel == 0 && CoinManager.Instance != null && CoinManager.Instance.coinPositions != null)
        {
            gearsInLevel = CoinManager.Instance.coinPositions.Length;
            Debug.Log($"Usando posiciones del CoinManager: {gearsInLevel} engranes");
        }
        
        // Método 3: Como último recurso, buscar objetos con componente Coin
        if (gearsInLevel == 0)
        {
            Coin[] coinComponents = FindObjectsByType<Coin>(FindObjectsSortMode.None);
            gearsInLevel = coinComponents.Length;
            Debug.Log($"Encontrados {gearsInLevel} objetos con componente Coin");
        }
        
        // Si aún no se encontraron engranes, usar valor por defecto
        if (gearsInLevel == 0)
        {
            gearsInLevel = 5; // Valor por defecto
            Debug.LogWarning("No se pudieron detectar engranes automáticamente. Usando valor por defecto: 5");
        }
        
        // Configurar el CoinManager con el límite correcto
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.SetMaxCoins(gearsInLevel);
            CoinManager.Instance.ResetCoins();
        }
        
        // Reiniciar la vida del jugador
        var playerHealth = FindFirstObjectByType<Health>();
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }
        
        Debug.Log($"Nivel configurado: {gearsInLevel} engranes disponibles");
    }

    private void EnsurePauseMenu()
    {
        if (pauseMenuInstance == null)
        {
            var existing = FindFirstObjectByType<PauseMenu>(FindObjectsInactive.Include);
            if (existing)
            {
                pauseMenuInstance = existing.gameObject;
            }
            else if (pauseMenuPrefab != null)
            {
                pauseMenuInstance = Instantiate(pauseMenuPrefab);
            }
            if (pauseMenuInstance)
            {
                var pm = pauseMenuInstance.GetComponent<PauseMenu>();
                if (pm) pm.HideImmediately();
                DontDestroyOnLoad(pauseMenuInstance);
            }
        }
        else
        {
            var pm = pauseMenuInstance.GetComponent<PauseMenu>();
            if (pm) pm.HideImmediately();
        }
    }
    
    public void CompleteLevel(string nextSceneName)
    {
        Debug.Log("¡Nivel completado! Transición al siguiente nivel.");
        
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("No se especificó el nombre de la siguiente escena");
        }
    }
}