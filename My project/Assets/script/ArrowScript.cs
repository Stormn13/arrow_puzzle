using UnityEngine;
using UnityEngine.UI;

public class ArrowScript : MonoBehaviour
{
    private Direction myDirection;
    private GameManager manager;
    private bool isMoving = false;
    private RectTransform rectTransform; 
    public float speed = 1500f; 
    
    public AudioSource clickAudio;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Setup(Direction dir, GameManager mgr)
    {
        myDirection = dir;
        manager = mgr;

        float angle = 0;
        switch(dir)
        {
            case Direction.Right: angle = 0;   break;
            case Direction.Up:    angle = 90;  break; 
            case Direction.Left:  angle = 180; break; 
            case Direction.Down:  angle = -90; break; 
        }
        
        
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnClick()
    {

        if (isMoving) return;
        
        if(clickAudio) clickAudio.Play();
        isMoving = true;
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

            
            rectTransform.anchoredPosition += moveDir * speed * Time.deltaTime;

            
            if (rectTransform.anchoredPosition.magnitude > 3000f) 
            {
                manager.ArrowExited(this.gameObject);
                Destroy(gameObject);
            }
        }
    }
}