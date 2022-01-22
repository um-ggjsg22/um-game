using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : RoomObject, IDraggable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool OnMove(Vector2 newPosition)
    {
        throw new System.NotImplementedException();
    }
}
