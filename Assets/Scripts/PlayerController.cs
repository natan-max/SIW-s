using UnityEngine;
<<<<<<< Updated upstream:Assets/Scripts/Player.cs
using TMPro;
using System.Collections;
=======
>>>>>>> Stashed changes:Assets/Scripts/PlayerController.cs

public class PlayerController : MonoBehaviour
{
    [Header("Рух")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    private float currentSpeed;

    [Header("Миша / Камера")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

<<<<<<< Updated upstream:Assets/Scripts/Player.cs
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
=======
    [Header("Crouching")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 6f;
    private CapsuleCollider playerCollider;

    [Header("Interaction")]
    public float interactRange = 3f;

    private Rigidbody rb;
    private float xRotation = 0f;
>>>>>>> Stashed changes:Assets/Scripts/PlayerController.cs

    void Start()
    {
        rb = GetComponent<Rigidbody>();

<<<<<<< Updated upstream:Assets/Scripts/Player.cs
        // Зберігаємо початкову позицію та поворот
        startPosition = transform.position;
        startRotation = transform.rotation;

        LockCursor(true);

        if (LOXCanvas != null)
            LOXCanvas.SetActive(false);
        if (buttonsPanel != null)
            buttonsPanel.SetActive(false);
=======
        if (Inventory.Instance == null)
            Debug.LogError("⚠️ На Player немає Inventory!");
>>>>>>> Stashed changes:Assets/Scripts/PlayerController.cs
    }

    void Update()
    {
<<<<<<< Updated upstream:Assets/Scripts/Player.cs
        if (canMove)
        {
            LookAround();
            HandleRun();
        }

        // Програш (Q)
        if (Input.GetKeyDown(KeyCode.Q) && !isLOXActive)
            ShowLOX();
=======
        LookAround();
        HandleCrouch();
        HandleRun();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();

        if (Input.GetKeyDown(KeyCode.E))
            Interact();

        if (Input.GetKeyDown(KeyCode.Q))
            Inventory.Instance.DropItem();
>>>>>>> Stashed changes:Assets/Scripts/PlayerController.cs
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
<<<<<<< Updated upstream:Assets/Scripts/Player.cs
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
=======
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
            currentSpeed = runSpeed;
        else if (Input.GetKey(KeyCode.LeftControl))
            currentSpeed = crouchSpeed;
        else
            currentSpeed = walkSpeed;
    }

    void HandleCrouch()
    {
        float targetHeight = Input.GetKey(KeyCode.LeftControl) ? crouchHeight : standingHeight;
        float newHeight = Mathf.Lerp(playerCollider.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        playerCollider.height = newHeight;
        Vector3 center = playerCollider.center;
        center.y = newHeight / 2f;
        playerCollider.center = center;
    }

    void Interact()
    {
        if (Inventory.Instance.GetCurrentItem() != null) return;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("Item"))
                Inventory.Instance.AddItem(hit.collider.gameObject);
        }
>>>>>>> Stashed changes:Assets/Scripts/PlayerController.cs
    }
}
