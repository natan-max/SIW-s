using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Слот для руки")]
    public Transform handSlot;
    private GameObject currentItem;

    public void TryPickupFromOutside(GameObject obj)
    {
        if (currentItem == null && obj.CompareTag("Item"))
        {
            currentItem = obj;

            Rigidbody rb = currentItem.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            currentItem.transform.SetParent(handSlot);
            currentItem.transform.localPosition = Vector3.zero;
            currentItem.transform.localRotation = Quaternion.identity;
        }
    }

    public void DropItem()
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(null);

            Rigidbody rb = currentItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse);
            }

            currentItem = null;
        }
    }
}