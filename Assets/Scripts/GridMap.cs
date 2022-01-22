using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    private static GridMap instance = null;
    public static GridMap Instance { get { return instance; } }

    [SerializeField]
    [Tooltip("Reference to Runner object")]
    private Runner runner;
    [SerializeField]
    private int startGridX = 2;
    [SerializeField]
    private int startGridY = 0;

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

        // Calculate info about the grids
        gridSize = floor.GetComponent<RectTransform>().sizeDelta.x / gridLength;
        startPos = new Vector3(floor.transform.position.x - gridLength * 0.5f * gridSize + 0.5f * gridSize, floor.transform.position.y - gridLength * 0.5f * gridSize + 0.5f * gridSize);

        // Resize Runner according to the computed grid size
        runner.GetComponent<RectTransform>().sizeDelta = new Vector2(gridSize, gridSize);
        // Set Runner's start position
        //runner.transform.position = GetPositionCoordinate(startGridX, startGridY);
        runner.GetComponent<GridPosition>().SetGridPosition(startGridX, startGridY);

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
}
