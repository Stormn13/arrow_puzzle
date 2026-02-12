using UnityEngine;
using UnityEngine.UI;

public class ArrowScript : MonoBehaviour
{
    private Direction myDirection;
    private GameManager manager;
    private bool isMoving = false;
    
    // UI Elements use RectTransform instead of Transform
    private RectTransform rectTransform; 

    // Make sure Speed is high enough for pixels (try 1500 if 1000 is too slow)
    public float speed = 1500f; 
    
    public AudioSource clickAudio;

    void Awake()
    {
        // Grab the RectTransform component immediately so we can use it later
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(Direction dir, GameManager mgr)
    {
        myDirection = dir;
        manager = mgr;

        float angle = 0;
        switch(dir)
        {
            case Direction.Right: angle = 0;   break; // No rotation needed
            case Direction.Up:    angle = 90;  break; // Turn 90 degrees Left
            case Direction.Left:  angle = 180; break; // Flip it
            case Direction.Down:  angle = -90; break; // Turn 90 degrees Right
        }
        
        // Apply the rotation to the Z axis (which is how 2D rotates)
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnClick()
    {
        Debug.Log("🖱️ CLICKED! Checking path...");

        if (isMoving) return;

        // --- 🛑 FIXED COLLISION CHECK 🛑 ---
    
        // 1. Identify "My Slot" (The Parent) and "The Grid" (The Grandparent)
        Transform mySlot = transform.parent;
        Transform gridContainer = mySlot.parent; 

        // 2. What is my Slot's number? (0 to 8)
        int currentIndex = mySlot.GetSiblingIndex();
    
        // 3. Grid Math
        int gridSize = 20; // Keep this matching your Inspector!
        int myRow = currentIndex / gridSize;
        int myCol = currentIndex % gridSize;

        int targetRow = myRow;
        int targetCol = myCol;

        // 4. Determine Target
        switch (myDirection)
        {
            case Direction.Up:    targetRow--; break;
            case Direction.Down:  targetRow++; break;
            case Direction.Left:  targetCol--; break;
            case Direction.Right: targetCol++; break;
        }

        // 5. Are we flying OUT of the grid? (Allowed!)
        bool isFlyingOut = (targetRow < 0 || targetRow >= gridSize || targetCol < 0 || targetCol >= gridSize);

        if (!isFlyingOut)
        {
            // 6. We are staying inside. Find the TARGET SLOT.
            int targetIndex = (targetRow * gridSize) + targetCol;
        
            // Ask the GRID (Grandparent) for the target slot
            Transform targetSlot = gridContainer.GetChild(targetIndex);

            // 7. Does the target slot have an Arrow inside it?
            // We look for "ArrowScript" in the children of that slot
            if (targetSlot.GetComponentInChildren<ArrowScript>() != null)
            {
                Debug.Log("⛔ BLOCKED! An arrow is in the way.");
                return; // STOP! Do not move.
            }
        }
        // --- 🛑 END CHECK 🛑 ---


        if(clickAudio) clickAudio.Play();
        isMoving = true;

        // Escape the slot and move to the Canvas
        transform.SetParent(manager.gridContainer.root); 
        transform.SetAsLastSibling();
    }

    void Update()
    {
        if (isMoving)
        {
            // Use Vector2 for UI movement
            Vector2 moveDir = Vector2.zero;

            switch (myDirection)
            {
                case Direction.Up: moveDir = Vector2.up; break;
                case Direction.Down: moveDir = Vector2.down; break;
                case Direction.Left: moveDir = Vector2.left; break;
                case Direction.Right: moveDir = Vector2.right; break;
            }

            // MOVE USING ANCHORED POSITION (The UI Way)
            rectTransform.anchoredPosition += moveDir * speed * Time.deltaTime;

            // Boundary Check (using Canvas scaler logic approximately)
            // If it goes too far (e.g. > 2000 pixels), kill it
            if (rectTransform.anchoredPosition.magnitude > 3000f) 
            {
                manager.ArrowExited(this.gameObject);
                Destroy(gameObject);
            }
        }
    }
}