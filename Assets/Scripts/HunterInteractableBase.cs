using UnityEngine;

public abstract class HunterInteractableBase
{
   public abstract Vector2 GetGridPosition();
   
   public virtual float DragPenalty => 1;
}