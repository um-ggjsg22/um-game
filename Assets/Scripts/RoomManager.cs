using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private Transform spawnObjectParent;
    [SerializeField]
    private GameObject roomObjectPrefab;

    [SerializeField]
    private Image floorImage;
    public Image FloorImage
    {
        get { return floorImage; }
    }
    [SerializeField]
    private Image wallImage;
    public Image WallImage
    {
        get { return wallImage; }
    }

    /// <summary>
    /// List of objects placed down in the editor
    /// </summary>
    private List<RoomObject> roomObjects;
    public List<RoomObject> RoomObjects
    {
        get { return roomObjects; }
    }

    private RoomObject[,] occupancyGrid;

    // Start is called before the first frame update
    void Start()
    {
        // Create List of objects
        roomObjects = new List<RoomObject>();

        // Create occupancy grid
        occupancyGrid = new RoomObject[GridMap.Instance.GridLength, GridMap.Instance.GridLength];
        for (int i = 0; i < GridMap.Instance.GridLength; ++i)
        {
            for (int j = 0; j < GridMap.Instance.GridLength; ++j)
            {
                occupancyGrid[i, j] = null;
            }
        }

        // Create rows for Image sort order
        for (int y = GridMap.Instance.GridLength - 1; y >= 0; --y)
        {
            GameObject o = new GameObject("GridRow" + y);
            o.transform.parent = spawnObjectParent;
        }
    }

    public bool PlaceRoomObject(GameObject placeholderObject, RoomObject obj, int gridX, int gridY)
    {
        // Check against occupancy grid
        for (int i = gridX; i < gridX + obj.BaseWidth; ++i)
        {
            for (int j = gridY; j < gridY + obj.BaseHeight; ++j)
            {
                RoomObject checkOccupied = occupancyGrid[i, j];
                if (checkOccupied != null)
                    return false;
            }
        }       

        Debug.Log("Place down object");

        // Instantiate object
        GameObject newObject = Instantiate(roomObjectPrefab);
        newObject.SetActive(true);
        CloneRoomObject(newObject.GetComponent<RoomObject>(), obj);

        // Set parent
        newObject.transform.parent = spawnObjectParent.Find("GridRow" + gridY);

        // Copy sprite
        newObject.GetComponent<Image>().sprite = placeholderObject.GetComponent<Image>().sprite;
        // Set object size
        newObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GridMap.Instance.GridSize * obj.SpriteWidth, GridMap.Instance.GridSize * obj.SpriteHeight);

        // Set position
        newObject.GetComponent<GridPosition>().SetGridPosition(gridX, gridY);
        // Set offset position
        newObject.transform.position += new Vector3((obj.SpriteWidth - 1) * 0.5f * GridMap.Instance.GridSize, (obj.SpriteHeight - 1) * 0.5f * GridMap.Instance.GridSize);

        // Add to List
        roomObjects.Add(newObject.GetComponent<RoomObject>());
        // Add to occupancy grid
        for (int i = gridX; i < gridX + obj.BaseWidth; ++i)
        {
            for (int j = gridY; j < gridY + obj.BaseHeight; ++j)
            {
                occupancyGrid[i, j] = newObject.GetComponent<RoomObject>();
            }
        }

        return true;
    }

    private void CloneRoomObject(RoomObject target, RoomObject source)
    {
        target.ObjectName = source.ObjectName;

        target.BaseWidth = source.BaseWidth;
        target.BaseHeight = source.BaseHeight;
        target.SpriteWidth = source.SpriteWidth;
        target.SpriteHeight = source.SpriteHeight;
    }

    public void RemoveRoomObject(int gridX, int gridY)
    {
        Debug.Log("GridX: " + gridX + " GridY: " + gridY);
        // Check if there is an object at X,Y
        RoomObject obj = occupancyGrid[gridX, gridY];
        if (obj != null)
        {
            // Remove from List
            roomObjects.Remove(obj);

            // Remove from occupancy grid
            Vector2 pos = obj.GetGridPosition();
            int startX = (int)pos.x;
            int startY = (int)pos.y;
            for (int i = startX; i < startX + obj.BaseWidth; ++i)
            {
                for (int j = startY; j < startY + obj.BaseHeight; ++j)
                {
                    occupancyGrid[i, j] = null;
                }
            }

            // Destroy the object
            Destroy(obj.gameObject);
        }
    }
}
