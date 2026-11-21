using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Clips")]
    [SerializeField] private AudioClip[] audioclips;
    [SerializeField] private AudioClip[] uiClips;

    [Header("Sources")]
    [SerializeField] private AudioSource sourceSFX;
    [SerializeField] private AudioSource sourceUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);

        if (sourceSFX == null) sourceSFX = GetComponent<AudioSource>();
        if (sourceUI  == null) sourceUI  = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (PlayerPrefsManager.Instance == null) return;

        PlayerPrefsManager.Instance.RegistrarSFX(sourceSFX);
        PlayerPrefsManager.Instance.RegistrarUI(sourceUI);
    }

    private void OnDestroy()
    {
        if (Instance != this || PlayerPrefsManager.Instance == null) return;

        PlayerPrefsManager.Instance.Desregistrar(sourceSFX);
        PlayerPrefsManager.Instance.Desregistrar(sourceUI);
    }

    public void PlaySFX(int index)
    {
        if (sourceSFX == null) return;
        if (audioclips == null || index < 0 || index >= audioclips.Length) return;

        sourceSFX.PlayOneShot(audioclips[index]);
    }

    public void PlayUI(int index)
    {
        if (sourceUI == null) return;
        if (uiClips == null || index < 0 || index >= uiClips.Length) return;

        sourceUI.PlayOneShot(uiClips[index]);
    }
}

