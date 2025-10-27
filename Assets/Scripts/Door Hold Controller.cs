using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorAnimatorController : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animator не знайдено на об’єкті!");
    }

    public void Interact()
    {
        if (animator == null) return;

        isOpen = !isOpen; // міняємо стан
        animator.SetBool("IsOpen", isOpen);
    }
}