using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : RoomObject
{
    public enum Direction
    {
        Dir_Up,
        Dir_Down,
        Dir_Left,
        Dir_Right,
    }

    private Direction faceDirection;    // direction the Runner is facing

    // Start is called before the first frame update
    void Awake()
    {
        faceDirection = Direction.Dir_Up;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!GridMap.Instance.IsOccupied(gridPosition.PosX, gridPosition.PosY + 1))
            {
                UpdateGridPosition(0, 1);
                faceDirection = Direction.Dir_Up;

                UpdateGridPositionCoordinate();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!GridMap.Instance.IsOccupied(gridPosition.PosX, gridPosition.PosY - 1))
            {
                UpdateGridPosition(0, -1);
                faceDirection = Direction.Dir_Down;

                UpdateGridPositionCoordinate();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!GridMap.Instance.IsOccupied(gridPosition.PosX - 1, gridPosition.PosY))
            {
                UpdateGridPosition(-1, 0);
                faceDirection = Direction.Dir_Left;

                UpdateGridPositionCoordinate();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!GridMap.Instance.IsOccupied(gridPosition.PosX + 1, gridPosition.PosY))
            {
                UpdateGridPosition(1, 0);
                faceDirection = Direction.Dir_Right;

                UpdateGridPositionCoordinate();
            }
        }
    }
}
