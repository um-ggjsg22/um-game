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
    void Start()
    {
        // Set object size
        GetComponent<RectTransform>().sizeDelta = new Vector2(GridMap.Instance.GridSize * spriteWidth, GridMap.Instance.GridSize * spriteHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetGridPosition()
    {
        return new Vector2(gridPosition.PosX, gridPosition.PosY);
    }

    public float DragPenalty() => 1;



    public bool SetGridPosition(int x, int y)
    {
        if (x < 0 || x >= GridMap.Instance.GridLength || y < 0 || y >= GridMap.Instance.GridLength)
            return false;

        gridPosition.PosX = x;
        gridPosition.PosY = y;

        UpdateGridPositionCoordinate();

        return true;
    }

    public bool UpdateGridPosition(int xDelta, int yDelta)
    {
        int prevPosX = gridPosition.PosX, prevPosY = gridPosition.PosY;

        gridPosition.PosX = Mathf.Clamp(gridPosition.PosX + xDelta, 0, GridMap.Instance.GridLength - 1);
        gridPosition.PosY = Mathf.Clamp(gridPosition.PosY + yDelta, 0, GridMap.Instance.GridLength - 1);

        // check whether it got updated
        if (prevPosX != gridPosition.PosX || prevPosY != gridPosition.PosY)
        {
            // Update occupancy grid - take note of size
            GridMap.Instance.ClearOccupancyGrid(prevPosX, prevPosY);
            //GridMap.Instance.UpdateOccupancyGrid();

            return true;
        }

        return false;
    }

    public void UpdateGridPositionCoordinate()
    {
        // set the new GameObject position
        transform.position = GridMap.Instance.GetPositionCoordinate(gridPosition.PosX, gridPosition.PosY);
        // Set offset position
        transform.position += new Vector3((spriteWidth - 1) * 0.5f * GridMap.Instance.GridSize, (spriteHeight - 1) * 0.5f * GridMap.Instance.GridSize);
    }
}
