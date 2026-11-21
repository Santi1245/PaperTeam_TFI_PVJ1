using UnityEngine;

public class BotonDificultad : MonoBehaviour
{
    [SerializeField] private int dificultad; // 0 = Facil, 1 = Normal, 2 = Dificil

    public void ElegirDificultad()
    {
        if (DificultadManager.Instance == null)
        {
            Debug.LogWarning("No hay DificultadManager en la escena.");
            return;
        }

        DificultadManager.Instance.SetDificultad(dificultad);
    }
}
