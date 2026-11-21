using UnityEngine;

public class DificultadManager : MonoBehaviour
{
    public static DificultadManager Instance;

    [SerializeField] private DificultadData datos;

    private const string KEY_DIFICULTAD = "dificultad";

    public int DificultadActual { get; private set; }

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
        DificultadActual = PlayerPrefs.GetInt(KEY_DIFICULTAD, 1);
    }

    public void SetDificultad(int valor)
    {
        DificultadActual = Mathf.Clamp(valor, 0, 2);
        PlayerPrefs.SetInt(KEY_DIFICULTAD, DificultadActual);
        PlayerPrefs.Save();
        Debug.Log("Dificultad: " + NombreDificultad(DificultadActual));
    }

    public float GetMultiplicador() => datos.GetMultiplicador(DificultadActual);
    public int GetDanioEnemigo() => datos.GetDanioEnemigo(DificultadActual);
    public int GetVidaEnemigo() => datos.GetVidaEnemigo(DificultadActual);

    public string NombreDificultad(int valor)
    {
        switch (valor)
        {
            case 0: return "Facil";
            case 2: return "Dificil";
            default: return "Normal";
        }
    }
}
