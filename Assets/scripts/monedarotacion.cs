using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monedarotacion : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0f, 30f * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("monedarotacion: GameManager.Instance es NULL. La moneda NO se recoge.");
            return;
        }

        GameManager.Instance.SumarMoneda(1);
        Destroy(gameObject);
    }
}
