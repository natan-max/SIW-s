using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Налаштування заряду")]
    public float charge = 100f;
    public float drainRate = 5f;

    [Header("Світло генератора (8 шт.)")]
    public Light[] generatorLights; // масив світильників

    [Header("Аудіо")]
    public AudioSource audioSource;
    public AudioClip beepHigh;   // перший сигнал
    public AudioClip beepMid;    // другий сигнал
    public AudioClip beepLow;    // третій сигнал

    [Header("Пороги сповіщення")]
    public float thresholdHigh = 25f; 
    public float thresholdMid = 15f;
    public float thresholdLow = 10f;

    private bool[] triggered = new bool[3];

    void Update()
    {
        // Розрядка генератора
        charge -= drainRate * Time.deltaTime;
        charge = Mathf.Max(0f, charge);

        // Керування всіма світильниками
        if (generatorLights != null && generatorLights.Length > 0)
        {
            foreach (Light lamp in generatorLights)
            {
                if (lamp != null)
                {
                    if (charge < thresholdLow)
                    {
                        lamp.intensity = Mathf.PingPong(Time.time * 5f, 1f);
                    }
                    else
                    {
                        lamp.intensity = 1f;
                    }
                }
            }
        }

        // Звукові сигнали
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