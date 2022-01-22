using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [SerializeField]
    private int spriteWidth;
    public int SpriteWidth
    {
        get { return spriteWidth; }
    }
    [SerializeField]
    private int spriteHeight;
    public int SpriteHeight
    {
        get { return spriteHeight; }
    }

    [SerializeField]
    private int baseWidth;
    public int BaseWidth
    {
        get { return baseWidth; }
    }
    [SerializeField]
    private int baseHeight;
    public int BaseHeight
    {
        get { return baseHeight; }
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
