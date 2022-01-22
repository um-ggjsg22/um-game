using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomEditor : MonoBehaviour
{
    [SerializeField]
    private RoomManager roomManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFloorSprite(Image floor)
    {
        roomManager.FloorImage.sprite = floor.sprite;
    }
    public void SetWallSprite(Image wall)
    {
        roomManager.WallImage.sprite = wall.sprite;
    }
}
