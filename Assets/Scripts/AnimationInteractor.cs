using UnityEngine;

public class AnimationInteractor : MonoBehaviour
{
    [Header("Список аніматорів для взаємодії")]
    public Animator[] animators; // додаєш сюди всі Animator-и в Inspector
    [Header("Назва тригера")]
    public string triggerName = "Play"; // ім'я тригера (налаштовуєш в Animator)

    [Header("Налаштування взаємодії")]
    public KeyCode interactKey = KeyCode.E; // кнопка взаємодії
    public float interactDistance = 3f; // дистанція взаємодії
    public Transform player; // сюди перетягуєш гравця

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            // перевірка дистанції
            float dist = Vector3.Distance(player.position, transform.position);
            if (dist <= interactDistance)
            {
                PlayAnimations();
            }
        }
    }

    private void PlayAnimations()
    {
        foreach (Animator anim in animators)
        {
            if (anim != null)
            {
                anim.SetTrigger(triggerName); // запускаємо тригер
            }
        }
    }
}