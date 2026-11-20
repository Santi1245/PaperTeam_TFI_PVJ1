using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "PaperTeam/GameData")]
public class GameData : ScriptableObject
{
    [Header("Progreso persistente")]
    public float currentHealth = 100f;
    public int currentScore = 0;
    public int currentLevel = 1;

    [Header("Economia")]
    public int currentCoins = 0;

    public void ResetProgress(float maxHealth)
    {
        currentHealth = maxHealth;
        currentScore = 0;
        currentLevel = 1;
        currentCoins = 0;
    }
}
