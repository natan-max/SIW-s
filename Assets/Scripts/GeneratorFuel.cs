using UnityEngine;

public class GeneratorFuel : MonoBehaviour, IInteractable
{
    [Header("Посилання на генератор")]
    public Generator generator; 

    [Header("UI генератора")]
    public GeneratorUI generatorUI;

    [Header("Паливо на одиницю")]
    public float fuelAmount = 25f;
    

    public void Interact()
    {
        if (generator != null)
        {
            generator.charge += fuelAmount;
            if (generator.charge > 100f) generator.charge = 100f;

            Debug.Log("⛽ Паливо додано: " + generator.charge + "/100");
        }

        // Відкриваємо/закриваємо UI тільки при натисканні кнопки
        if (generatorUI != null)
        {
            generatorUI.ToggleUI();
        }
    }
}