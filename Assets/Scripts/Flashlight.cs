using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlightLight;
    public float batteryLife = 100f;
    public float drainRate = 10f;
    public float maxBatteryLife = 100f;

    private bool isOn = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if (isOn)
        {
            batteryLife -= drainRate * Time.deltaTime;
            if (batteryLife <= 0f)
            {
                batteryLife = 0f;
                flashlightLight.enabled = false;
                isOn = false;
            }
        }
    }

    public void ToggleFlashlight()
    {
        if (batteryLife > 0f)
        {
            isOn = !isOn;
            flashlightLight.enabled = isOn;
        }
    }

    public void AddBattery(float amount)
    {
        batteryLife = Mathf.Clamp(batteryLife + amount, 0f, maxBatteryLife);
    }
}