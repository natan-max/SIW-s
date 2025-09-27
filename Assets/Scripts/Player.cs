using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Рух")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpForce = 5f;
    private float currentSpeed;

    [Header("Миша / Камера")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    [Header("Взаємодія")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;

    [HideInInspector] public bool canMove = true; // 🔹 прапорець для блокування руху

    private Rigidbody rb;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LookAround();
        HandleRun();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (!canMove) return; // 🔹 блокуємо рух при відкритому UI

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 velocity = move * currentSpeed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;
    }

    void LookAround()
    {
        // 🔹 огляд завжди працює, навіть якщо рух заблокований
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

        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;
    }

    void Interact()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            Debug.Log("Взаємодія з: " + hit.collider.name);

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
