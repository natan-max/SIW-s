using UnityEngine;

public class Battery : MonoBehaviour, IInteractable
{
    public float chargeAmount = 30f;

    public void Interact()
    {
        Player player = FindObjectOfType<Player>();
        if (player == null) return;

        if (player.currentHeldItem is Flashlight flashlight)
        {
            flashlight.AddBattery(chargeAmount);
            Destroy(gameObject);
        }
    }
}