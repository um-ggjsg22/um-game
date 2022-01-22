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

    // the current grid coordinate the cursor is on
    private int gridX;
    private int gridY;

    private bool onGridArea = false;
    public bool OnGridArea
    {
        get { return onGridArea; }
    }

    public void SetOnGridArea(bool onGrid)
    {
        onGridArea = onGrid;
        highlightedGrid.gameObject.SetActive(onGrid);
    }

    // Start is called before the first frame update
    void Start()
    {
        // resize highlightedGrid according to grid size
        highlightedGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(gridMap.GridSize, gridMap.GridSize);
        // Disable first
        highlightedGrid.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if cursor is on Floor
        if (onGridArea)
        {
            // Check which grid the cursor is on
            Vector2 mousePos = Input.mousePosition;
            Vector2 gridCoord = gridMap.GetGridCoordinate(mousePos);

            gridX = (int)gridCoord.x;
            gridY = (int)gridCoord.y;

            // Get actual position of grid coordinate
            highlightedGrid.transform.position = gridMap.GetPositionCoordinate(gridX, gridY);
        }
    }
}
