using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableToggle : MonoBehaviour
{
    public enum Mode { Bool, Trigger }
    public Mode mode = Mode.Bool;

    [Header("Animator")]
    public Animator animator;
    public string boolParameter = "isOpen";    // для Mode.Bool
    public string openTrigger = "Open";        // для Mode.Trigger
    public string closeTrigger = "Close";      // для Mode.Trigger

    [Header("Налаштування")]
    public bool startOpen = false;
    public bool interactable = true; // можна вимикати взаємодію через інспектор

    private bool isOpen;

    void Start()
    {
        isOpen = startOpen;
        if (animator != null && mode == Mode.Bool)
            animator.SetBool(boolParameter, isOpen);
    }

    // Викликається зовні (наприклад, CrosshairController коли натиснуто E)
    public void Interact()
    {
        if (!interactable) return;

        if (animator == null)
        {
            Debug.LogWarning($"{name}: Animator не встановлено!");
            return;
        }

        if (mode == Mode.Bool)
        {
            isOpen = !isOpen;
            animator.SetBool(boolParameter, isOpen);
        }
        else // Trigger
        {
            if (!isOpen)
            {
                animator.SetTrigger(openTrigger);
                isOpen = true;
            }
            else
            {
                animator.SetTrigger(closeTrigger);
                isOpen = false;
            }
        }
    }

    // опціонально — щоб інші скрипти могли дізнатись стан
    public bool IsOpen() => isOpen;
}