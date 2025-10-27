using UnityEngine;

public class GeneratorButton : MonoBehaviour, IInteractable
{
    public GeneratorUI generatorUI;

    public void Interact()
    {
        if (generatorUI != null)
            generatorUI.ToggleUI();
    }
}