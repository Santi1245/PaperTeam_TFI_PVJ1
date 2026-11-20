using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject extrasPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;

    private const string KEY_MUSICA = "MusicaActiva";
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
    }

    public void OnPlayPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Nivel1");
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
