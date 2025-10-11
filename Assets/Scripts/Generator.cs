using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Налаштування заряду")]
    public float charge = 100f;
<<<<<<< Updated upstream
    public float drainRate = 5f;

    [Header("Світло генератора (8 шт.)")]
    public Light[] generatorLights;

    [Header("Аудіо")]
    public AudioSource audioSource;
    public AudioClip beepHigh; // сигнал при < thresholdHigh
    public AudioClip beepMid;  // сигнал при < thresholdMid
    public AudioClip beepLow;  // сигнал при < thresholdLow

    [Header("Пороги сигналів")]
    public float thresholdHigh = 25f;
    public float thresholdMid = 15f;
    public float thresholdLow = 10f;
=======
    public float drainRatePerMinute = 1f;
    public Light generatorLight;
    public AudioSource audioSource;
    public AudioClip beep50, beep25, beep10, beepCritical;
>>>>>>> Stashed changes

    private bool[] triggered = new bool[4];
    private bool powerOutage = false;

    // Властивість для інших скриптів
    public bool HasPower
    {
        get { return charge > 0f; }
    }

    void Update()
    {
<<<<<<< Updated upstream
        // Зменшення заряду
        charge -= drainRate * Time.deltaTime;
        charge = Mathf.Max(0f, charge);

        // Керування всіма лампами
        if (generatorLights != null && generatorLights.Length > 0)
        {
            foreach (Light lamp in generatorLights)
            {
                if (lamp != null)
                {
                    if (charge < thresholdLow)
                        lamp.intensity = Mathf.PingPong(Time.time * 5f, 1f);
                    else
                        lamp.intensity = 1f;
                }
            }
        }

        // Звукові сповіщення
        if (charge < thresholdHigh && !triggered[0])
        {
            if (beepHigh != null) audioSource.PlayOneShot(beepHigh);
            triggered[0] = true;
        }
        if (charge < thresholdMid && !triggered[1])
        {
            if (beepMid != null) audioSource.PlayOneShot(beepMid);
            triggered[1] = true;
        }
        if (charge < thresholdLow && !triggered[2])
        {
            if (beepLow != null) audioSource.PlayOneShot(beepLow);
            triggered[2] = true;
        }
=======
        // Розрахунок витрати енергії
        charge -= (drainRatePerMinute / 60f) * Time.deltaTime;
        charge = Mathf.Max(0f, charge);

        // Перевірка на відключення енергії
        if (charge <= 0f && !powerOutage)
        {
            TriggerPowerOutage();
        }
        else if (charge > 0f && powerOutage)
        {
            RestorePower();
        }

        // Світлова індикація
        HandleLightIndicator();

        // Звукові сповіщення
        CheckChargeLevel(50f, beep50, 0);
        CheckChargeLevel(25f, beep25, 1);
        CheckChargeLevel(10f, beep10, 2);
        CheckChargeLevel(5f, beepCritical, 3);

        // Скидання прапорців при заряджанні
        ResetTriggersIfCharged();
    }

    void HandleLightIndicator()
    {
        if (generatorLight != null)
        {
            generatorLight.intensity = charge < 10f ? Mathf.PingPong(Time.time * 5f, 1f) : 1f;
        }
    }

    void CheckChargeLevel(float level, AudioClip clip, int index)
    {
        if (charge < level && !triggered[index])
        {
            PlaySound(clip, index);
        }
    }

    void PlaySound(AudioClip clip, int index)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
            triggered[index] = true;
        }
    }

    void TriggerPowerOutage()
    {
        powerOutage = true;
        Debug.Log("ЕНЕРГІЯ ВИМКНЕНА! Системи не працюють!");
    }

    void RestorePower()
    {
        powerOutage = false;
        Debug.Log("ЕНЕРГІЯ ВІДНОВЛЕНА!");
    }

    void ResetTriggersIfCharged()
    {
        if (charge > 50f)
        {
            for (int i = 0; i < triggered.Length; i++) triggered[i] = false;
        }
        else if (charge > 25f)
        {
            triggered[3] = false;
        }
    }

    public void AddFuel(float amount)
    {
        charge = Mathf.Min(charge + amount, 100f);
        ResetTriggersIfCharged();
    }

    public bool HasPower()
    {
        return charge > 0f;
>>>>>>> Stashed changes
    }
}