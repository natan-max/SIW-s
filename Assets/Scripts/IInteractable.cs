using UnityEngine;

/// <summary>
/// Інтерфейс для всіх об'єктів, з якими можна взаємодіяти.
/// Будь-який скрипт, що реалізує IInteractable, повинен реалізувати метод Interact().
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Викликається, коли гравець взаємодіє з об'єктом (натискає E)
    /// </summary>
    void Interact();
}