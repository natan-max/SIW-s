using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras (Drag your CinemachineVirtualCameras here)")]
    public CinemachineVirtualCamera[] cameras; // усі камери
    private int currentIndex = 0;

    [Header("UI")]
    public GameObject cameraUI; // Canvas для UI камер
    public Button exitButton;   // Кнопка "Вихід" з камер

    [Header("Player Reference")]
    public Player playerScript; // Скрипт гравця, щоб вимикати рух

    private bool isInCameraMode = false;

    void Start()
    {
        // вимикаємо всі камери, крім першої
        for (int i = 0; i < cameras.Length; i++)
            cameras[i].gameObject.SetActive(i == 0);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitCameraMode);

        cameraUI.SetActive(false);
    }

    void Update()
    {
        if (!isInCameraMode) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            SwitchCamera(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SwitchCamera(1);
        }
    }

    public void EnterCameraMode()
    {
        if (cameras.Length == 0)
        {
            Debug.LogWarning("Немає камер у масиві!");
            return;
        }

        isInCameraMode = true;
        cameraUI.SetActive(true);

        // вимикаємо рух гравця
        if (playerScript != null)
            playerScript.canMove = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // вмикаємо першу камеру
        ActivateCamera(0);
    }

    private void SwitchCamera(int direction)
    {
        cameras[currentIndex].gameObject.SetActive(false);

        currentIndex += direction;
        if (currentIndex < 0) currentIndex = cameras.Length - 1;
        if (currentIndex >= cameras.Length) currentIndex = 0;

        ActivateCamera(currentIndex);
    }

    private void ActivateCamera(int index)
    {
        cameras[index].gameObject.SetActive(true);
        Debug.Log($"Активна камера: {cameras[index].name}");
    }

    private void ExitCameraMode()
    {
        Debug.Log("Вихід з камер");

        isInCameraMode = false;
        cameraUI.SetActive(false);

        foreach (var cam in cameras)
            cam.gameObject.SetActive(false);

        if (playerScript != null)
            playerScript.canMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
