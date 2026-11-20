using UnityEngine;

public class MonedasManager : MonoBehaviour
{
    public static MonedasManager Instancia;

    [SerializeField] private int monedas = 0;

    public int Monedas => monedas;

    private void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    public void SumarMonedas(int cantidad)
    {
        monedas += cantidad;
        Debug.Log("Monedas: " + monedas);
    }

    public void GastarMonedas(int cantidad)
    {
        monedas -= cantidad;
        if (monedas < 0) monedas = 0;
    }
}
