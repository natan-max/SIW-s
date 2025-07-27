using UnityEngine;

public class PickableItem : MonoBehaviour, IInteractable
{
    public string itemName;

    public void Interact()
    {
        Debug.Log("Підібрано предмет: " + itemName); 
        Destroy(gameObject);
    }
}