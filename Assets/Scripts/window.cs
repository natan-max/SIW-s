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
<<<<<<< Updated upstream
            if (generator != null && !generator.HasPower)  // <-- видалено ()
=======
            if (generator != null && !generator.HasPower())
>>>>>>> Stashed changes
            {
                Debug.Log("Немає енергії! Вікно не відчиняється!");
                return;
            }
        }

        if (!isOpen)
<<<<<<< Updated upstream
            OpenWindow();
        else
            CloseWindow();
=======
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
>>>>>>> Stashed changes
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