using UnityEngine;

public class Laptop : MonoBehaviour, IInteractable
{

    [Header("Laptop Models")]
    public GameObject laptopOff; // модель викл
    public GameObject laptopOn;  // модель вкл

    [Header("Laptop UI")]
    public GameObject laptopUI;  // Canvas з UI
    public Player playerScript;  // твій Player.cs

    [Header("Моделі ноутбука")]
    public GameObject laptopOffModel;   // Laptop_black_off
    public GameObject laptopOnModel;    // Laptop_black
    public CameraManager cameraManager;


    private bool isOn = false;
    private bool isTurningOn = false;
    private bool isUIOpen = false;

    public void Interact()
    {

        if (!isOn && !isTurningOn)
        {
            StartCoroutine(TurnOnLaptop());
        }
        else if (isOn)
        {
            ToggleUI();
        }
    }

    private System.Collections.IEnumerator TurnOnLaptop()
    {
        isTurningOn = true;
        Debug.Log("Увімкнення ноутбука... 5 секунд");

        yield return new WaitForSeconds(5f);

        laptopOff.SetActive(false);
        laptopOn.SetActive(true);

        isOn = true;
        isTurningOn = false;

        Debug.Log("Ноутбук тепер: УВІМКНЕНИЙ");
    }

    private void ToggleUI()
    {
        isUIOpen = !isUIOpen;
        laptopUI.SetActive(isUIOpen);

        if (isUIOpen)
        {
            playerScript.canMove = false; // ❌ рух вимкнено
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            playerScript.canMove = true; // ✅ рух знову увімкнено
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Debug.Log(isUIOpen ? "Відкрив UI ноутбука" : "Закрив UI ноутбука");

        if (!isOn)
        {
            TurnOn();
        }
        else
        {
            Debug.Log("Ноут увімкнений – користуйся камерами");
        }
    }

    void Update()
    {
        if (!isOn) return;

        // ЛКМ → переключення на наступну камеру
        if (Input.GetMouseButtonDown(0))
        {
            cameraManager.NextCamera();
        }

        // Escape → вихід (вимикаємо камери й повертаємось до гравця)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cameraManager.ExitCameras();
        }
    }

    void TurnOn()
    {
        if (laptopOffModel != null) laptopOffModel.SetActive(false);
        if (laptopOnModel != null) laptopOnModel.SetActive(true);

        isOn = true;

        // одразу запускаємо камери з 1-ї
        cameraManager.StartCameras();

        Debug.Log("Ноутбук увімкнено (Laptop_black)");

    }
}