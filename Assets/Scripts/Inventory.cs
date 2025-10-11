using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public Transform handSlot;
    private GameObject currentItem;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(GameObject item)
    {
        if (currentItem != null) DropItem();

        currentItem = item;

        item.transform.SetParent(handSlot);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = item.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    public void DropItem()
    {
        if (currentItem == null) return;

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        Collider col = currentItem.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        currentItem.transform.SetParent(null);
        currentItem.transform.position = handSlot.position + handSlot.forward * 1f;

        currentItem = null;
    }

    public GameObject GetCurrentItem()
    {
        return currentItem;
    }
}