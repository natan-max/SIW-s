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

    [Header("Взаємодія")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;

    [Header("UI LOX")]
    public GameObject LOXCanvas;        // Канвас програшу
    public TMP_Text LOXText;            // Текст TMP для LOX
    public GameObject buttonsPanel;     // Панель з кнопками
    public float textSpeed = 0.1f;      // Швидкість друку тексту

    [HideInInspector] public bool canMove = true;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isLOXActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (LOXCanvas != null)
            LOXCanvas.SetActive(false);
        if (buttonsPanel != null)
            buttonsPanel.SetActive(false);
    }

    void Update()
    {
        LookAround();
        HandleRun();

        if (Input.GetKeyDown(KeyCode.E))
            Interact();

        // 🔹 Q/Й для відкриття LOX
        if (Input.GetKeyDown(KeyCode.Q) && !isLOXActive)
        {
            ShowLOX();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (!canMove) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 velocity = move * currentSpeed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

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

    void Interact()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
                interactable.Interact();
        }
    }

    // 🔹 Показати LOX
    void ShowLOX()
    {
        if (LOXCanvas == null || LOXText == null || buttonsPanel == null) return;

        isLOXActive = true;
        canMove = false;
        LOXCanvas.SetActive(true);
        buttonsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

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

    // 🔹 Кнопка Play Again
    public void PlayAgain()
    {
        isLOXActive = false;
        canMove = true;

        if (LOXCanvas != null) LOXCanvas.SetActive(false);
        if (buttonsPanel != null) buttonsPanel.SetActive(false);
        if (LOXText != null) LOXText.text = "";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 🔹 Кнопка Exit
    public void ExitGame()
    {
        Application.Quit();
    }
}
