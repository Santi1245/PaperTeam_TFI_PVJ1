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

    // Valores originales para poder volver a ellos
    private float defaultMaxHealth;
    private float defaultWalkSpeed;
    private float defaultRunSpeed;
    private float defaultDamage;

    private void OnEnable()
    {
        // Guarda los valores tal como estan en el asset
        defaultMaxHealth = maxHealth;
        defaultWalkSpeed = walkSpeed;
        defaultRunSpeed = runSpeed;
        defaultDamage = damage;
    }

    public void ResetStats()
    {
        maxHealth = defaultMaxHealth;
        walkSpeed = defaultWalkSpeed;
        runSpeed = defaultRunSpeed;
        damage = defaultDamage;
    }
}
