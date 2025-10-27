using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject tilePrefab;   // твій TilePrefab
    public int width = 10;
    public int height = 10;
    public int mineCount = 15;
    public float spacing = 1f;

    [Header("UI")]
    public Text statusText;         // UI Text для статусу ("You win!" / "You lost!")
    public Text minesLeftText;      // Лічильник мін

    private Tile[,] tiles;
    private bool firstClick = true;
    private bool gameOver = false;
    private int flagsPlaced = 0;
    private int cellsToReveal = 0;

    void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        // очистити старі клітинки
        foreach (Transform t in transform)
            Destroy(t.gameObject);

        firstClick = true;
        gameOver = false;
        flagsPlaced = 0;

        tiles = new Tile[width, height];

        // створення поля
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * spacing, -y * spacing, 0);
                GameObject obj = Instantiate(tilePrefab, pos, Quaternion.identity, transform);

                Tile tile = obj.GetComponent<Tile>();
                tile.Init(x, y, this);
                tiles[x, y] = tile;
            }
        }

        cellsToReveal = width * height - mineCount;

        if (statusText != null)
            statusText.text = "Ready";

        if (minesLeftText != null)
            minesLeftText.text = $"Mines: {mineCount}";

// Центруємо камеру на середину поля
        if (Camera.main != null)
        {
            float camX = (width - 1) * spacing / 2f;
            float camY = -(height - 1) * spacing / 2f;
            Camera.main.transform.position = new Vector3(camX, camY, -10f);

            // Опціонально: масштаб камери так, щоб поле помістилося
            Camera.main.orthographic = true;
            Camera.main.orthographicSize = Mathf.Max(width, height) / 2f + 1f;
        }

    }

    // Ставимо міни після першого кліку
    void PlaceMines(int safeX, int safeY)
    {
        List<(int, int)> all = new List<(int, int)>();

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (!(x == safeX && y == safeY))
                    all.Add((x, y));

        for (int i = 0; i < mineCount; i++)
        {
            int index = Random.Range(0, all.Count);
            var p = all[index];
            all.RemoveAt(index);
            tiles[p.Item1, p.Item2].isMine = true;
        }

        // підрахунок мін поруч
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                tiles[x, y].adjacentMines = CountAdjacentMines(x, y);
    }

    int CountAdjacentMines(int x, int y)
    {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (tiles[nx, ny].isMine)
                        count++;
                }
            }
        }
        return count;
    }

    public void RevealTile(int x, int y)
    {
        if (gameOver) return;

        Tile t = tiles[x, y];

        if (firstClick)
        {
            PlaceMines(x, y);
            firstClick = false;
        }

        if (t.isFlagged || t.isRevealed) return;

        if (t.isMine)
        {
            // Програш
            t.Reveal(true);
            GameOver(false);
            return;
        }

        FloodReveal(x, y);
        CheckWin();
    }

    void FloodReveal(int sx, int sy)
    {
        Stack<Tile> stack = new Stack<Tile>();
        stack.Push(tiles[sx, sy]);

        while (stack.Count > 0)
        {
            Tile current = stack.Pop();

            if (current.isRevealed || current.isFlagged) continue;

            current.Reveal(false);
            cellsToReveal--;

            if (current.adjacentMines == 0)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = current.x + dx;
                        int ny = current.y + dy;

                        if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                        {
                            Tile neighbor = tiles[nx, ny];
                            if (!neighbor.isRevealed && !neighbor.isMine)
                                stack.Push(neighbor);
                        }
                    }
                }
            }
        }
    }

    public void ToggleFlag(int x, int y)
    {
        if (gameOver) return;

        Tile t = tiles[x, y];
        if (t.isRevealed) return;

        t.ToggleFlag();
        flagsPlaced += t.isFlagged ? 1 : -1;

        if (minesLeftText != null)
            minesLeftText.text = $"Mines: {mineCount - flagsPlaced}";
    }

    void GameOver(bool win)
    {
        gameOver = true;
        if (win)
        {
            if (statusText != null)
                statusText.text = "🎉 You Win!";
        }
        else
        {
            if (statusText != null)
                statusText.text = "💥 You Lost!";
            // показати всі міни
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (tiles[x, y].isMine)
                        tiles[x, y].Reveal(true);
        }
    }

    void CheckWin()
    {
        if (cellsToReveal <= 0)
        {
            GameOver(true);
        }
    }
}
