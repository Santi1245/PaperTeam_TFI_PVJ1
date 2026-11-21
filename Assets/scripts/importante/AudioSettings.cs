using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;

    private void Start()
    {
        var pm = PlayerPrefsManager.Instance;
        if (pm == null)
        {
            Debug.LogWarning("AudioSettings: no hay PlayerPrefsManager en la escena.");
            return;
        }

        Init(masterSlider, pm.GetVolumenMaster(), pm.SetVolumenMaster);
        Init(musicSlider,  pm.GetVolumenMusica(), pm.SetVolumenMusica);
        Init(sfxSlider,    pm.GetVolumenSFX(),    pm.SetVolumenSFX);
        Init(uiSlider,     pm.GetVolumenUI(),     pm.SetVolumenUI);
    }

    private void Init(Slider s, float valorInicial, UnityEngine.Events.UnityAction<float> setter)
    {
        if (s == null)
        {
            Debug.LogWarning("AudioSettings: hay un slider sin asignar en el Inspector.");
            return;
        }

        s.value = valorInicial;
        s.onValueChanged.AddListener(setter);
        setter(valorInicial);
    }
}