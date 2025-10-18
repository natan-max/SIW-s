using UnityEngine;
using TMPro;
using System.Collections;
using Cinemachine;

public class Player : MonoBehaviour
{
    [Header("Рух")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    private float currentSpeed;

    [Header("Камера")]
    public float mouseSensitivity = 100f;
    
    [Header("Cinemachine")]
    public CinemachineVirtualCamera playerCamera; // Звичайна Virtual Camera
    
    [Header("UI LOX")]
    public GameObject LOXCanvas;
    public TMP_Text LOXText;
    public GameObject buttonsPanel;
    public AudioSource loseSound;
    public float textSpeed = 0.1f;

    [HideInInspector] public bool canMove = true;

    private Rigidbody rb;
    private bool isLOXActive = false;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        // Зберігаємо початковий поворот гравця
        xRotation = transform.eulerAngles.x;
        yRotation = transform.eulerAngles.y;

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
        if (!canMove) return;

        // Миша
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Вертикальний рух (голова)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Горизонтальний рух (тіло)
        yRotation += mouseX;

        // Застосовуємо обертання
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        
        // Обертаємо камеру окремо для вертикального руху
        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
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
        ResetGame();
    }

    void ResetGame()
    {
        StopAllCoroutines();

        // 🔹 Повністю вимикаємо UI
        if (LOXCanvas != null)
            LOXCanvas.SetActive(false);
        if (buttonsPanel != null)
            buttonsPanel.SetActive(false);
        
        LOXText.text = "";

        // 🔹 Відновлюємо позицію гравця
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 🔹 Відновлюємо поворот камери
        xRotation = 0f;
        yRotation = transform.eulerAngles.y;
        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.identity;
        }

        // 🔹 Вмикаємо управління
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