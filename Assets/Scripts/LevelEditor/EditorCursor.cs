using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorCursor : MonoBehaviour
{
    [SerializeField]
    private GridMap gridMap;
    [SerializeField]
    private Image highlightedGrid;
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
            if (isObjectSelected)
                placeholderObject.gameObject.SetActive(value);

            highlightedGrid.gameObject.SetActive(value);
        }
    }

    private bool isObjectSelected = false;
    public bool IsObjectSelected
    {
        get { return isObjectSelected; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // resize highlightedGrid according to grid size
        highlightedGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(gridMap.GridSize, gridMap.GridSize);
        // Disable first
        highlightedGrid.gameObject.SetActive(false);

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

            if (gridX != -1)    // valid coordinate
            {
                // Get actual position of grid coordinate
                Vector3 gridPos = gridMap.GetPositionCoordinate(gridX, gridY);

                if (isObjectSelected)
                    placeholderObject.transform.position = gridPos;

                highlightedGrid.transform.position = gridPos;
            }
        }
    }

    public void DeselectObject()
    {
        isObjectSelected = false;
        placeholderObject.gameObject.SetActive(false);
    }

    public void SelectObject(RoomObject obj)
    {
        isObjectSelected = true;

        // Set object sprite
        placeholderObject.sprite = obj.GetComponent<Image>().sprite;

        // Set object size
        placeholderObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gridMap.GridSize * obj.SpriteWidth, gridMap.GridSize * obj.SpriteHeight);

        // Set offset position
        placeholderObject.transform.localPosition = new Vector3((obj.SpriteWidth - 1) * gridMap.GridSize, (obj.SpriteHeight - 1) * gridMap.GridSize);
    }
}
