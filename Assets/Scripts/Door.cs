using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Animator animator;
    public bool requiresPower = true;
    private bool isOpen = false;

    public void Interact()
    {
<<<<<<< Updated upstream
        if (requiresPower)
        {
            Generator generator = FindObjectOfType<Generator>();
            if (generator != null && !generator.HasPower)  // <-- видалено ()
=======
        // Перевірка чи потрібна енергія
        if (requiresPower)
        {
            Generator generator = FindObjectOfType<Generator>();
            if (generator != null && !generator.HasPower())
>>>>>>> Stashed changes
            {
                Debug.Log("Немає енергії! Двері не відчиняються!");
                return;
            }
        }

<<<<<<< Updated upstream
        if (!isOpen)
            OpenDoor();
        else
            CloseDoor();
=======
        // Відкриття/закриття дверей
        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
>>>>>>> Stashed changes
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