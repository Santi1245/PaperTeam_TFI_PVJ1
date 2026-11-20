using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TiendaManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject panelTienda;
    [SerializeField] private TextMeshProUGUI textoMonedas;
    [SerializeField] private TextMeshProUGUI textoMensaje;

    [Header("Mejora 1 - Curar vida completa")]
    [SerializeField] private Button botonCurar;
    [SerializeField] private int costoCurar = 30;

    [Header("Mejora 2 - Aumentar vida maxima")]
    [SerializeField] private Button botonVidaMaxima;
    [SerializeField] private int costoVidaMaxima = 50;
    [SerializeField] private float bonusVidaMaxima = 20f;

    [Header("Mejora 3 - Aumentar velocidad")]
    [SerializeField] private Button botonVelocidad;
    [SerializeField] private int costoVelocidad = 40;
    [SerializeField] private float bonusVelocidad = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoCompra;
    [SerializeField] private AudioClip sonidoError;

    private void Start()
    {
        if (botonCurar != null) botonCurar.onClick.AddListener(ComprarCuracion);
        if (botonVidaMaxima != null) botonVidaMaxima.onClick.AddListener(ComprarVidaMaxima);
        if (botonVelocidad != null) botonVelocidad.onClick.AddListener(ComprarVelocidad);

        if (panelTienda != null) panelTienda.SetActive(false);
        ActualizarUI();
    }

    public void AbrirTienda()
    {
        if (panelTienda != null) panelTienda.SetActive(true);
        if (textoMensaje != null) textoMensaje.text = "";
        ActualizarUI();
    }

    public void CerrarTienda()
    {
        if (panelTienda != null) panelTienda.SetActive(false);
    }

    public void ComprarCuracion()
    {
        if (!PuedeComprar(costoCurar)) return;

        if (gameData.currentHealth >= playerStats.maxHealth)
        {
            MostrarMensaje("Ya tenes la vida al maximo");
            return;
        }

        gameData.currentCoins -= costoCurar;
        gameData.currentHealth = playerStats.maxHealth;
        ComprarExitoso("Vida restaurada");
    }

    public void ComprarVidaMaxima()
    {
        if (!PuedeComprar(costoVidaMaxima)) return;

        gameData.currentCoins -= costoVidaMaxima;
        playerStats.maxHealth += bonusVidaMaxima;
        gameData.currentHealth += bonusVidaMaxima;
        ComprarExitoso("Vida maxima +" + bonusVidaMaxima);
    }

    public void ComprarVelocidad()
    {
        if (!PuedeComprar(costoVelocidad)) return;

        gameData.currentCoins -= costoVelocidad;
        playerStats.walkSpeed += bonusVelocidad;
        playerStats.runSpeed += bonusVelocidad;
        ComprarExitoso("Velocidad +" + bonusVelocidad);
    }

    private bool PuedeComprar(int costo)
    {
        if (gameData.currentCoins < costo)
        {
            MostrarMensaje("No tenes monedas suficientes");
            if (audioSource != null && sonidoError != null) audioSource.PlayOneShot(sonidoError);
            return false;
        }
        return true;
    }

    private void ComprarExitoso(string mensaje)
    {
        MostrarMensaje(mensaje);
        if (audioSource != null && sonidoCompra != null) audioSource.PlayOneShot(sonidoCompra);
        ActualizarUI();
    }

    private void MostrarMensaje(string mensaje)
    {
        if (textoMensaje != null) textoMensaje.text = mensaje;
    }

    private void ActualizarUI()
    {
        if (textoMonedas != null && gameData != null)
            textoMonedas.text = "Monedas: " + gameData.currentCoins;
    }
}
