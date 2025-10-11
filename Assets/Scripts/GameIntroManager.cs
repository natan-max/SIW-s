using UnityEngine;
using System.Collections;

public class GameIntroManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip welcomeSound;
    public AudioClip companyCrashSound;
    public AudioClip hackingSound;
    public AudioClip endoExplanationSound;

    public float delayBetweenClips = 1.5f;
    
    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // Чекаємо один кадр щоб все ініціалізувалось
        yield return null;
        
        // Відтворюємо звуки по порядку
        if (welcomeSound != null)
        {
            audioSource.PlayOneShot(welcomeSound);
            yield return new WaitForSeconds(welcomeSound.length + delayBetweenClips);
        }

        if (companyCrashSound != null)
        {
            audioSource.PlayOneShot(companyCrashSound);
            yield return new WaitForSeconds(companyCrashSound.length + delayBetweenClips);
        }

        if (hackingSound != null)
        {
            audioSource.PlayOneShot(hackingSound);
            yield return new WaitForSeconds(hackingSound.length + delayBetweenClips);
        }

        if (endoExplanationSound != null)
        {
            audioSource.PlayOneShot(endoExplanationSound);
        }
    }
}