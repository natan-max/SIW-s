using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// DoorHoldController — механіка "тримати двері/вентиляцію/вікно".
/// Сценарій: коли ворог знаходиться під дверима (або у вікні/вентиляції),
/// запускається 15-секундний таймер (approachTime), щоб гравець встиг добігти.
/// Коли гравець підходить — потрібно утримувати кнопку (за замовчуванням E)
/// протягом requiredHoldTime (5 секунд). Якщо відпустив раніше — програш.
/// Якщо не встиг підійти за approachTime — програш.
/// Скрипт використовує триггер-колайдер для зони, де вороги "стоять під" об'єктом.
/// Всі повідомлення/інтерфейс опціональні (Text/Slider) — можна підключити у інспекторі.
/// </summary>
[RequireComponent(typeof(Collider))]
public class DoorHoldController : MonoBehaviour
{
    public enum HoldableType { Door, Window, Vent }
    public HoldableType type = HoldableType.Door;

    [Header("Часи (сек)")]
    public float approachTime = 15f;         // час на підхід
    public float requiredHoldTime = 5f;     // час утримання

    [Header("Взаємодія")]
    public KeyCode holdKey = KeyCode.E;     // клавіша утримання
    public float interactRange = 2.5f;      // дистанція взаємодії (вже поруч із дверима)

    [Header("UI (опціонально)")]
    public Text countdownText;              // показати час підходу
    public Slider holdProgressSlider;       // прогрес утримання

    [Header("Події")]
    public UnityEvent OnApproachStarted;
    public UnityEvent OnApproachFailed;     // таймер підходу вичерпався
    public UnityEvent OnHoldStarted;        // гравець почав утримувати
    public UnityEvent OnHoldSuccess;        // утримав requiredHoldTime
    public UnityEvent OnHoldFailed;         // відпустив раніше

    // внутрішні стани
    private bool enemyPresent = false;      // вороги під/біля об'єкта
    private float approachTimer = 0f;
    private bool approachRunning = false;

    private bool playerInRange = false;
    private Transform playerTransform;

    private float holdTimer = 0f;
    private bool isHolding = false;

    private Coroutine approachCoroutine;

    void Awake()
    {
        // collider має бути trigger — використовується як зона "під дверима" для ворогів
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;

        if (countdownText != null) countdownText.gameObject.SetActive(false);
        if (holdProgressSlider != null) { holdProgressSlider.minValue = 0f; holdProgressSlider.maxValue = requiredHoldTime; holdProgressSlider.value = 0f; }
    }

    void Update()
    {
        if (!enemyPresent) return;

        // перевіряємо дистанцію гравця (якщо задано)
        if (playerTransform != null)
        {
            float dist = Vector3.Distance(playerTransform.position, transform.position);
            playerInRange = dist <= interactRange;
        }

        // Якщо гравець почав тримати кнопку і він у діапазоні — рахуємо час утримання
        if (playerInRange && Input.GetKey(holdKey))
        {
            if (!isHolding)
            {
                isHolding = true;
                OnHoldStarted?.Invoke();
            }

            holdTimer += Time.deltaTime;
            if (holdProgressSlider != null) holdProgressSlider.value = holdTimer;

            if (holdTimer >= requiredHoldTime)
            {
                // Успіх
                SuccessHold();
            }
        }
        else
        {
            // Якщо гравець відпустив кнопку до потрібного часу — програш
            if (isHolding && holdTimer < requiredHoldTime)
            {
                FailHold();
            }
            isHolding = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ворог заходить під двері/вікно/вентиляцію
        if (other.CompareTag("Enemy"))
        {
            enemyPresent = true;
            StartApproachCountdown();
        }

        // просте виявлення гравця (щоб перевіряти дистанцію); гравець все одно має підходити
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            float dist = Vector3.Distance(playerTransform.position, transform.position);
            playerInRange = dist <= interactRange;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // коли ворог зникає — зупиняємо лічильники
            enemyPresent = false;
            StopApproachCountdown();
            ResetHoldState();
        }

        if (other.CompareTag("Player"))
        {
            playerTransform = null;
            playerInRange = false;
            // якщо гравець пішов під час утримання — це по суті відпускання -> програш
            if (isHolding) FailHold();
        }
    }

    #region Таймер підходу
    private void StartApproachCountdown()
    {
        if (approachRunning) return;
        approachCoroutine = StartCoroutine(ApproachCountdown());
        OnApproachStarted?.Invoke();
    }

    private void StopApproachCountdown()
    {
        if (!approachRunning) return;
        if (approachCoroutine != null) StopCoroutine(approachCoroutine);
        approachCoroutine = null;
        approachRunning = false;
        approachTimer = 0f;
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    private IEnumerator ApproachCountdown()
    {
        approachRunning = true;
        approachTimer = approachTime;
        if (countdownText != null) { countdownText.gameObject.SetActive(true); }

        while (approachTimer > 0f)
        {
            if (countdownText != null) countdownText.text = Mathf.CeilToInt(approachTimer).ToString();
            approachTimer -= Time.deltaTime;
            yield return null;

            // Якщо гравець почав утримувати під час підйому, не треба нічого робити — утримання перевіряється в Update
        }

        // час вичерпано -> програш
        approachRunning = false;
        countdownText?.gameObject.SetActive(false);
        OnApproachFailed?.Invoke();
        LoseGame("Не встиг підбігти!");
    }
    #endregion

    #region Утримання
    private void SuccessHold()
    {
        ResetApproachAndHold();
        OnHoldSuccess?.Invoke();
        // Тут можна викликати логіку: ендоскелет жертвується і виводить вас з хати
        WinHold("Утримав! Ендо вас врятував.");
    }

    private void FailHold()
    {
        ResetApproachAndHold();
        OnHoldFailed?.Invoke();
        LoseGame("Відпустив занадто рано — програш.");
    }

    private void ResetApproachAndHold()
    {
        StopApproachCountdown();
        ResetHoldState();
    }

    private void ResetHoldState()
    {
        isHolding = false;
        holdTimer = 0f;
        if (holdProgressSlider != null) holdProgressSlider.value = 0f;
    }
    #endregion

    #region Результати гри (приклади, можна підключити свій GameManager)
    private void LoseGame(string reason)
    {
        Debug.Log("Lose: " + reason);
        // Тут викликайте свій екран програшу або SceneManager.LoadScene("LoseScene");
        // Для прикладу — зупинимо гру
        Time.timeScale = 0f;
        // Також можна викликати анімацію/панель програшу через Canvas
    }

    private void WinHold(string reason)
    {
        Debug.Log("Win: " + reason);
        // Викликаємо логіку перемоги: ендоскелет виводить гравця і жертвує собою
        // Наприклад — відкриваємо двері і вимикаємо ворогів
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
