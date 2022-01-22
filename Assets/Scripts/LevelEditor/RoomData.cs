using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public string floor;
    public string wall;
    public RoomObjectData[] objects;
}

[System.Serializable]
public class RoomObjectData
{
    public string objectType;
    //public int isOnFloor;
    public int gridX;
    public int gridY;
}
