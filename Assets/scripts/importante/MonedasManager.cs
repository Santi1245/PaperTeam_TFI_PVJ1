
using UnityEngine;

public class MonedasManager : MonoBehaviour
{
    public static MonedasManager Instancia;

    [SerializeField] private GameData gameData;

    public int Monedas => gameData != null ? gameData.currentCoins : 0;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else if (Instancia != this)
        {
            Destroy(this);
            return;
        }
    }

    private void Start()
    {
        if (gameData == null)
        {
            Debug.LogError("MonedasManager: falta asignar el asset GameData en el Inspector.");
            return;
        }

        Debug.Log("MonedasManager usa el asset: " + gameData.name +
                  " | monedas: " + gameData.currentCoins);
    }

    public void SumarMonedas(int cantidad)
    {
        if (gameData == null) return;
        if (cantidad <= 0) return;

        gameData.currentCoins += cantidad;

        Debug.Log("Monedas: " + gameData.currentCoins);
    }

    public void GastarMonedas(int cantidad)
    {
        if (gameData == null) return;
        if (cantidad <= 0) return;
        if (gameData.currentCoins < cantidad) return;

        gameData.currentCoins -= cantidad;

        Debug.Log("Monedas restantes: " + gameData.currentCoins);
    }

    private void OnDestroy()
    {
        if (Instancia == this)
        {
            Instancia = null;
        }
    }
}
