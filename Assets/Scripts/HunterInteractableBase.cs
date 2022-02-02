using UnityEngine;

public interface IHunterInteractable
{
    Vector2 GetGridPosition();

    float DragPenalty();
}