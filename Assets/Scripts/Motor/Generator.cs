using UnityEngine;

public class Generator : MonoBehaviour
{
    public float charge = 100f;
    public float drainRate = 5f;
    public Light[] lights;
    public AudioSource audioSource;
    public AudioClip runningLoopClip;
    public AudioClip voice50;
    public AudioClip voice25;
    public AudioClip voice10;
    public float threshold50 = 50f;
    public float threshold25 = 25f;
    public float threshold10 = 10f;
    private bool[] triggered = new bool[3];
    public float blinkSpeed = 5f;
    public float maxRunningVolume = 1f;

    private void Start()
    {
        if (audioSource != null && runningLoopClip != null)
        {
            audioSource.loop = true;
            audioSource.clip = runningLoopClip;
            if (charge > 0f) audioSource.Play();
        }
    }

    private void Update()
    {
        charge -= drainRate * Time.deltaTime;
        charge = Mathf.Clamp(charge, 0f, 100f);
        bool isOn = charge > 0f;

        for (int i = 0; i < lights.Length; i++)
        {
            Light l = lights[i];
            if (l == null) continue;
            l.enabled = isOn;
            if (isOn)
            {
                if (charge <= threshold10)
                    l.intensity = Mathf.PingPong(Time.time * blinkSpeed, 1f);
                else
                    l.intensity = Mathf.Clamp01(charge / 100f);
            }
        }

        if (audioSource != null)
        {
            if (isOn)
            {
                if (!audioSource.isPlaying && runningLoopClip != null) audioSource.Play();
                audioSource.volume = Mathf.Clamp01(charge / 100f) * maxRunningVolume;
            }
            else
            {
                if (audioSource.isPlaying) audioSource.Stop();
            }
        }

        if (!triggered[0] && charge < threshold50)
        {
            if (audioSource != null && voice50 != null) audioSource.PlayOneShot(voice50);
            triggered[0] = true;
        }
        if (!triggered[1] && charge < threshold25)
        {
            if (audioSource != null && voice25 != null) audioSource.PlayOneShot(voice25);
            triggered[1] = true;
        }
        if (!triggered[2] && charge < threshold10)
        {
            if (audioSource != null && voice10 != null) audioSource.PlayOneShot(voice10);
            triggered[2] = true;
        }

        if (charge > threshold50 + 5f) triggered[0] = false;
        if (charge > threshold25 + 5f) triggered[1] = false;
        if (charge > threshold10 + 5f) triggered[2] = false;
    }
}
