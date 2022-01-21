using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    private static GridMap instance = null;
    public static GridMap Instance { get { return instance; } }

    [SerializeField]
    private int gridLength = 13;
    public int GridLength
    {
        get { return gridLength; }
    }

    [SerializeField]
    private GameObject floor;

    private float gridSize; // in pixels
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPositionCoordinate(int x, int y)
    {
        return startPos + new Vector3(x * gridSize, y * gridSize);
    }
}
