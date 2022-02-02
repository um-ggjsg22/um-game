using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RoomObject : MonoBehaviour, IHunterInteractable
{
    [SerializeField]
    private string objectName;
    public string ObjectName
    {
        get { return objectName; }
        set { objectName = value; }
    }

    [SerializeField]
    private int spriteWidth;
    public int SpriteWidth
    {
        get { return spriteWidth; }
        set { spriteWidth = value; }
    }
    [SerializeField]
    private int spriteHeight;
    public int SpriteHeight
    {
        get { return spriteHeight; }
        set { spriteHeight = value; }
    }

    [SerializeField]
    private int baseWidth;
    public int BaseWidth
    {
        get { return baseWidth; }
        set { baseWidth = value; }
    }
    [SerializeField]
    private int baseHeight;
    public int BaseHeight
    {
        get { return baseHeight; }
        set { baseHeight = value; }
    }

    protected GridPosition gridPosition;

    // Start is called before the first frame update
    protected void Start()
    {
        SetObjectSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetObjectSize()
    {
        // Set object size
        GetComponent<RectTransform>().sizeDelta = new Vector2(GridMap.Instance.GridSize * spriteWidth, GridMap.Instance.GridSize * spriteHeight);
    }

    public Vector2 GetGridPosition()
    {
        return new Vector2(gridPosition.PosX, gridPosition.PosY);
    }

    public virtual float DragPenalty() => 1;



    public bool SetGridPosition(int x, int y, bool clear = true)
    {
        // Check if out of bounds
        if (x < 0 || x >= GridMap.Instance.GridLength || y < 0 || y >= GridMap.Instance.GridLength)
            return false;

        // Clear occupancy grid - take note of size
        if (clear)
        {
            for (int i = gridPosition.PosX; i < gridPosition.PosX + baseWidth; ++i)
            {
                for (int j = gridPosition.PosY; j < gridPosition.PosY + baseHeight; ++j)
                {
                    GridMap.Instance.ClearOccupancyGrid(i, j);
                }
            }
        }

        // Update grid position
        gridPosition.PosX = x;
        gridPosition.PosY = y;

        // Update occupancy grid - take note of size
        for (int i = gridPosition.PosX; i < gridPosition.PosX + baseWidth; ++i)
        {
            for (int j = gridPosition.PosY; j < gridPosition.PosY + baseHeight; ++j)
            {
                GridMap.Instance.UpdateOccupancyGrid(this, i, j);
            }
        }

        // if Y changed, update row parent for sprite sorting
        GridMap.Instance.SetRowSorting(transform, gridPosition.PosY);

        UpdateGridPositionCoordinate();

        return true;
    }

    public void UpdateGridPositionCoordinate()
    {
        // set the new GameObject position
        transform.position = GridMap.Instance.GetPositionCoordinate(gridPosition.PosX, gridPosition.PosY);
        // Set offset position
        transform.position += new Vector3((spriteWidth - 1) * 0.5f * GridMap.Instance.GridSize, (spriteHeight - 1) * 0.5f * GridMap.Instance.GridSize);
    }
}
