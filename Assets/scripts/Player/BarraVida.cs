using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [SerializeField] private Image rellenoBarraVida;

    private HealthSystem playerHealth;

    private void Start()
    {
        if (rellenoBarraVida == null)
            rellenoBarraVida = GetComponent<Image>();

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
            playerHealth = jugador.GetComponent<HealthSystem>();

        if (playerHealth == null)
            Debug.LogWarning("BarraVida: no se encontro el HealthSystem del jugador");
    }

    private void Update()
    {
        if (playerHealth == null || rellenoBarraVida == null) return;

        rellenoBarraVida.fillAmount = playerHealth.CurrentHealth / playerHealth.MaxHealth;
    }
}
