using UnityEngine;
using System.Collections;

[System.Serializable]
public class UASItem
{
    [Header("Об’єкт(и) для анімації")]
    public GameObject[] targetObjects;

    [Header("Collider для взаємодії")]
    public Collider interactionCollider;

    [Header("Renderers для підсвічування")]
    public Renderer[] renderers;

    [Header("Animator та анімація")]
    public Animator animator;
    public string animationName = "Open";

    [Header("Підсвічування")]
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 2f;

    [HideInInspector] public bool isOpen = false;
    [HideInInspector] public bool isAnimating = false;
    [HideInInspector] public Coroutine highlightCoroutine;
}

public class UAS : MonoBehaviour
{
    [Header("Список взаємодій")]
    public UASItem[] interactables;

    [Header("Гравець")]
    public Transform playerTransform;
    public Camera playerCamera;

    [Header("Взаємодія")]
    public KeyCode interactKey = KeyCode.Mouse0;
    public float maxDistance = 5f;
    public float highlightSpeed = 5f;

    private UASItem currentHighlighted = null;

    void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        // Ініціалізація Animator для targetObjects
        foreach (var item in interactables)
        {
            if (item.animator == null && item.targetObjects != null)
            {
                foreach (var obj in item.targetObjects)
                {
                    if (obj == null) continue;
                    Animator anim = obj.GetComponentInChildren<Animator>();
                    if (anim != null)
                    {
                        item.animator = anim;
                        item.animator.Play(item.animationName, -1, 0f);
                        item.animator.speed = 0f; // зупиняємо автоматичне відтворення
                        break;
                    }
                }
            }
        }
    }

    void Update()
    {
        if (playerCamera == null || playerTransform == null) return;

        HandleHighlight();

        if (Input.GetKeyDown(interactKey) && currentHighlighted != null)
        {
            if (!currentHighlighted.isAnimating)
                StartCoroutine(AnimateItem(currentHighlighted));
        }
    }

    private void HandleHighlight()
    {
        UASItem newHighlight = null;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            foreach (var item in interactables)
            {
                if (item.interactionCollider == null) continue;

                float dist = Vector3.Distance(playerTransform.position, item.interactionCollider.ClosestPoint(playerTransform.position));
                if (dist <= maxDistance && (hit.collider == item.interactionCollider || hit.collider.transform.IsChildOf(item.interactionCollider.transform)))
                {
                    newHighlight = item;
                    break;
                }
            }
        }

        // Знімання підсвічування з попереднього
        if (currentHighlighted != null && currentHighlighted != newHighlight)
        {
            if (currentHighlighted.highlightCoroutine != null)
                StopCoroutine(currentHighlighted.highlightCoroutine);
            currentHighlighted.highlightCoroutine = StartCoroutine(FadeEmission(currentHighlighted, false));
        }

        // Підсвічування нового
        if (newHighlight != null && currentHighlighted != newHighlight)
        {
            if (newHighlight.highlightCoroutine != null)
                StopCoroutine(newHighlight.highlightCoroutine);
            newHighlight.highlightCoroutine = StartCoroutine(FadeEmission(newHighlight, true));
        }

        currentHighlighted = newHighlight;

        // Зняття підсвітки, якщо відходить
        foreach (var item in interactables)
        {
            if (item.interactionCollider == null) continue;
            float dist = Vector3.Distance(playerTransform.position, item.interactionCollider.ClosestPoint(playerTransform.position));
            if (dist > maxDistance && item.highlightCoroutine != null)
            {
                StopCoroutine(item.highlightCoroutine);
                item.highlightCoroutine = StartCoroutine(FadeEmission(item, false));
            }
        }
    }

    private IEnumerator FadeEmission(UASItem item, bool fadeIn)
    {
        if (item.renderers == null || item.renderers.Length == 0) yield break;

        float targetIntensity = fadeIn ? item.highlightIntensity : 0f;

        while (true)
        {
            bool done = true;
            foreach (var r in item.renderers)
            {
                if (r == null || !r.material.HasProperty("_EmissionColor")) continue;

                Color current = r.material.GetColor("_EmissionColor");
                Color target = fadeIn ? item.highlightColor * targetIntensity : Color.black;
                Color newColor = Color.Lerp(current, target, Time.deltaTime * highlightSpeed);
                r.material.SetColor("_EmissionColor", newColor);

                if ((fadeIn && newColor.maxColorComponent < target.maxColorComponent - 0.01f) ||
                    (!fadeIn && newColor.maxColorComponent > 0.01f))
                    done = false;
            }

            if (done) break;
            yield return null;
        }
    }

    private IEnumerator AnimateItem(UASItem item)
    {
        if (item.animator == null) yield break;

        item.isAnimating = true;

        // Дізнаємось довжину анімації
        AnimationClip clip = null;
        RuntimeAnimatorController rac = item.animator.runtimeAnimatorController;
        foreach (var c in rac.animationClips)
        {
            if (c.name == item.animationName)
            {
                clip = c;
                break;
            }
        }

        if (clip == null)
        {
            Debug.LogWarning($"Анімація '{item.animationName}' не знайдена!");
            item.isAnimating = false;
            yield break;
        }

        float duration = clip.length;
        float timer = 0f;

        float start = item.isOpen ? 1f : 0f;
        float end = item.isOpen ? 0f : 1f;

        item.animator.speed = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float normalizedTime = Mathf.Lerp(start, end, t);
            item.animator.Play(item.animationName, 0, normalizedTime);
            yield return null;
        }

        item.animator.Play(item.animationName, 0, end);
        item.isOpen = !item.isOpen;
        item.isAnimating = false;
    }
}
