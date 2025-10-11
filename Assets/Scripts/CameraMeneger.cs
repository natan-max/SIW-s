using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera playerCamera;       // основна камера гравця
    public Camera[] securityCameras;  // камери ноутбука

    private int currentCamIndex = -1;

    void Start()
    {
        // Спочатку вимикаємо всі камери ноутбука
        foreach (Camera cam in securityCameras)
            cam.gameObject.SetActive(false);

        playerCamera.gameObject.SetActive(true);
    }

    public void StartCameras()
    {
        // Вимикаємо камеру гравця і вмикаємо першу
        playerCamera.gameObject.SetActive(false);

        currentCamIndex = 0;
        securityCameras[currentCamIndex].gameObject.SetActive(true);
    }

    public void NextCamera()
    {
        if (currentCamIndex == -1) return;

        // Вимикаємо поточну
        securityCameras[currentCamIndex].gameObject.SetActive(false);

        // Наступна
        currentCamIndex++;
        if (currentCamIndex >= securityCameras.Length)
            currentCamIndex = 0;

        // Вмикаємо наступну
        securityCameras[currentCamIndex].gameObject.SetActive(true);
    }

    public void ExitCameras()
    {
        if (currentCamIndex != -1)
        {
            securityCameras[currentCamIndex].gameObject.SetActive(false);
            currentCamIndex = -1;
        }

        playerCamera.gameObject.SetActive(true);
    }
}