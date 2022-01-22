using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : RoomObject, IDraggable
{
    private bool _isAnimating = false;

    [Tooltip("Time in seconds it takes for the shifting animation to complete")]
    [SerializeField]private float animationTime = 0.5f;
    
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
        if (_isAnimating) return false;
        if (Vector2.Distance(this.GetGridPosition(), newPosition) > 1) return false;
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
