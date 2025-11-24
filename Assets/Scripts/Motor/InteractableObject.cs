using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Налаштування")]
    public string openTrigger = "Open";     // тригер для відкриття
    public string closeTrigger = "Close";   // тригер для закриття

    private Animator animator;
    private bool isOpen = false;
    private bool isAnimating = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        string tag = gameObject.tag.ToLower();

        if (tag == "door" || tag == "ventilation")
        {
            ToggleWithAnimation();
        }
        else if (tag == "window")
        {
            ToggleSimple();
        }
    }

    void ToggleWithAnimation()
    {
        if (isAnimating) return; // блокуємо спам

        if (animator != null)
        {
            if (!isOpen)
            {
                animator.SetTrigger(openTrigger);
            }
            else
            {
                animator.SetTrigger(closeTrigger);
            }
        }

        isOpen = !isOpen;
    }

    void ToggleSimple()
    {
        isOpen = !isOpen;
        Debug.Log($"{gameObject.name} {(isOpen ? "Opened" : "Closed")}");
        // Тут можна, наприклад, крутити вікно або ховати Mesh
        // transform.Rotate(0, isOpen ? 90f : -90f, 0);  <-- приклад
    }

    // Викликати з анімації (Animation Event) в кінці
    public void AnimationFinished()
    {
        isAnimating = false;
    }
}