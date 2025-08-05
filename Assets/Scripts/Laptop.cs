using UnityEngine;

public class Laptop : MonoBehaviour, IInteractable
{
    public GameObject laptopOff;
    public GameObject laptopOn;
    private bool isOn = false;

    public void Interact()
    {
        isOn = !isOn;
        laptopOff.SetActive(!isOn);
        laptopOn.SetActive(isOn);
        Debug.Log("Ноутбук тепер: " + (isOn ? "УВІМКНЕНИЙ" : "ВИМКНЕНИЙ"));
    }
}
