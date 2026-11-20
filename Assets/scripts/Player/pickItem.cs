using UnityEngine;
using System;

public class PickItem : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 holdOffset = new Vector3(0.4f, 0.3f, 0f);

    private GameObject currentItem = null;

    private void Update()
    {
        if (currentItem != null && player != null)
        {
            // Offset relativo al jugador, sin ser hijo suyo
            currentItem.transform.position = player.position + player.rotation * holdOffset;
            currentItem.transform.rotation = player.rotation;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item") && currentItem == null)
        {
            if (Input.GetKeyDown(KeyCode.E)) Pick(other.gameObject);
        }
    }

    private void Pick(GameObject item)
    {
        currentItem = item;

        item.transform.position = player.position + player.rotation * holdOffset;
        item.transform.rotation = player.rotation;
        item.transform.localScale = Vector3.one;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider collider = item.GetComponent<Collider>();
        if (rb != null) rb.isKinematic = true;
        if (collider != null) collider.enabled = false;
    }

    public GameObject DropItem()
    {
        GameObject temp = currentItem;
        currentItem = null;

        if (temp != null)
        {
            Rigidbody rb = temp.GetComponent<Rigidbody>();
            Collider collider = temp.GetComponent<Collider>();
            if (rb != null) rb.isKinematic = false;
            if (collider != null) collider.enabled = true;
        }

        return temp;
    }
}