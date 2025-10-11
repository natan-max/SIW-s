using UnityEngine;

public class Window : MonoBehaviour, IInteractable
{
    public Animator animator;
    public bool requiresPower = true;
    private bool isOpen = false;

    public void Interact()
    {
        if (requiresPower)
        {
            Generator generator = FindObjectOfType<Generator>();
            if (generator != null && !generator.HasPower)  // <-- видалено ()
            {
                Debug.Log("Немає енергії! Вікно не відчиняється!");
                return;
            }
        }

        if (!isOpen)
            OpenWindow();
        else
            CloseWindow();
    }

    public void OpenWindow()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
            isOpen = true;
        }
    }

    public void CloseWindow()
    {
        if (animator != null)
        {
            animator.SetTrigger("Close");
            isOpen = false;
        }
    }
}