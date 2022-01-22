using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorCursor : MonoBehaviour
{
    [SerializeField]
    private GridMap gridMap;
    [SerializeField]
    private RoomManager roomManager;
    [SerializeField]
    private Image highlightedGrid;
    [SerializeField]
    private Image gridNotAllowedIndicator;
    [SerializeField]
    private Image placeholderObject;

    // the current grid coordinate the cursor is on
    private int gridX;
    private int gridY;

    private bool onGridArea = false;
    public bool OnGridArea
    {
        get { return onGridArea; }
        set
        {
            onGridArea = value;
            if (IsObjectSelected)
                placeholderObject.gameObject.SetActive(value);

            highlightedGrid.gameObject.SetActive(value);
        }
    }

    public bool IsObjectSelected
    {
        get { return selectedObject != null; }
    }

    private RoomObject selectedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        // resize highlightedGrid according to grid size
        highlightedGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(gridMap.GridSize, gridMap.GridSize);
        gridNotAllowedIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2(gridMap.GridSize, gridMap.GridSize);
        // Disable first
        highlightedGrid.gameObject.SetActive(false);
        gridNotAllowedIndicator.gameObject.SetActive(false);

        // Disable placeholderObject first
        placeholderObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if cursor is on Floor
        if (onGridArea)
        {
            // Check which grid the cursor is on
            Vector2 mousePos = Input.mousePosition;
            Vector2 gridCoord = gridMap.GetGridCoordinate(mousePos) ?? new Vector2(-1, -1);

            gridX = (int)gridCoord.x;
            gridY = (int)gridCoord.y;

            if (gridX != -1)    // cursor is within the grid space
            {
                // Get actual position of grid coordinate
                Vector3 gridPos = gridMap.GetPositionCoordinate(gridX, gridY);

                // Update cursor position
                transform.position = gridPos;

                // If object is selected,
                if (IsObjectSelected)
                {
                    // Check if object placement is valid
                    if (gridX + selectedObject.BaseWidth - 1 >= gridMap.GridLength || gridY + selectedObject.BaseHeight - 1 >= gridMap.GridLength)  // invalid
                        gridNotAllowedIndicator.gameObject.SetActive(true);
                    else
                    {
                        gridNotAllowedIndicator.gameObject.SetActive(false);

                        // Check for click
                        if (Input.GetMouseButtonDown(0))
                        {
                            // Place down object
                            roomManager.PlaceRoomObject(placeholderObject.gameObject, selectedObject, gridX, gridY);
                        }
                    }
                }

                // Check for delete object
                if (Input.GetMouseButton(1))
                {
                    Debug.Log("RMB click");
                    // Check against occupancy grid for an object
                    roomManager.RemoveRoomObject(gridX, gridY);
                }
            }
        }
    }

    public void DeselectObject()
    {
        selectedObject = null;
        placeholderObject.gameObject.SetActive(false);
    }

    public void SelectObject(RoomObject obj)
    {
        selectedObject = obj;

        // Set object sprite
        placeholderObject.sprite = obj.GetComponent<Image>().sprite;

        // Set object size
        placeholderObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gridMap.GridSize * obj.SpriteWidth, gridMap.GridSize * obj.SpriteHeight);

        // Set offset position
        placeholderObject.transform.localPosition = new Vector3((obj.SpriteWidth - 1) * 0.5f * gridMap.GridSize, (obj.SpriteHeight - 1) * 0.5f * gridMap.GridSize);
    }
}
