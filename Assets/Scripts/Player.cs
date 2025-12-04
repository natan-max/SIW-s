using UnityEngine;
using TMPro;
using System.Collections;
using Cinemachine;

public class Player : MonoBehaviour
{
    public InventoryCore InventoryCore;
    
    [Header("Рух")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    private float currentSpeed;

    [Header("Камера")]
    public float mouseSensitivity = 100f;

    [Header("Cinemachine")]
    public CinemachineVirtualCamera playerCamera;

    [Header("UI LOX")]
    public GameObject LOXCanvas;
    public TMP_Text LOXText;
    public GameObject buttonsPanel;
    public AudioSource loseSound;
    public float textSpeed = 0.1f;

    [Header("Слот руки")]
    [HideInInspector] public MonoBehaviour currentHeldItem;

    [Header("Стан гравця")]
    public bool canMove = true;

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

        // Ініціалізація обертання
        xRotation = 0f;
        yRotation = transform.eulerAngles.y;

        LockCursor(true);

        if (LOXCanvas != null) LOXCanvas.SetActive(false);
        if (buttonsPanel != null) buttonsPanel.SetActive(false);
    }

    void Update()
    {
        if (canMove)
        {
            LookAround();
            HandleRun();
            Move();
            
            InventoryCore.HandleItemDrop();
            InventoryCore.HandleSlotsSwitch();

        }

        HandleFuelFilling();
        HandleLOX();
    }

    private void LookAround()
    {
        // Читання миші
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Обмеження вертикалі
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Горизонталь (обертання гравця)
        yRotation += mouseX;

        // Обертання гравця по Y
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        // Обертання камери локально по X
        if (playerCamera != null)
        {
            // Тут ми не чіпаємо Brain — просто локальна ротація
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 velocity = move * currentSpeed;
        velocity.y = rb.velocity.y; // залишаємо гравітацію
        rb.velocity = velocity;
    }

    private void HandleRun()
    {
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
    }

    private void HandleFuelFilling()
    {
        
    }

    private void HandleLOX()
    {
    }

    // ==========================
    // 🔹 ПРОГРАШ
    // ==========================
    public void ShowLOX()
    {
        if (LOXCanvas == null || LOXText == null || buttonsPanel == null) return;

        isLOXActive = true;
        canMove = false;
        rb.velocity = Vector3.zero;

        LOXCanvas.SetActive(true);
        buttonsPanel.SetActive(false);
        LockCursor(false);

        if (loseSound != null) loseSound.Play();
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

    public void PlayAgain() => ResetGame();

    private void ResetGame()
    {
        StopAllCoroutines();
        if (LOXCanvas != null) LOXCanvas.SetActive(false);
        if (buttonsPanel != null) buttonsPanel.SetActive(false);
        LOXText.text = "";

        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        xRotation = 0f;
        yRotation = transform.eulerAngles.y;

        if (playerCamera != null)
            playerCamera.transform.localRotation = Quaternion.identity;

        isLOXActive = false;
        canMove = true;
        LockCursor(true);
    }

    public void ExitGame() => Application.Quit();

    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
