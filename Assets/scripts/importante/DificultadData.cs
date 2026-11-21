using UnityEngine;

[CreateAssetMenu(fileName = "DificultadData", menuName = "LittleAdventure/Dificultad")]
public class DificultadData : ScriptableObject
{
    [Header("Facil")]
    public float multiplicadorFacil = 0.5f;
    public int danioEnemigoFacil = 1;
    public int vidaEnemigoFacil = 1;

    [Header("Normal")]
    public float multiplicadorNormal = 1f;
    public int danioEnemigoNormal = 2;
    public int vidaEnemigoNormal = 2;

    [Header("Dificil")]
    public float multiplicadorDificil = 2f;
    public int danioEnemigoDificil = 4;
    public int vidaEnemigoDificil = 4;

    public float GetMultiplicador(int dificultad)
    {
        switch (dificultad)
        {
            case 0: return multiplicadorFacil;
            case 2: return multiplicadorDificil;
            default: return multiplicadorNormal;
        }
    }

    public int GetDanioEnemigo(int dificultad)
    {
        switch (dificultad)
        {
            case 0: return danioEnemigoFacil;
            case 2: return danioEnemigoDificil;
            default: return danioEnemigoNormal;
        }
    }

    public int GetVidaEnemigo(int dificultad)
    {
        switch (dificultad)
        {
            case 0: return vidaEnemigoFacil;
            case 2: return vidaEnemigoDificil;
            default: return vidaEnemigoNormal;
        }
    }
}
