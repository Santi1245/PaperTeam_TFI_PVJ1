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

        // Solo este objeto se persiste, nunca su padre ni el Canvas.
        // Si tiene padre, lo desenganchamos antes para no arrastrar la jerarquia.
        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);

        if (sourceSFX == null) sourceSFX = GetComponent<AudioSource>();
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
