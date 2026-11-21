using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager Instance;

    private const string KEY_VOL_MASTER = "volumenMaster";
    private const string KEY_VOL_MUSICA = "volumenMusica";
    private const string KEY_VOL_SFX    = "volumenSFX";
    private const string KEY_VOL_UI     = "volumenUI";
    private const string KEY_DIFICULTAD = "dificultad";
    private const string KEY_PUNTAJE    = "puntajeMaximo";

    public float VolumenMaster { get; private set; }
    public float VolumenMusica { get; private set; }
    public float VolumenSFX    { get; private set; }
    public float VolumenUI     { get; private set; }

    // AudioSources registrados por categoria
    private readonly List<AudioSource> fuentesMusica = new List<AudioSource>();
    private readonly List<AudioSource> fuentesSFX    = new List<AudioSource>();
    private readonly List<AudioSource> fuentesUI     = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        CargarTodo();
    }

    // ---------- VOLUMEN ----------

    public void SetVolumenMaster(float valor)
    {
        VolumenMaster = valor;
        PlayerPrefs.SetFloat(KEY_VOL_MASTER, valor);
        AudioListener.volume = valor;
    }

    public void SetVolumenMusica(float valor)
    {
        VolumenMusica = valor;
        PlayerPrefs.SetFloat(KEY_VOL_MUSICA, valor);
        AplicarA(fuentesMusica, valor);
    }

    public void SetVolumenSFX(float valor)
    {
        VolumenSFX = valor;
        PlayerPrefs.SetFloat(KEY_VOL_SFX, valor);
        AplicarA(fuentesSFX, valor);
    }

    public void SetVolumenUI(float valor)
    {
        VolumenUI = valor;
        PlayerPrefs.SetFloat(KEY_VOL_UI, valor);
        AplicarA(fuentesUI, valor);
    }

    public float GetVolumenMaster() => VolumenMaster;
    public float GetVolumenMusica() => VolumenMusica;
    public float GetVolumenSFX()    => VolumenSFX;
    public float GetVolumenUI()     => VolumenUI;

    // ---------- REGISTRO DE AUDIOSOURCES ----------

    public void RegistrarMusica(AudioSource fuente)
    {
        if (fuente == null || fuentesMusica.Contains(fuente)) return;
        fuentesMusica.Add(fuente);
        fuente.volume = VolumenMusica;
    }

    public void RegistrarSFX(AudioSource fuente)
    {
        if (fuente == null || fuentesSFX.Contains(fuente)) return;
        fuentesSFX.Add(fuente);
        fuente.volume = VolumenSFX;
    }

    public void RegistrarUI(AudioSource fuente)
    {
        if (fuente == null || fuentesUI.Contains(fuente)) return;
        fuentesUI.Add(fuente);
        fuente.volume = VolumenUI;
    }

    public void Desregistrar(AudioSource fuente)
    {
        fuentesMusica.Remove(fuente);
        fuentesSFX.Remove(fuente);
        fuentesUI.Remove(fuente);
    }

    private void AplicarA(List<AudioSource> lista, float valor)
    {
        for (int i = lista.Count - 1; i >= 0; i--)
        {
            if (lista[i] == null) { lista.RemoveAt(i); continue; }
            lista[i].volume = valor;
        }
    }

    // ---------- DIFICULTAD ----------

    public void SetDificultad(int valor)
    {
        valor = Mathf.Clamp(valor, 0, 2);
        PlayerPrefs.SetInt(KEY_DIFICULTAD, valor);
        PlayerPrefs.Save();
    }

    public int GetDificultad() => PlayerPrefs.GetInt(KEY_DIFICULTAD, 1);

    // ---------- PUNTAJE ----------

    public bool GuardarPuntaje(int puntaje)
    {
        if (puntaje <= PlayerPrefs.GetInt(KEY_PUNTAJE, 0)) return false;
        PlayerPrefs.SetInt(KEY_PUNTAJE, puntaje);
        PlayerPrefs.Save();
        return true;
    }

    public int GetPuntajeMaximo() => PlayerPrefs.GetInt(KEY_PUNTAJE, 0);

    // ---------- CARGA INICIAL ----------

    private void CargarTodo()
    {
        VolumenMaster = PlayerPrefs.GetFloat(KEY_VOL_MASTER, 0.75f);
        VolumenMusica = PlayerPrefs.GetFloat(KEY_VOL_MUSICA, 0.75f);
        VolumenSFX    = PlayerPrefs.GetFloat(KEY_VOL_SFX,    0.75f);
        VolumenUI     = PlayerPrefs.GetFloat(KEY_VOL_UI,     0.75f);

        AudioListener.volume = VolumenMaster;
        AplicarA(fuentesMusica, VolumenMusica);
        AplicarA(fuentesSFX,    VolumenSFX);
        AplicarA(fuentesUI,     VolumenUI);
    }

    public void GuardarEnDisco() => PlayerPrefs.Save();

    [ContextMenu("Borrar PlayerPrefs")]
    public void BorrarTodo()
    {
        PlayerPrefs.DeleteAll();
        CargarTodo();
        Debug.Log("PlayerPrefs borrados.");
    }
}