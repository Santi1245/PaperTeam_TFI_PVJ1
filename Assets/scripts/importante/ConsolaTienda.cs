using UnityEngine;

public class ConsolaTienda : MonoBehaviour
{
    [SerializeField] private TiendaManager tiendaManager;
    [SerializeField] private GameObject indicadorInteraccion;
    [SerializeField] private KeyCode teclaInteraccion = KeyCode.E;

    private bool jugadorCerca = false;

    private void Start()
    {
        if (indicadorInteraccion != null) indicadorInteraccion.SetActive(false);
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(teclaInteraccion))
        {
            tiendaManager.AbrirTienda();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (indicadorInteraccion != null) indicadorInteraccion.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (indicadorInteraccion != null) indicadorInteraccion.SetActive(false);
        }
    }
}
