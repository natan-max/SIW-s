using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float crouchSpeed = 2f;
    public float jumpForce = 5f;
    private float currentSpeed;
    private bool isGrounded;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    [Header("Interaction")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;
    public Transform handSlot;
    public GameObject laptopOffModel;
    public GameObject laptopOnModel;
    public GameObject panelObject;

    [Header("Crouching")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 6f;
    private CapsuleCollider playerCollider;

    [Header("UI")]
    public Image crosshairImage;
    public Color defaultCrosshairColor = Color.white;
    public Color interactCrosshairColor = Color.red;

    private Rigidbody rb;
    private float xRotation = 0f;
    private GameObject heldObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;

        if (laptopOffModel != null && laptopOnModel != null)
        {
            laptopOffModel.SetActive(true);
            laptopOnModel.SetActive(false);
        }
    }

    void Update()
    {
        LookAround();
        HandleCrouch();
        HandleRun();
        UpdateCrosshair();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
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
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Angle(contact.normal, Vector3.up) < 45f)
            {
                isGrounded = true;
                break;
            }
        }
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

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                return;
            }

            if (hit.collider.gameObject == laptopOffModel || hit.collider.gameObject == laptopOnModel)
            {
                bool turningOn = laptopOffModel.activeSelf;
                laptopOffModel.SetActive(!turningOn);
                laptopOnModel.SetActive(turningOn);
                return;
            }

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
        heldObject.transform.position = playerCamera.position + playerCamera.forward * 1f;

        if (rbItem != null)
        {
            rbItem.isKinematic = false;
            rbItem.velocity = rb.velocity;
            rbItem.AddForce(playerCamera.forward * 3f, ForceMode.Impulse);
        }

        if (col != null)
        {
            col.enabled = true;
        }

        Debug.Log("Викинув: " + heldObject.name);
        heldObject = null;
    }

    void UpdateCrosshair()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            crosshairImage.color = interactCrosshairColor;
        }
        else
        {
            crosshairImage.color = defaultCrosshairColor;
        }
    }
}
