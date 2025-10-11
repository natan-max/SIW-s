using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Налаштування заряду")]
    public float charge = 100f;
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

    private bool[] triggered = new bool[3];

    // Властивість для інших скриптів
    public bool HasPower
    {
        get { return charge > 0f; }
    }

    void Update()
    {
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
    }
}