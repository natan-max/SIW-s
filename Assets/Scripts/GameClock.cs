using UnityEngine;

public class GameClock : MonoBehaviour   // 👈 назва класу = назві файлу
{
    [Header("Стрілки")]
    public Transform hourHand;     // годинна
    public Transform minuteHand;   // хвилинна
    public Transform secondHand;   // секундна

    [Header("Налаштування")]
    public float realMinutesPerGameHour = 2f; // 2 хв = 1 година

    private float gameTime; // час у грі в годинах (0 - 12)
    private bool isWin = false;

    void Start()
    {
        // Почати від 00:00
        gameTime = 0f;

        if (hourHand) hourHand.localRotation = Quaternion.Euler(0, 0, 0);
        if (minuteHand) minuteHand.localRotation = Quaternion.Euler(0, 0, 0);
        if (secondHand) secondHand.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        if (isWin) return;

        // Скільки годин пройшло у грі
        gameTime += (Time.deltaTime / 60f) * (1f / realMinutesPerGameHour);

        if (gameTime >= 12f)
            gameTime -= 12f;

        // Обчислюємо обертання стрілок від 00:00
        float hourRotation = (gameTime / 12f) * 360f;
        float minuteRotation = ((gameTime % 1f) * 60f) / 60f * 360f;
        float secondRotation = ((gameTime * 3600f) % 60f) / 60f * 360f;

        // Повертаємо стрілки
        if (hourHand) hourHand.localRotation = Quaternion.Euler(0, 0, -hourRotation);
        if (minuteHand) minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteRotation);
        if (secondHand) secondHand.localRotation = Quaternion.Euler(0, 0, -secondRotation);

        // Перевірка виграшу (6:00)
        if (Mathf.FloorToInt(gameTime) == 6 && !isWin)
        {
            isWin = true;
            Debug.Log("🎉 Ти виграв! Стрілка дійшла до 6 години ранку!");
        }
    }
}