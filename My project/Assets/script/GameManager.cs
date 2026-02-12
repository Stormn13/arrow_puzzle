using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

// --- DATA STRUCTURES ---
public enum Direction { Up, Down, Left, Right }

[System.Serializable]
public struct ArrowData
{
    public int x;
    public int y;
    public Direction direction;
}

[System.Serializable]
public struct LevelData
{
    public int gridSize;
    public List<ArrowData> arrowLayout;
}

// --- MAIN MANAGER ---
public class GameManager : MonoBehaviour
{
    [Header("Prefabs & UI")]
    public GameObject arrowPrefab;
    public GameObject emptySlotPrefab;
    public RectTransform gridContainer;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalTimeText;
    public GameObject winPanel;

    [Header("Level Design")]
    public List<LevelData> levels;
    public int currentLevelIndex = 0;

    private float elapsedTime;
    private bool gameIsActive = false;
    private List<GameObject> activeArrows = new List<GameObject>();

    void Start()
    {
        if (gridContainer == null) 
        {
            // Instead of .transform, we specifically ask for the RectTransform component
            gridContainer = GameObject.Find("GridContainer").GetComponent<RectTransform>();
        }

        LoadLevel(currentLevelIndex);
    }

    void Update()
    {
        if (gameIsActive)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = "Time: " + Mathf.Floor(elapsedTime).ToString() + "s";
        }
    }

    public void LoadLevel(int index)
    {
        if (index >= levels.Count) return;
        currentLevelIndex = index;

        // Reset State
        gameIsActive = true;
        elapsedTime = 0;
        winPanel.SetActive(false);
        activeArrows.Clear();

        // Clear Grid
        foreach (Transform child in gridContainer) Destroy(child.gameObject);

        // Setup Layout
        LevelData data = levels[index];
        GridLayoutGroup grid = gridContainer.GetComponent<GridLayoutGroup>();
        grid.enabled = true;
        grid.constraintCount = data.gridSize;

        // Create Grid Slots
        int totalCells = data.gridSize * data.gridSize;
        GameObject[] cells = new GameObject[totalCells];
        for (int i = 0; i < totalCells; i++)
        {
            cells[i] = Instantiate(emptySlotPrefab, gridContainer);
        }

        // Spawn Custom Arrows
        foreach (ArrowData info in data.arrowLayout)
        {
            int targetIndex = (info.y * data.gridSize) + info.x;
            GameObject newArrow = Instantiate(arrowPrefab, cells[targetIndex].transform);
            activeArrows.Add(newArrow);

            // Initialize Arrow
            newArrow.GetComponent<ArrowScript>().Setup(info.direction, this);
        }
    }

    public void ArrowExited(GameObject arrow)
    {
        activeArrows.Remove(arrow);
        if (activeArrows.Count == 0)
        {
            WinLevel();
        }
    }

    void WinLevel()
    {
        gameIsActive = false;
        winPanel.SetActive(true);
        finalTimeText.text = "Finished in " + Mathf.Floor(elapsedTime) + " seconds!";
    }
}