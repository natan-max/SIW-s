using UnityEngine;
using TMPro;


public class Tile : MonoBehaviour
{
    [HideInInspector]
    public int x, y;

    [HideInInspector]
    public bool isMine = false;

    [HideInInspector]
    public bool isRevealed = false;

    [HideInInspector]
    public bool isFlagged = false;

    [HideInInspector]
    public int adjacentMines = 0;

    public Sprite closedSprite;
    public Sprite openSprite;
    public Sprite mineSprite;
    public Sprite flagSprite;
    public TextMeshProUGUI labelText; // Текст цифри або "F" для прапорця

    private SpriteRenderer sr;
    private GameManager gm;

    // Ініціалізація клітинки
    public void Init(int _x, int _y, GameManager manager)
    {
        x = _x;
        y = _y;
        gm = manager;
        sr = GetComponent<SpriteRenderer>();
        ResetTile();
    }

    void ResetTile()
    {
        isMine = false;
        isRevealed = false;
        isFlagged = false;
        adjacentMines = 0;
        if (sr != null && closedSprite != null) sr.sprite = closedSprite;
        if (labelText != null) labelText.text = "";
    }

    // Лівий клік
    void OnMouseUpAsButton()
    {
        if (!isFlagged)
            gm.RevealTile(x, y);
    }

    // Правий клік
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
            gm.ToggleFlag(x, y);
    }

    // Метод для відкриття клітинки
    public void Reveal(bool showMine)
    {
        isRevealed = true;

        if (!isMine)
        {
            if (sr != null && openSprite != null)
                sr.sprite = openSprite;

            if (adjacentMines > 0 && labelText != null)
            {
                labelText.text = adjacentMines.ToString();

                // кольорові цифри як у класичному Minesweeper
                switch (adjacentMines)
                {
                    case 1: labelText.color = Color.blue; break;
                    case 2: labelText.color = Color.green; break;
                    case 3: labelText.color = Color.red; break;
                    case 4: labelText.color = Color.magenta; break;
                    case 5: labelText.color = new Color(0.5f,0,0); break; // темно-червоний
                    case 6: labelText.color = Color.cyan; break;
                    case 7: labelText.color = Color.black; break;
                    case 8: labelText.color = Color.gray; break;
                }
            }
        }
        else
        {
            if (sr != null && openSprite != null) sr.sprite = openSprite;
            if (labelText != null && adjacentMines > 0) labelText.text = adjacentMines.ToString();
        }
    }

    // Метод для установки або зняття прапорця
    public void ToggleFlag()
    {
        if (isRevealed) return;

        isFlagged = !isFlagged;

        if (isFlagged)
        {
            if (sr != null && flagSprite != null) sr.sprite = flagSprite;
            if (labelText != null) labelText.text = "F";
        }
        else
        {
            if (sr != null && closedSprite != null) sr.sprite = closedSprite;
            if (labelText != null) labelText.text = "";
        }
    }
}
