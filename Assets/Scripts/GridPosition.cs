using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    private int posX;
    public int PosX
    {
        get { return posX; }
    }
    private int posY;
    public int PosY
    {
        get { return posY; }
    }

    // Start is called before the first frame update
    public GridPosition(int posX, int posY)
    {
        this.posX = posX;
        this.posY = posY;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetGridPosition(int x, int y)
    {
        if (x < 0 || x >= GridMap.Instance.GridLength || y < 0 || y >= GridMap.Instance.GridLength)
            return false;

        posX = x;
        posY = y;

        UpdateGridPositionCoordinate();

        return true;
    }

    public bool UpdateGridPosition(int xDelta, int yDelta)
    {
        int prevPosX = posX, prevPosY = posY;

        posX = Mathf.Clamp(posX + xDelta, 0, GridMap.Instance.GridLength - 1);
        posY = Mathf.Clamp(posY + yDelta, 0, GridMap.Instance.GridLength - 1);

        // return whether it got updated
        return !(prevPosX == posX && prevPosY == posY) ;
    }

    public void UpdateGridPositionCoordinate()
    {
        // set the new GameObject position
        transform.position = GridMap.Instance.GetPositionCoordinate(posX, posY);
    }
}
