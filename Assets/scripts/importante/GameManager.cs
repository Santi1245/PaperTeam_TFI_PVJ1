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

    public int Score => score;
    public bool EstaPausado => currentState == GameState.Paused;
    public bool EstaJugando => currentState == GameState.Playing;

    [Header("State Settings")]
    [SerializeField] private GameState currentState;

    [Header("Victory Settings")]
    [SerializeField] private EnemyAI[] enemies;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int enemiesRemaining;

    [Header("Puntaje")]
    [SerializeField] private int score;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    [Header("Monedas")]
    [SerializeField] private int monedas = 0;
    [SerializeField] private TextMeshProUGUI textoMonedas;

    [Header("UI")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Escenas")]
    [SerializeField] private string levelSelectSceneName = "SelectorDeNiveles";
    [SerializeField] private float victoryDelay = 2f;
    [SerializeField] private float gameOverDelay = 2f;

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
        if (enemies == null || enemies.Length == 0)
        {
            enemies = FindObjectsByType<EnemyAI>(FindObjectsInactive.Exclude);
        }

        totalEnemies = enemies.Length;
        enemiesRemaining = totalEnemies;

        CargarMonedas();
        ActualizarMonedasUI();

        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.Start:
                Time.timeScale = 1f;
                Debug.Log("Inicio el juego");
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                if (victoryPanel != null) victoryPanel.SetActive(false);
                if (gameOverPanel != null) gameOverPanel.SetActive(false);
                Debug.Log("<color=green>ESTADO: jugando</color>");
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                Debug.Log("<color=yellow>ESTADO: pausa</color>");
                break;

            case GameState.Victory:
                Time.timeScale = 0f;
                if (victoryPanel != null) victoryPanel.SetActive(true);
                Debug.Log("<color=cyan>ESTADO: Victoria</color>");
                StartCoroutine(VolverASeleccionDeNiveles(victoryDelay));
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                if (gameOverPanel != null) gameOverPanel.SetActive(true);
                Debug.Log("<color=red>ESTADO: Game Over</color>");
                StartCoroutine(VolverASeleccionDeNiveles(gameOverDelay));
                break;
        }
    }

    private IEnumerator VolverASeleccionDeNiveles(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Debug.Log("Cargando: " + levelSelectSceneName);
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelectSceneName);
    }

    public void Paused()
    {
        if (currentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null) scoreText.text = $"x{score}";
        Debug.Log($"<color=orange>Puntaje: {score}</color>");
    }

    // ---------- MONEDAS ----------

    public void SumarMoneda(int cantidad)
    {
        monedas += cantidad;
        ActualizarMonedasUI();
        GuardarMonedas();
        Debug.Log($"<color=yellow>Monedas: {monedas}</color>");
    }

    public int GetMonedas()
    {
        return monedas;
    }

    public bool GastarMonedas(int cantidad)
    {
        if (monedas < cantidad) return false;

        monedas -= cantidad;
        ActualizarMonedasUI();
        GuardarMonedas();
        return true;
    }

    void ActualizarMonedasUI()
    {
        if (textoMonedas != null)
            textoMonedas.text = "x" + monedas.ToString();
    }

    void GuardarMonedas()
    {
        PlayerPrefs.SetInt("Monedas", monedas);
        PlayerPrefs.Save();
    }

    void CargarMonedas()
    {
        monedas = PlayerPrefs.GetInt("Monedas", 0);
    }

    // ---------- ESTADOS ----------

    public void NotifyPlayerDead()
    {
        if (currentState != GameState.Playing) return;
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
