using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    private static GridMap instance = null;
    public static GridMap Instance { get { return instance; } }

    [SerializeField]
    private Transform spawnObjectParent;
    public Transform SpawnObjectParent
    {
        get { return spawnObjectParent; }
    }

    [SerializeField]
    [Tooltip("Reference to Runner object")]
    private Runner runner;

    [SerializeField]
    private int gridLength = 13;
    public int GridLength
    {
        get { return gridLength; }
    }

    [SerializeField]
    private GameObject floor;

    private float gridSize; // in pixels
    public float GridSize
    {
        get { return gridSize; }
    }
    private Vector3 startPos;   // grid 0,0

    // Floor bounding box
    private Vector3 floorMinPos;
    private Vector3 floorMaxPos;

    // Occupancy Grid
    private RoomObject[,] occupancyGrid;
    public RoomObject OccupancyGrid(int x, int y)
    {
        return occupancyGrid[x, y];
    }
    public void UpdateOccupancyGrid(RoomObject obj, int x, int y)
    {
        // set to new grid
        occupancyGrid[x, y] = obj;
    }

    public void ClearOccupancyGrid(int x, int y)
    {
        occupancyGrid[x, y] = null;
    }

    public bool IsOccupied(RoomObject obj, int gridX, int gridY)
    {
        if (gridX < 0 || gridX >= GridLength || gridY < 0 || gridY >= GridLength)
            return true;

        return !((occupancyGrid[gridX, gridY] == null) || (occupancyGrid[gridX, gridY] == obj));
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.Log("GridMap already has an existing instance! Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        // Create occupancy grid
        occupancyGrid = new RoomObject[GridLength, GridLength];
        for (int i = 0; i < GridLength; ++i)
        {
            for (int j = 0; j < GridLength; ++j)
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

        // Calculate info about the grids
        gridSize = floor.GetComponent<RectTransform>().sizeDelta.x / gridLength;
        startPos = new Vector3(floor.transform.position.x - gridLength * 0.5f * gridSize + 0.5f * gridSize, floor.transform.position.y - gridLength * 0.5f * gridSize + 0.5f * gridSize);

        // Resize Runner according to the computed grid size
        if (runner != null)
        {
            runner.GetComponent<RectTransform>().sizeDelta = new Vector2(gridSize, gridSize);
        }

        floorMinPos = floor.transform.position - 0.5f * (Vector3)floor.GetComponent<RectTransform>().sizeDelta;
        floorMaxPos = floor.transform.position + 0.5f * (Vector3)floor.GetComponent<RectTransform>().sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 FloorMinCoordinate()
    {
        return floorMinPos;
    }
    public Vector3 FloorMaxCoordinate()
    {
        return floorMaxPos;
    }

    public Vector2? GetGridCoordinate(Vector3 pos)
    {
        int gridX = Mathf.FloorToInt((pos.x - floor.transform.position.x + gridLength * 0.5f * (gridSize - 1)) / gridSize);
        int gridY = Mathf.FloorToInt((pos.y - floor.transform.position.y + gridLength * 0.5f * (gridSize - 1)) / gridSize);

        if (gridX < 0 || gridX >= gridLength || gridY < 0 || gridY >= gridLength)
            return null;

        return new Vector2(gridX, gridY);
    }

    /// <summary>
    /// Get the screen coordinate of the center of a grid coordinate.
    /// </summary>
    /// <param name="x">Grid coordinate X</param>
    /// <param name="y">Grid coordinate Y</param>
    /// <returns></returns>
    public Vector3 GetPositionCoordinate(int x, int y)
    {
        return startPos + new Vector3(x * gridSize, y * gridSize);
    }

    public Vector2 GetRunnerPosition()
    {
        return runner.GetGridPosition();
    }

    public void SetRowSorting(Transform obj, int gridY)
    {
        obj.SetParent(SpawnObjectParent.Find("GridRow" + gridY));
    }
}
