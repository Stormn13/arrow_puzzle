using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public enum Direction { Up, Down, Left, Right }


public struct ArrowData
{
    public int x;
    public int y;
    public Direction direction;
}


public struct LevelData
{
    public int gridSize;
    public List<ArrowData> arrowLayout;
}

public class GameManager : MonoBehaviour
{
    
    public GameObject arrowPrefab;
    public GameObject emptySlotPrefab;
    public RectTransform gridContainer;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalTimeText;
    public GameObject winPanel;

    
    public List<LevelData> levels;
    public int currentLevelIndex = 0;

    private float elapsedTime;
    private bool gameIsActive = false;
    private List<GameObject> activeArrows = new List<GameObject>();

    void Start()
    {
        if (gridContainer == null) 
        {
            
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

        
        gameIsActive = true;
        elapsedTime = 0;
        winPanel.SetActive(false);
        activeArrows.Clear();

        
        foreach (Transform child in gridContainer) Destroy(child.gameObject);

        
        LevelData data = levels[index];
        GridLayoutGroup grid = gridContainer.GetComponent<GridLayoutGroup>();
        grid.enabled = true;
        grid.constraintCount = data.gridSize;

        
        int totalCells = data.gridSize * data.gridSize;
        GameObject[] cells = new GameObject[totalCells];
        for (int i = 0; i < totalCells; i++)
        {
            cells[i] = Instantiate(emptySlotPrefab, gridContainer);
        }

        
        foreach (ArrowData info in data.arrowLayout)
        {
            int targetIndex = (info.y * data.gridSize) + info.x;
            GameObject newArrow = Instantiate(arrowPrefab, cells[targetIndex].transform);
            activeArrows.Add(newArrow);

            
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