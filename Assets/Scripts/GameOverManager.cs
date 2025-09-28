using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject LOXCanvas;        
    public TMP_Text gameOverText;       
    public GameObject buttonsPanel;     
    public AudioSource loseSound;       
    public float textSpeed = 0.05f;    

    [Header("Player")]
    public Transform player;            
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool isGameOver = false;

    void Start()
    {
        LOXCanvas.SetActive(false);
        buttonsPanel.SetActive(false);
        initialPosition = player.position;
        initialRotation = player.rotation;
    }

    void Update()
    {
        // Натискання клавіш для тесту
        if (Input.GetKeyDown(KeyCode.O) && !isGameOver)
            TriggerGameOver();

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isGameOver)
            TriggerGameOver();
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;          // Зупиняємо гру
        LOXCanvas.SetActive(true);    // Включаємо канвас

        if (loseSound != null)
            loseSound.Play();

        StartCoroutine(TypeText("ТИ ПРОГРАВ!"));
    }

    IEnumerator TypeText(string text)
    {
        gameOverText.text = "";
        foreach (char c in text)
        {
            gameOverText.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        buttonsPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        player.position = initialPosition;
        player.rotation = initialRotation;
        LOXCanvas.SetActive(false);
        buttonsPanel.SetActive(false);
        isGameOver = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}