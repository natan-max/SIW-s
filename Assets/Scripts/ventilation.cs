using UnityEngine;

public class Ventilation : MonoBehaviour, IInteractable
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
                Debug.Log("Немає енергії! Вентиляція не працює!");
                return;
            }
        }

        if (!isOpen)
<<<<<<< Updated upstream
            OpenVent();
        else
            CloseVent();
=======
        {
            OpenVent();
        }
        else
        {
            CloseVent();
        }
>>>>>>> Stashed changes
    }

    public void OpenVent()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
            isOpen = true;
        }
    }

    public void CloseVent()
    {
        if (animator != null)
        {
            animator.SetTrigger("Close");
            isOpen = false;
        }
    }
}