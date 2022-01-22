using UnityEngine;

public abstract class HunterInteractableBase : MonoBehaviour
{
   public abstract Vector2 GetGridPosition();
   
   public virtual float DragPenalty => 1;

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
}