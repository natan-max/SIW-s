using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        gameOverPanel.SetActive(false); // спочатку прихований
    }

    // Викликаєш цей метод, коли гравець програв
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // ставимо паузу
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Кнопка "Вернутися" (рестарт рівня)
    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Кнопка "Вийти"
    public void OnQuitButton()
    {
        Application.Quit();
        // Для редактора (щоб теж працювало)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
