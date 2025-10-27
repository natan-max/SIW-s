using UnityEngine;

/// <summary>
/// Скрипт для об’єктів з Animator (двері, вентиля, вікна).
/// Реалізує IInteractable, дозволяє відкривати/закривати через CrosshairController.
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractableToggle : MonoBehaviour, IInteractable
{
    public enum Mode { Bool, Trigger }
    public Mode mode = Mode.Bool;

    [Header("Animator")]
    public Animator animator;
    public string boolParameter = "isOpen";    // для Mode.Bool
    public string openTrigger = "Open";        // для Mode.Trigger
    public string closeTrigger = "Close";      // для Mode.Trigger

    [Header("Налаштування")]
    public bool startOpen = false;             // чи початково відкрито
    public bool interactable = true;           // можна вимикати взаємодію

    private bool isOpen;

    void Start()
    {
        isOpen = startOpen;
        if (animator != null && mode == Mode.Bool)
            animator.SetBool(boolParameter, isOpen);
    }

    /// <summary>
    /// Викликається CrosshairController при натисканні E
    /// </summary>
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
        else // Mode.Trigger
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

        Debug.Log($"{name}: Взаємодія виконана, isOpen = {isOpen}");
    }

    /// <summary>
    /// Повертає стан об’єкта
    /// </summary>
    public bool IsOpen() => isOpen;
}