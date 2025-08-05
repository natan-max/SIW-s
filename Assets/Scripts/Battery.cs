using UnityEngine;

public class Battery : MonoBehaviour, IInteractable
{
    public float chargeAmount = 30f;

    public void Interact()
    {
        Flashlight flashlight = FindObjectOfType<Flashlight>();
        if (flashlight != null)
        {
            flashlight.AddBattery(chargeAmount);
            Destroy(gameObject);
        }
    }
}