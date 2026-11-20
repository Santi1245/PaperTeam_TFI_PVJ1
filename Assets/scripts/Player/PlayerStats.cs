using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "paperTeam/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Vida")]
    public float maxHealth = 100f;

    [Header("Movimiento")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;

    [Header("Combate")]
    public float damage = 20f;
}
