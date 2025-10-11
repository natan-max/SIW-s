using UnityEngine;

public class ClockController : MonoBehaviour
{
    [Header("Стрілки годинника")]
    public Transform hourHand;
    public Transform minuteHand;
    public Transform secondHand;

    [Header("Налаштування часу")]
    public float timeScale = 60f; // прискорення часу (1 секунда = X секунд)
    private float hours = 0f;
    private float minutes = 0f;
    private float seconds = 0f;

    private bool gameWon = false;

    void Update()
    {
        if (gameWon) return;

        // Рахуємо час
        seconds += Time.deltaTime * timeScale;

        if (seconds >= 60f)
        {
            minutes += Mathf.Floor(seconds / 60f);
            seconds %= 60f;
        }

        if (minutes >= 60f)
        {
            hours += Mathf.Floor(minutes / 60f);
            minutes %= 60f;
        }

        if (hours >= 12f)
            hours %= 12f;

        // Обертаємо стрілки
        RotateHands();

        // Перевірка перемоги
        if (hours >= 6f && !gameWon)
        {
            gameWon = true;
            WinGame();
        }
    }

    void RotateHands()
    {
        // По осі Z, і колайдери обертаються разом із моделлю
        if (hourHand != null)
        {
            float hourRotation = (hours / 12f * 360f) + (minutes / 60f * 30f);
            hourHand.localRotation = Quaternion.Euler(0f, 0f, -hourRotation);
        }

        if (minuteHand != null)
        {
            float minuteRotation = (minutes / 60f * 360f) + (seconds / 60f * 6f);
            minuteHand.localRotation = Quaternion.Euler(0f, 0f, -minuteRotation);
        }

        if (secondHand != null)
        {
            float secondRotation = (seconds / 60f * 360f);
            secondHand.localRotation = Quaternion.Euler(0f, 0f, -secondRotation);
        }
    }

    void WinGame()
    {
        Debug.Log("Гравець виграв! Годинник показав 6 ранку.");
        // Можна додати UI або завершення рівня
    }
}