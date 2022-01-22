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

    [SerializeField]
    private Transform startTest;

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

        // Spawn smth at start pos
        startTest.position = startPos;

        // TODO: get all "grid objects", resize everything according to the computed grid size

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2? GetGridCoordinate(Vector3 pos)
    {
        int gridX = Mathf.FloorToInt((pos.x - floor.transform.position.x + gridLength * 0.5f * (gridSize - 1)) / gridSize);
        int gridY = Mathf.FloorToInt((pos.y - floor.transform.position.y + gridLength * 0.5f * (gridSize - 1)) / gridSize);

        if (gridX < 0 || gridX >= gridLength || gridY < 0 || gridY >= gridLength)
            return null;

        return new Vector2(gridX, gridY);
    }

    public Vector3 GetPositionCoordinate(int x, int y)
    {
        return startPos + new Vector3(x * gridSize, y * gridSize);
    }

    public Vector2 GetRunnerPosition()
    {
        return runner.GetGridPosition();
    }
}
