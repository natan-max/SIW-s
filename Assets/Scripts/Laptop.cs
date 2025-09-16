using UnityEngine;

public class Laptop : MonoBehaviour, IInteractable
{
    [Header("Моделі ноутбука")]
    public GameObject laptopOn;   // Laptop_black
    public GameObject laptopOff;  // Laptop_black_off

    [Header("UI")]
    public GameObject laptopUI;   // Canvas або панель

    private bool isOn = false;
    private bool uiOpen = false;

    public void Interact()
    {
        if (!isOn)
        {
            // Вмикаємо ноутбук
            isOn = true;
            laptopOn.SetActive(true);
            laptopOff.SetActive(false);
            Debug.Log("Laptop turned ON");
        }
        else
        {
            // Якщо вже увімкнено -> показуємо/ховаємо UI
            uiOpen = !uiOpen;
            laptopUI.SetActive(uiOpen);
            Debug.Log("Laptop UI " + (uiOpen ? "Opened" : "Closed"));
        }
    }
}
