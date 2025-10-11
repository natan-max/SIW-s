using UnityEngine;

public class Door : MonoBehaviour, IInteractable
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
                Debug.Log("Немає енергії! Двері не відчиняються!");
                return;
            }
        }

        if (!isOpen)
            OpenDoor();
        else
            CloseDoor();
    }

    public void OpenDoor()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
            isOpen = true;
            Debug.Log("Двері відкриті");
        }
    }

    public void CloseDoor()
    {
        if (animator != null)
        {
            animator.SetTrigger("Close");
            isOpen = false;
            Debug.Log("Двері закриті");
        }
    }
}