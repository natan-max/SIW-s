using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false); // ховаємо панель спочатку
    }

    void Update()
    {
        // Виклик меню на клавішу `
        if (Input.GetKeyDown(KeyCode.BackQuote)) // це "`"
        {
            ShowGameOver();
        }
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // стоп гри
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnRestartButton()
    {
        Time.timeScale = 1f; // повертаємо час
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // перезапускаємо сцену
    }

    public void OnQuitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // щоб працювало в редакторі
#endif
    }
}
