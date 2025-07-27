using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float crouchSpeed = 2f;
    private float currentSpeed;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    [Header("Interaction")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;
    public Transform handSlot;

    [Header("Crouching")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 6f;
    private CapsuleCollider playerCollider;

    private Rigidbody rb;
    private float xRotation = 0f;
    private GameObject heldObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LookAround();
        HandleCrouch();
        HandleRun();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 velocity = move * currentSpeed;

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
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
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = runSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
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
        if (heldObject != null)
        {
            Debug.Log("Уже є предмет у руці");
            return;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            if (hit.collider.CompareTag("Item"))
            {
                PickupItem(hit.collider.gameObject);
            }
        }
    }

    void PickupItem(GameObject item)
    {
        heldObject = item;

        item.transform.SetParent(handSlot);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rbItem = item.GetComponent<Rigidbody>();
        if (rbItem) rbItem.isKinematic = true;

        Collider col = item.GetComponent<Collider>();
        if (col) col.enabled = false;

        Debug.Log("Взяв у руку: " + item.name);
    }

    void DropItem()
    {
        if (heldObject == null) return;

        Rigidbody rbItem = heldObject.GetComponent<Rigidbody>();
        Collider col = heldObject.GetComponent<Collider>();

        heldObject.transform.SetParent(null);

        // Кидаємо предмет трохи вперед
        heldObject.transform.position = playerCamera.position + playerCamera.forward * 1f;

        if (rbItem != null)
        {
            rbItem.isKinematic = false;
            rbItem.velocity = rb.velocity; // щоб предмет "летів" разом з гравцем
            rbItem.AddForce(playerCamera.forward * 3f, ForceMode.Impulse);
        }

        if (col != null)
        {
            col.enabled = true;
        }

        Debug.Log("Викинув: " + heldObject.name);
        heldObject = null;
    }
}
