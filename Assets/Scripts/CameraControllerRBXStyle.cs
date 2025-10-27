using UnityEngine;
using Cinemachine;

public class CameraControllerRBXStyle : MonoBehaviour
{
    [Header("Камера")]
    public CinemachineVirtualCamera virtualCamera;
    public float mouseSensitivity = 100f;

    [Header("Статус")]
    private bool canLook = false;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        if (virtualCamera != null)
        {
            xRotation = virtualCamera.transform.localEulerAngles.x;
            yRotation = virtualCamera.transform.eulerAngles.y;
        }

        LockCursor(true);
    }

    void Update()
    {
        // Натискаємо ПКМ → активуємо рух камери
        canLook = Input.GetMouseButton(1);

        // Ctrl → зафіксувати камеру
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            canLook = false;

        if (canLook)
            HandleMouseLook();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        // Обертання гравця по Y
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        // Обертання камери по X
        if (virtualCamera != null)
            virtualCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}