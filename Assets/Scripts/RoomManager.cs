using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Awake()
    {
        // Create List of objects
        roomObjects = new List<RoomObject>();
    }

    public bool PlaceRoomObject(GameObject placeholderObject, RoomObject obj, int gridX, int gridY)
    {
        // Check against occupancy grid
        for (int i = gridX; i < gridX + obj.BaseWidth; ++i)
        {
            for (int j = gridY; j < gridY + obj.BaseHeight; ++j)
            {
                RoomObject checkOccupied = GridMap.Instance.OccupancyGrid(i, j);
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
        //newObject.transform.SetParent(GridMap.Instance.SpawnObjectParent.Find("GridRow" + gridY));
        GridMap.Instance.SetRowSorting(newObject.transform, gridY);

        // Copy sprite
        newObject.GetComponent<Image>().sprite = placeholderObject.GetComponent<Image>().sprite;

        // Set position
        newObject.GetComponent<RoomObject>().SetGridPosition(gridX, gridY);

        // Add to List & Occupancy Grid
        AddObjectToList(newObject.GetComponent<RoomObject>(), gridX, gridY);

        return true;
    }

    public void AddObjectToList(RoomObject obj, int gridX, int gridY)
    {
        // Add to List
        roomObjects.Add(obj);
        // Add to occupancy grid
        for (int i = gridX; i < gridX + obj.BaseWidth; ++i)
        {
            for (int j = gridY; j < gridY + obj.BaseHeight; ++j)
            {
                GridMap.Instance.UpdateOccupancyGrid(obj, i, j);
            }
        }
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
        RoomObject obj = GridMap.Instance.OccupancyGrid(gridX, gridY);
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
                    GridMap.Instance.ClearOccupancyGrid(i, j);
                }
            }

            // Destroy the object
            Destroy(obj.gameObject);
        }
    }
}
