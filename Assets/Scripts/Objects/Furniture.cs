using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furniture : RoomObject, IDraggable
{
    private bool _isAnimating = false;

    [Tooltip("Time in seconds it takes for the shifting animation to complete")]
    [SerializeField]private float animationTime = 0.5f;

    [Tooltip("How much to slow the cursor by when dragging this item")]
    [SerializeField] private float dragMultiplier = 0.7f;

    public override float DragPenalty()
    {
        return dragMultiplier;
    }

    [SerializeField]
    private int durability = 3;
    [SerializeField]
    private Sprite brokenSprite;

    public bool BreakObject()
    {
        durability -= 1;
        Debug.Log(durability);
        if (durability == 0)
        {
            Debug.Log("Destroyed!");

            // Break object sprite
            StartCoroutine(SwapBrokenSprite());
            // Remove from occupancy grid
            GridMap.Instance.RemoveRoomObjectFromOccupancyGrid(this);

            return true;
        }
        else
        {
            // TODO: Flashing the sprite Coroutine
            StartCoroutine(FlashSprite());

            return false;
        }
    }

    private IEnumerator FlashSprite()
    {
        Image imageComponent = GetComponent<Image>();
        Color imageColor = imageComponent.color;

        imageColor.a = 0.5f;
        imageComponent.color = imageColor;
        yield return new WaitForSeconds(0.1f);

        imageColor.a = 1f;
        imageComponent.color = imageColor;
    }

    private IEnumerator SwapBrokenSprite()
    {
        // Swap to brokenSprite
        Image imageComponent = GetComponent<Image>();
        imageComponent.sprite = brokenSprite;

        // attach to parent
        transform.SetParent(transform.parent.parent.Find("Destroyed"));

        // TODO: alpha fade out instead

        yield return new WaitForSeconds(3f);

        // Destroy after 3 seconds
        Destroy(gameObject);
    }

    public bool OnMove(Vector2 transitionVector)
    {
        if (_isAnimating) return false;
        if (transitionVector.magnitude > 1) return false;
        var newPosition = GetGridPosition() + transitionVector;
        if(!this.CanPlace(newPosition)) return false;
        StartCoroutine(AnimateMovement(GridManager.GetPositionCoordinate(newPosition, this)));
        GridManager.MoveObject(this, newPosition);
        return true;
    }
    
    public IEnumerator AnimateMovement(Vector3 endPosition)
    {
        _isAnimating = true;
        var initialPosition = transform.position;
        float time = 0;
        while (time < animationTime)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPosition, endPosition, time/animationTime);
            yield return null;
        }
        transform.position = endPosition;
        _isAnimating = false;
        yield return null;
    }
}
