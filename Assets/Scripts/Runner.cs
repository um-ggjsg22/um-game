using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridPosition))]
public class Runner : MonoBehaviour
{
    private GridPosition gridPositionComponent;

    // Start is called before the first frame update
    void Awake()
    {
        gridPositionComponent = GetComponent<GridPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gridPositionComponent.UpdateGridPosition(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gridPositionComponent.UpdateGridPosition(0, -1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gridPositionComponent.UpdateGridPosition(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gridPositionComponent.UpdateGridPosition(1, 0);
        }

        gridPositionComponent.UpdateGridPositionCoordinate();
    }

    public Vector2 GetGridPosition()
    {
        return new Vector2(GetComponent<GridPosition>().PosX, GetComponent<GridPosition>().PosY);
    }
}
