using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Paneles del menu de pausa")]
    public GameObject menuPausa;     // Panel con Reanudar / Opciones / Salir
    public GameObject menuOpciones;  // Panel con Musica / Volver

    [Header("Botones (opcional, para auto-conectar)")]
    public Button botonOpciones;
    public Button botonVolver;

    private bool pausado = false;

    void Awake()
    {
        // CRÍTICO: restaurar el tiempo ANTES de que corra cualquier Update
        Time.timeScale = 1f;
        pausado = false;
    }

    void Start()
    {
        // Estado inicial: juego corriendo, paneles ocultos
        if (menuPausa != null) menuPausa.SetActive(false);
        if (menuOpciones != null) menuOpciones.SetActive(false);

        if (botonOpciones != null) botonOpciones.onClick.AddListener(AbrirOpciones);
        if (botonVolver != null) botonVolver.onClick.AddListener(CerrarOpciones);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOpciones != null && menuOpciones.activeSelf)
            {
                CerrarOpciones();
                return;
            }

            if (pausado) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        pausado = true;
        Time.timeScale = 0f;
        if (menuPausa != null) menuPausa.SetActive(true);
        if (menuOpciones != null) menuOpciones.SetActive(false);
    }

    public void Reanudar()
    {
        pausado = false;
        Time.timeScale = 1f;
        if (menuPausa != null) menuPausa.SetActive(false);
        if (menuOpciones != null) menuOpciones.SetActive(false);
    }

    public void AbrirOpciones()
    {
        if (menuPausa != null) menuPausa.SetActive(false);
        if (menuOpciones != null) menuOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        if (menuOpciones != null) menuOpciones.SetActive(false);
        if (menuPausa != null) menuPausa.SetActive(true);
    }

    // Boton "Salir": vuelve al menu principal
    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        pausado = false;
        SceneManager.LoadScene("MainMenu");
    }

    // Red de seguridad: si el objeto se desactiva, no dejar el juego congelado
    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
