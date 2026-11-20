using System.Collections;
using UnityEngine;

public class TiendaCilindro : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private int costo = 30;

    [Header("Referencias visuales")]
    [SerializeField] private Renderer stationRenderer;

    [Header("Colores de estado")]
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color sinMonedasColor = Color.red;

    private bool isOnCooldown = false;

    private void Start()
    {
        if (stationRenderer == null)
            stationRenderer = GetComponent<Renderer>();

        SetColor(normalColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isOnCooldown) return;

        HealthSystem playerHealth = other.GetComponent<HealthSystem>();
        if (playerHealth == null)
        {
            Debug.LogWarning("El jugador no tiene HealthSystem");
            return;
        }

        if (playerHealth.CurrentHealth >= playerHealth.MaxHealth)
        {
            Debug.Log("Vida al maximo, no se cobra");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("No hay GameManager en la escena");
            return;
        }

        int scoreActual = GameManager.Instance.Score;

        if (scoreActual >= costo)
        {
            GameManager.Instance.AddScore(-costo);

            float faltante = playerHealth.MaxHealth - playerHealth.CurrentHealth;
            playerHealth.Heal(faltante);

            Debug.Log($"Curado completo. Score restante: {GameManager.Instance.Score}");
        }
        else
        {
            Debug.Log($"No alcanza: tenes {scoreActual} y necesitas {costo}");
            StartCoroutine(ParpadearRojo());
        }
    }

    private IEnumerator ParpadearRojo()
    {
        isOnCooldown = true;
        SetColor(sinMonedasColor);
        yield return new WaitForSeconds(1f);
        SetColor(normalColor);
        isOnCooldown = false;
    }

    private void SetColor(Color c)
    {
        if (stationRenderer != null)
            stationRenderer.material.color = c;
    }
}
