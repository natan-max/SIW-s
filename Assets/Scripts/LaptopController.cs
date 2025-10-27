using UnityEngine;

public class LaptopController : MonoBehaviour, IInteractable
{
    [Header("Laptop Models")]
    public GameObject Laptop_black_off;   // вимкнений ноутбук
    public GameObject Laptop_black;       // увімкнений ноутбук

    [Header("UI Panel")]
    public GameObject uiPanel;            // панель, що відкривається після другого натискання

    private int interactionCount = 0;     // кількість взаємодій

    void Start()
    {
        // Початковий стан
        Laptop_black_off.SetActive(true);
        Laptop_black.SetActive(false);
        uiPanel.SetActive(false);
    }

    public void Interact()
    {
        interactionCount++;

        if (interactionCount == 1)
        {
            // Перше натискання — вмикаємо ноутбук
            Laptop_black_off.SetActive(false);
            Laptop_black.SetActive(true);
            Debug.Log("💻 Ноутбук увімкнено");
        }
        else if (interactionCount == 2)
        {
            // Друге натискання — відкриваємо UI
            uiPanel.SetActive(true);
            Debug.Log("🖥️ UI запущено");

            // (опціонально) Пауза гри і курсор
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}