using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridPosition
{
    private int posX;
    public int PosX
    {
        get { return posX; }
        set { posX = value; }
    }
    private int posY;
    public int PosY
    {
        get { return posY; }
        set { posY = value; }
    }

    public GridPosition(int posX, int posY)
    {
        this.posX = posX;
        this.posY = posY;
    }
}
