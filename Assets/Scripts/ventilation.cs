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
            if (generator != null && !generator.HasPower)  // <-- видалено ()
            {
                Debug.Log("Немає енергії! Вентиляція не працює!");
                return;
            }
        }

        if (!isOpen)
            OpenVent();
        else
            CloseVent();
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