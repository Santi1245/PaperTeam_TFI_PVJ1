using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Configuracion Salud")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    [SerializeField] private bool isDead;
    public bool IsDead => isDead;

    [Header("Persistencia (solo Player)")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameData gameData;

    [Header("Animacion (solo Player)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string estadoMuerte = "death";
    [SerializeField] private string estadoIdle = "Idle";

    private bool EsPlayer => CompareTag("Player");

    private void Awake()
    {
        if (EsPlayer)
        {
            if (animator == null) animator = GetComponent<Animator>();

            if (playerStats != null)
                maxHealth = playerStats.maxHealth;

            if (gameData != null)
                currentHealth = Mathf.Clamp(gameData.currentHealth, 0f, maxHealth);
            else
                currentHealth = maxHealth;

            isDead = false;

            if (animator != null)
            {
                animator.SetBool("isDead", false);
                animator.SetBool("isWalking", false);
                Debug.Log("ANIMATOR OK en Player | bool isDead existe: " + TieneParametro("isDead"));
            }
            else
            {
                Debug.LogWarning("NO HAY ANIMATOR en el Player");
            }
        }
        else
        {
            currentHealth = maxHealth;
            isDead = false;
        }

        Debug.Log($"Salud actual de {gameObject.name}: {currentHealth}");
    }

    private bool TieneParametro(string nombre)
    {
        if (animator == null) return false;
        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            if (p.name == nombre) return true;
        }
        return false;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        if (amount <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        GuardarEnGameData();

        Debug.Log($"Salud actual de {gameObject.name}: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Dead();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        if (amount <= 0f) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        GuardarEnGameData();

        Debug.Log($"Salud actual de {gameObject.name}: {currentHealth}");
    }

    private void GuardarEnGameData()
    {
        if (EsPlayer && gameData != null)
            gameData.currentHealth = currentHealth;
    }

    private void Dead()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} eliminado");

        if (EsPlayer)
        {
            PlayerMovement pm = GetComponent<PlayerMovement>();
            if (pm != null) pm.IsStop = true;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            if (animator != null)
            {
                Debug.Log("MURIENDO -> mando isDead=true | bool existe: " + TieneParametro("isDead"));
                animator.SetBool("isWalking", false);
                animator.SetBool("isDead", true);

                // Fuerza el estado de muerte en la capa 0, sin depender de la transición
                if (TieneEstado(estadoMuerte))
                    animator.Play(estadoMuerte, 0, 0f);
                else
                    Debug.LogWarning("No existe el estado '" + estadoMuerte + "' en el Animator");
            }

            StartCoroutine(AvisarMuerteUnFrameDespues());
        }
        else
        {
            if (GameManager.Instance != null)
                GameManager.Instance.NotifyEnemyDefeat();

            Transform target = transform.parent != null ? transform.parent : transform;
            Destroy(target.gameObject);
        }
    }

    private System.Collections.IEnumerator AvisarMuerteUnFrameDespues()
    {
        yield return null; // deja que el Animator entre al estado death
        yield return null;

        if (GameManager.Instance != null)
            GameManager.Instance.NotifyPlayerDead();
    }

    private bool TieneEstado(string nombre)
    {
        if (animator == null) return false;
        if (animator.runtimeAnimatorController == null) return false;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == nombre) return true;
        }
        return false;
    }

    // Llamalo al reiniciar partida / revivir
    public void Revivir()
    {
        isDead = false;
        currentHealth = maxHealth;
        GuardarEnGameData();

        if (animator != null)
        {
            animator.SetBool("isDead", false);
            animator.SetBool("isWalking", false);
            if (TieneEstado(estadoIdle))
                animator.Play(estadoIdle, 0, 0f);
        }

        PlayerMovement pm = GetComponent<PlayerMovement>();
        if (pm != null) pm.IsStop = false;

        Debug.Log("Jugador revivido. Salud: " + currentHealth);
    }
}
