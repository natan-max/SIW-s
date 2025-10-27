using UnityEngine;

public class DoorHoldEvents : MonoBehaviour
{
    public void HandleApproachStarted()
    {
        Debug.Log("📌 Подія: Approach Started");
        // тут можна робити UI/звук/анімацію
    }

    public void HandleHoldStarted()
    {
        Debug.Log("📌 Подія: Hold Started");
    }

    public void HandleHoldSuccess()
    {
        Debug.Log("✅ Подія: Hold Success");
    }

    public void HandleHoldFailed()
    {
        Debug.Log("❌ Подія: Hold Failed");
    }

    public void HandleApproachFailed()
    {
        Debug.Log("❌ Подія: Approach Failed");
    }
}