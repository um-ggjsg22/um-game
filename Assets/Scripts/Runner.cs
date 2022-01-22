using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridPosition))]
public class Runner : MonoBehaviour
{
    public enum Direction
    {
        Dir_Up,
        Dir_Down,
        Dir_Left,
        Dir_Right,
    }

    [SerializeField]
    private RoomManager roomManager;

    private GridPosition gridPositionComponent;

    private Direction faceDirection;    // direction the Runner is facing

    // Start is called before the first frame update
    void Awake()
    {
        gridPositionComponent = GetComponent<GridPosition>();
        faceDirection = Direction.Dir_Up;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!roomManager.IsOccupied(gridPositionComponent.PosX, gridPositionComponent.PosY + 1))
            {
                gridPositionComponent.UpdateGridPosition(0, 1);
                faceDirection = Direction.Dir_Up;

                gridPositionComponent.UpdateGridPositionCoordinate();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!roomManager.IsOccupied(gridPositionComponent.PosX, gridPositionComponent.PosY - 1))
            {
                gridPositionComponent.UpdateGridPosition(0, -1);
                faceDirection = Direction.Dir_Down;

                gridPositionComponent.UpdateGridPositionCoordinate();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!roomManager.IsOccupied(gridPositionComponent.PosX - 1, gridPositionComponent.PosY))
            {
                gridPositionComponent.UpdateGridPosition(-1, 0);
                faceDirection = Direction.Dir_Left;

                gridPositionComponent.UpdateGridPositionCoordinate();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!roomManager.IsOccupied(gridPositionComponent.PosX + 1, gridPositionComponent.PosY))
            {
                gridPositionComponent.UpdateGridPosition(1, 0);
                faceDirection = Direction.Dir_Right;

                gridPositionComponent.UpdateGridPositionCoordinate();
            }
        }
    }

    public Vector2 GetGridPosition()
    {
        return new Vector2(GetComponent<GridPosition>().PosX, GetComponent<GridPosition>().PosY);
    }
}
