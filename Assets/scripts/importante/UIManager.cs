using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject extrasPanel;
    [SerializeField] private GameObject difficultyPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;

    private const string KEY_MUSICA = "MusicaActiva";
    private const string KEY_DIFICULTAD = "dificultad";
    private bool musicaActiva = true;

    private void Start()
    {
        ShowPanel(titlePanel);

        musicaActiva = PlayerPrefs.GetInt(KEY_MUSICA, 1) == 1;
        AplicarMusica();
    }

    private void ShowPanel(GameObject panel)
    {
        titlePanel.SetActive(panel == titlePanel);
        optionsPanel.SetActive(panel == optionsPanel);
        extrasPanel.SetActive(panel == extrasPanel);
        difficultyPanel.SetActive(panel == difficultyPanel);
    }

    // PLAY -> abre el panel de dificultad
    public void OnPlayPressed()
    {
        Time.timeScale = 1f;
        ShowPanel(difficultyPanel);
    }

    // Botones de dificultad -> guardan y cargan el Tutorial
    public void OnFacilPressed()   { CargarTutorial(1); }
    public void OnNormalPressed()  { CargarTutorial(2); }
    public void OnDificilPressed() { CargarTutorial(3); }

    private void CargarTutorial(int nivel)
    {
        PlayerPrefs.SetInt(KEY_DIFICULTAD, nivel);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Tutorial");
    }

    public void OnOptionsPressed()
    {
        ShowPanel(optionsPanel);
    }

    public void OnCreditsPressed()
    {
        ShowPanel(extrasPanel);
    }

    public void OnBackPressed()
    {
        ShowPanel(titlePanel);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }

    public void ToggleMusic()
    {
        musicaActiva = !musicaActiva;
        AplicarMusica();
    }

    private void AplicarMusica()
    {
        if (musicSource != null) musicSource.mute = !musicaActiva;
        PlayerPrefs.SetInt(KEY_MUSICA, musicaActiva ? 1 : 0);
    }
}
