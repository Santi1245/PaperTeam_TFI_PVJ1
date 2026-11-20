using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public enum GameState
{
    Start,
    Playing,
    Paused,
    Victory,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Score => gameData != null ? gameData.currentScore : 0;
    public bool EstaPausado => currentState == GameState.Paused;
    public bool EstaJugando => currentState == GameState.Playing;

    [Header("Persistencia")]
    [SerializeField] private GameData gameData;
    [SerializeField] private PlayerStats playerStats;

    [Header("State Settings")]
    [SerializeField] private GameState currentState;

    [Header("Victory Settings")]
    [SerializeField] private EnemyAI[] enemies;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int enemiesRemaining;

    [Header("Puntaje")]
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    [Header("Monedas")]
    [SerializeField] private TextMeshProUGUI textoMonedas;

    [Header("UI")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private string victorySceneName = "Victory";
    [SerializeField] private float victoryDelay = 2f;
    [SerializeField] private float gameOverDelay = 2f;

    [Header("Orden de niveles")]
    [SerializeField] private string[] nivelesEnOrden = { "Tutorial", "Nivel1", "Nivel2" };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (gameData == null)
            Debug.LogError("[GameManager] Falta asignar el asset GameData en el Inspector.");
        else
            Debug.Log("[GameManager] Escena " + SceneManager.GetActiveScene().name +
                      " | asset: " + gameData.name +
                      " | monedas: " + gameData.currentCoins +
                      " | vida: " + gameData.currentHealth);

        if (playerStats == null)
            Debug.LogError("[GameManager] Falta asignar el asset PlayerStats en el Inspector.");

        if (enemies == null || enemies.Length == 0)
            enemies = FindObjectsByType<EnemyAI>(FindObjectsInactive.Exclude);

        totalEnemies = enemies.Length;
        enemiesRemaining = totalEnemies;

        ActualizarMonedasUI();
        ActualizarPuntajeUI();

        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.Start:
                Time.timeScale = 1f;
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                if (victoryPanel != null) victoryPanel.SetActive(false);
                if (gameOverPanel != null) gameOverPanel.SetActive(false);
                Debug.Log("<color=green>ESTADO: jugando</color>");
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;

            case GameState.Victory:
                Time.timeScale = 0f;
                if (victoryPanel != null) victoryPanel.SetActive(true);
                Debug.Log("<color=cyan>ESTADO: Victoria</color>");
                StartCoroutine(IrAlSiguienteNivel(victoryDelay));
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                if (gameOverPanel != null) gameOverPanel.SetActive(true);
                Debug.Log("<color=red>ESTADO: Game Over</color>");
                StartCoroutine(ReiniciarNivel(gameOverDelay));
                break;
        }
    }

    // ---------- FLUJO DE ESCENAS ----------

    private IEnumerator ReiniciarNivel(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        string actual = SceneManager.GetActiveScene().name;
        Debug.Log("Reiniciando nivel: " + actual);
        Time.timeScale = 1f;
        SceneManager.LoadScene(actual);
    }

    private IEnumerator IrAlSiguienteNivel(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        string actual = SceneManager.GetActiveScene().name;
        int indice = System.Array.IndexOf(nivelesEnOrden, actual);

        if (indice >= 0 && indice + 1 < nivelesEnOrden.Length)
        {
            string siguiente = nivelesEnOrden[indice + 1];
            Debug.Log($"Nivel completado: {actual} -> cargando {siguiente}");
            Time.timeScale = 1f;
            SceneManager.LoadScene(siguiente);
        }
        else
        {
            Debug.Log($"Ultimo nivel completado ({actual}) -> cargando {victorySceneName}");
            Time.timeScale = 1f;
            SceneManager.LoadScene(victorySceneName);
        }
    }

    public void Paused()
    {
        if (currentState == GameState.Playing)
            ChangeState(GameState.Paused);
        else if (currentState == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    // ---------- PUNTAJE ----------

    public void AddScore(int amount)
    {
        if (gameData == null) return;

        gameData.currentScore += amount;
        ActualizarPuntajeUI();
        Debug.Log($"<color=orange>Puntaje: {gameData.currentScore}</color>");
    }

    void ActualizarPuntajeUI()
    {
        if (scoreText != null && gameData != null)
            scoreText.text = gameData.currentScore.ToString();
    }

    // ---------- MONEDAS ----------

    public void SumarMoneda(int cantidad)
    {
        if (gameData == null) return;

        gameData.currentCoins += cantidad;
        ActualizarMonedasUI();
        Debug.Log($"<color=yellow>Monedas: {gameData.currentCoins}</color>");
    }

    public int GetMonedas()
    {
        return gameData != null ? gameData.currentCoins : 0;
    }

    public bool GastarMonedas(int cantidad)
    {
        if (gameData == null) return false;
        if (gameData.currentCoins < cantidad) return false;

        gameData.currentCoins -= cantidad;
        ActualizarMonedasUI();
        return true;
    }

    void ActualizarMonedasUI()
    {
        if (textoMonedas != null && gameData != null)
            textoMonedas.text = gameData.currentCoins.ToString();
    }

    // ---------- ESTADOS ----------

    public void NotifyPlayerDead()
    {
        if (currentState != GameState.Playing) return;

        if (gameData != null && playerStats != null)
        {
            gameData.ResetProgress(playerStats.maxHealth);
            Debug.Log("[GameManager] Progreso reseteado. Vida: " + gameData.currentHealth +
                      " | Monedas: " + gameData.currentCoins);
        }
        else
        {
            Debug.LogError("[GameManager] No se pudo resetear el progreso: falta GameData o PlayerStats.");
        }

        ChangeState(GameState.GameOver);
    }

    public void NotifyEnemyDefeat()
    {
        if (currentState != GameState.Playing) return;

        enemiesRemaining--;
        Debug.Log($"Enemigos restantes: {enemiesRemaining}");
    }

    public void NotifyGoalZone()
    {
        if (currentState != GameState.Playing) return;
        ChangeState(GameState.Victory);
    }
}
