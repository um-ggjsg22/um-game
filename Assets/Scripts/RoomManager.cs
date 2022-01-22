using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private Image floorImage;
    public Image FloorImage
    {
        get { return floorImage; }
    }
    [SerializeField]
    private Image wallImage;
    public Image WallImage
    {
        get { return wallImage; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
