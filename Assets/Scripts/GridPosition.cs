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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
