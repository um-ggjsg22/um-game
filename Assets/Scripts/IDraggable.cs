using UnityEngine;

public interface IDraggable
{
    bool OnMove(Vector2 newPosition);
}

public static class DragHelperMethods
{
    public static bool CanPlace(this RoomObject obj, Vector2 bottomLeftCoord)
    {
        for (var i = 0; i < obj.BaseWidth; i++)
        {
            for (var j = 0; j < obj.BaseHeight; j++)
            {
                if (GridManager.IsGridOccupied(obj, bottomLeftCoord + new Vector2(i, j))) return false;
            }
        }
        return true;
    }
}