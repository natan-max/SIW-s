using UnityEngine;

public class Generator : MonoBehaviour
{
    public float charge = 100f;
    public float drainRate = 5f;
    public Light generatorLight;
    public AudioSource audioSource;
    public AudioClip beep25, beep15, beep10;

    private bool[] triggered = new bool[3];

    void Update()
    {
        charge -= drainRate * Time.deltaTime;
        charge = Mathf.Max(0f, charge);

        if (generatorLight != null)
        {
            if (charge < 10f)
            {
                generatorLight.intensity = Mathf.PingPong(Time.time * 5f, 1f);
            }
            else
            {
                generatorLight.intensity = 1f;
            }
        }

        if (charge < 25f && !triggered[0]) { audioSource.PlayOneShot(beep25); triggered[0] = true; }
        if (charge < 15f && !triggered[1]) { audioSource.PlayOneShot(beep15); triggered[1] = true; }
        if (charge < 10f && !triggered[2]) { audioSource.PlayOneShot(beep10); triggered[2] = true; }
    }
}