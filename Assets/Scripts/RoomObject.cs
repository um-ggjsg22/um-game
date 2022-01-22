using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridPosition), typeof(Image))]
public class RoomObject : MonoBehaviour, IHunterInteractable
{
    [SerializeField]
    private string objectName;
    public string ObjectName
    {
        get { return objectName; }
        set { objectName = value; }
    }

    [SerializeField]
    private int spriteWidth;
    public int SpriteWidth
    {
        get { return spriteWidth; }
        set { spriteWidth = value; }
    }
    [SerializeField]
    private int spriteHeight;
    public int SpriteHeight
    {
        get { return spriteHeight; }
        set { spriteHeight = value; }
    }

    [SerializeField]
    private int baseWidth;
    public int BaseWidth
    {
        get { return baseWidth; }
        set { baseWidth = value; }
    }
    [SerializeField]
    private int baseHeight;
    public int BaseHeight
    {
        get { return baseHeight; }
        set { baseHeight = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetGridPosition()
    {
        return new Vector2(GetComponent<GridPosition>().PosX, GetComponent<GridPosition>().PosY);
    }

    public float DragPenalty() => 1;
}
