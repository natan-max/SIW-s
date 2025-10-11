using UnityEngine;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Рух")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    private float currentSpeed;

    [Header("Миша / Камера")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    [Header("UI LOX")]
    public GameObject LOXCanvas;        // Канвас програшу
    public TMP_Text LOXText;            // TMP-текст
    public GameObject buttonsPanel;     // Панель з кнопками
    public AudioSource loseSound;       // Звук програшу
    public float textSpeed = 0.1f;      // Швидкість появи тексту

    [HideInInspector] public bool canMove = true;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isLOXActive = false;
    private Vector3 startPosition; // Початкова позиція гравця
    private Quaternion startRotation; // Початковий поворот гравця

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Зберігаємо початкову позицію та поворот
        startPosition = transform.position;
        startRotation = transform.rotation;

        LockCursor(true);

        if (LOXCanvas != null)
            LOXCanvas.SetActive(false);
        if (buttonsPanel != null)
            buttonsPanel.SetActive(false);
    }

    void Update()
    {
        if (canMove)
        {
            LookAround();
            HandleRun();
        }

        // Програш (Q)
        if (Input.GetKeyDown(KeyCode.Q) && !isLOXActive)
            ShowLOX();
    }

    void FixedUpdate()
    {
        if (canMove)
            Move();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 velocity = move * currentSpeed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    void LookAround()
    {
        if (!canMove || playerCamera == null) return;

        // 🔹 Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 🔹 Вертикальний рух (камера)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 🔹 Горизонтальний рух (тіло)
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleRun()
    {
        if (!canMove)
        {
            currentSpeed = 0f;
            return;
        }

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
    }

    // ==========================
    // 🔹 ПРОГРАШ
    // ==========================
    void ShowLOX()
    {
        if (LOXCanvas == null || LOXText == null || buttonsPanel == null) return;

        isLOXActive = true;
        canMove = false;
        rb.velocity = Vector3.zero;

        LOXCanvas.SetActive(true);
        buttonsPanel.SetActive(false);

        LockCursor(false);

        if (loseSound != null)
            loseSound.Play();

        StartCoroutine(TypeLOXText("ТИ ПРОГРАВ!"));
    }

    IEnumerator TypeLOXText(string text)
    {
        LOXText.text = "";
        foreach (char c in text)
        {
            LOXText.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        buttonsPanel.SetActive(true);
    }

    // ==========================
    // 🔹 КНОПКИ
    // ==========================
    public void PlayAgain()
    {
        StopAllCoroutines();

        LOXCanvas.SetActive(false);
        buttonsPanel.SetActive(false);
        LOXText.text = "";

        // 🔹 Відновлюємо позицію, кут, управління
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.velocity = Vector3.zero;
        playerCamera.localRotation = Quaternion.identity;
        xRotation = 0f;

        StartCoroutine(ReenableControl());
    }

    IEnumerator ReenableControl()
    {
        yield return null; // чекаємо 1 кадр
        isLOXActive = false;
        canMove = true;
        LockCursor(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // 🔹 Курсор (вкл/викл)
    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
