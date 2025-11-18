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

            DoFlashlightLogic();
        }
    }

    void DoFlashlightLogic()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);

        if (Physics.Raycast(ray, out hit, 10f))
        {
            Debug.Log("Ћуч попав у: " + hit.collider.name);
    
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnHitByLight();
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