using System;
using System.Collections;
using System.Net;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Cursor = UnityEngine.Cursor;
using Random = UnityEngine.Random;

public class GameCursor : MonoBehaviour
{
    /* ==================================
     *   CONFIG
     *  ================================== */    
    
    [Tooltip("Sets the base cursor speed in the X direction")]
    [SerializeField] private float baseXSpeed = 1000;

    [Tooltip("Sets the base cursor speed in the Y direction")]
    [SerializeField] private float baseYSpeed = 1000;

    [Tooltip("Sets the minimum 'drag distance' before a click is considered a drag")]
    [SerializeField] private float minDragDistanceThreshold = 10000;

    [Tooltip("What multiplier does a slow apply")]
    [SerializeField] private float slowFactor = 0.6f;

    [Tooltip("How probable is random teleportation out of 1")] 
    [SerializeField] private float teleportProbability = 0.002f;

    [FormerlySerializedAs("_teleportCooldown")] [Tooltip("What is the minimum duration between teleports in 100ms")] [SerializeField]
    private int teleportCooldown = 5;

    /* ==================================
     *   STATE
     *  ================================== */
    // The multiplier due to dragging the interactable object
    
    private float _dragPenalty = 1;
    
    // Position of hunter in grid
    private Vector2 _gridPosition;

    // Checks if the hunter has moved to a new square in the current frame
    private bool _hasCrossedGridSquare = false;

    // The state the hunter is currently in
    private ICursorStateMachine _realState = Idle.State();

    // Property representing the state as enum
    private CursorState _state => _realState.GetState();
    
    // Get min and max pixel coordinates of grid
    private Vector3 _gridBottomLeft = Vector3.negativeInfinity;
    private Vector3 _gridTopRight = Vector3.negativeInfinity;
    private float _mapWidth;
    private float _mapHeight;
    
    //TODO: Add support for debuff penalties
    // Debuff state durations in 100ms
    private int _slowDurationLeft = 0;
    private int _teleportDurationLeft = 0;
    private int _freezeDurationLeft = 0;
    private int _invertDurationLeft = 0;
    private int _flipAxesDurationLeft = 0;
    private int _teleportCooldownLeft = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //TODO: Set grid position?
        _dragPenalty = TileUnderCursor.DragPenalty();
        _gridBottomLeft = GridManager.GetGridBottomLeft();
        _gridTopRight = GridManager.GetGridTopRight();
        _mapWidth = _gridTopRight.x - _gridBottomLeft.x;
        _mapHeight = _gridTopRight.y - _gridBottomLeft.y;
        transform.position = _gridBottomLeft + new Vector3(_mapWidth * 0.5f, _mapHeight * 0.5f, 0);
        _gridPosition = (Vector2)GridManager.GetGridPosition(CurrentCursorPosition);
        StartCoroutine(TickDownDebuffs());

        ApplyFlip(5);
        ApplyInvert(5);
        ApplyFreeze(5);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursorPosition();
        UpdateCursorState();
    }
    
    private IEnumerator TickDownDebuffs()
    {
        while (true)
        {
            if (_slowDurationLeft > 0) _slowDurationLeft -= 1;
            if (_teleportDurationLeft > 0) _teleportDurationLeft -= 1;
            if (_freezeDurationLeft > 0) _freezeDurationLeft -= 1;
            if (_invertDurationLeft > 0) _invertDurationLeft -= 1;
            if (_flipAxesDurationLeft > 0) _flipAxesDurationLeft -= 1;
            if (_teleportCooldownLeft > 0) _teleportCooldownLeft -= 1;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateCursorPosition()
    {
        // Reset grid moved
        _hasCrossedGridSquare = false;
        
        // Skip movement for freeze
        if (_freezeDurationLeft > 0) return;

        Vector3 position;
        //Debug.Log($"Current Pos: {CurrentCursorPosition}");
        // Teleport for teleport
        if (_teleportDurationLeft > 0 && _teleportCooldownLeft == 0 && Random.value < teleportProbability)
        {
            position = new Vector3(_gridBottomLeft.x + Random.value * _mapWidth,
                _gridBottomLeft.y + Random.value * _mapHeight);
            _dragPenalty = 1;
            if(Input.GetMouseButton(0)) _realState = Dragging.State(CurrentCursorPosition, EmptySquare.At(Vector2.negativeInfinity));
            else _realState = Idle.State();
            _teleportCooldownLeft = teleportCooldown;
        }
        else
        {
            // Get mouse movement
            var aX = Input.GetAxis("Mouse X");
            var aY = Input.GetAxis("Mouse Y");

            // Compute actual cursor movement factoring in slow
            var xMove = baseXSpeed * aX * Time.deltaTime
                        * (_state is CursorState.Idle ? 1 : _dragPenalty)
                        * (_slowDurationLeft > 0 ? slowFactor : 1);
            var yMove = baseYSpeed * aY * Time.deltaTime
                        * (_state is CursorState.Idle ? 1 : _dragPenalty)
                        * (_slowDurationLeft > 0 ? slowFactor : 1);

            Vector3 offsetVector;
            // Update cursor position
            // Check if axes are flipped
            if (_flipAxesDurationLeft > 0)
            {
                offsetVector = new Vector3(yMove, xMove, 0);
            }
            else
            {
                offsetVector = new Vector3(xMove, yMove, 0);
            }

            // Check if axes are inverted
            if (_invertDurationLeft > 0)
            {
                position = CurrentCursorPosition - offsetVector;
            }
            else
            {
                position = CurrentCursorPosition + offsetVector;
            }

            position = CorrectPositionForBounds(position);
        }

        transform.position = position;
        
        // Check if grid square changed
        var newPos = GridManager.GetGridPosition(position);
        if (newPos is null)
        {
            return;
        }
        _hasCrossedGridSquare = newPos != _gridPosition;
        _gridPosition = (Vector2)newPos;
    }
    
    private Vector3 CorrectPositionForBounds(Vector3 position)
    {
        if (position.y > _gridTopRight.y) position.y = _gridTopRight.y;
        else if (position.y < _gridBottomLeft.y) position.y = _gridBottomLeft.y;
        if (position.x > _gridTopRight.x) position.x = _gridTopRight.x;
        else if (position.x < _gridBottomLeft.x) position.x = _gridBottomLeft.x;
        return position;
    }
    
    private void UpdateCursorState()
    {
        _realState = _realState.NextState(this);
    }

    /** ==================
     * APPLY DEBUFFS FUNCTIONS
     *  ==================*/
    public void ApplySlow(int durationInSecs)
    {
        if(_slowDurationLeft == 0)
        {
            // Color tint
            GetComponent<Image>().color = Color.green;
            StartCoroutine(RemoveSlowVisual());
        }
        _slowDurationLeft = durationInSecs * 10;

    }

    private IEnumerator RemoveSlowVisual()
    {
        yield return null;
        while (_slowDurationLeft > 0)
            yield return new WaitForSeconds(0.1f);

        GetComponent<Image>().color = Color.white;
    }
    
    public void ApplyFreeze(int durationInSecs)
    {
        if (_freezeDurationLeft == 0)
        {
            // Color tint
            GetComponent<Image>().color = new Color(0f, 0f, 1f, 0.5f);
            StartCoroutine(RemoveFreezeVisual());
        }

        _freezeDurationLeft = durationInSecs * 10;
    }

    private IEnumerator RemoveFreezeVisual()
    {
        yield return null;
        while (_freezeDurationLeft > 0)
            yield return new WaitForSeconds(0.1f);

        GetComponent<Image>().color = Color.white;
    }

    public void ApplyTeleport(int durationInSecs)
    {
        if (_teleportDurationLeft == 0)
        {
            // Flip Y-axis
            transform.localScale = new Vector3(transform.localScale.x, -1f, 1f);
            StartCoroutine(RemoveTeleportVisual());
        }

        _teleportDurationLeft = durationInSecs * 10;
    }

    private IEnumerator RemoveTeleportVisual()
    {
        yield return null;
        while (_teleportDurationLeft > 0)
            yield return new WaitForSeconds(0.1f);

        // Revert Y-axis
        transform.localScale = new Vector3(transform.localScale.x, 1f, 1f);
    }

    public void ApplyInvert(int durationInSecs)
    {
        if (_invertDurationLeft == 0)
        {
            // Flip X-axis
            transform.localScale = new Vector3(-1f, transform.localScale.y, 1f);
            StartCoroutine(RemoveInvertVisual());
        }

        _invertDurationLeft = durationInSecs * 10;
    }

    private IEnumerator RemoveInvertVisual()
    {
        yield return null;
        while (_invertDurationLeft > 0)
            yield return new WaitForSeconds(0.1f);

        // Revert Y-axis
        transform.localScale = new Vector3(1f, transform.localScale.y, 1f);
    }

    public void ApplyFlip(int durationInSecs)
    {
        if (_flipAxesDurationLeft == 0)
        {
            // Rotate
            transform.eulerAngles = new Vector3(0f, 0f, 90f);
            StartCoroutine(RemoveFlipVisual());
        }

        _flipAxesDurationLeft = durationInSecs * 10;
    }

    private IEnumerator RemoveFlipVisual()
    {
        yield return null;
        while (_flipAxesDurationLeft > 0)
            yield return new WaitForSeconds(0.1f);

        // Rotate back
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }



    // Properties to simplify code
    private static bool LeftClick => Input.GetMouseButtonDown(0);

    private static bool LeftUp => Input.GetMouseButtonUp(0);
    
    
    // Helper for streamlining logic and clarity
    private IHunterInteractable TileUnderCursor =>
        GridManager.GetInteractable(_gridPosition) ?? EmptySquare.At(_gridPosition);

    private Vector3 CurrentCursorPosition => transform.position;
    
    private class EmptySquare : IHunterInteractable
    {
        private Vector2 _gridPosition = Vector2.zero;

        public Vector2 GetGridPosition()
        {
            return _gridPosition;
        }

        public float DragPenalty() => 1;

        private static EmptySquare _emptySquare = new EmptySquare();

        public static EmptySquare At(Vector2 coords)
        {
            _emptySquare._gridPosition = coords;
            return _emptySquare;
        }
        private EmptySquare()
        {
        }
    }

    // Enum representation of real state
    private enum CursorState {Idle, MouseDown, Dragging}

    // State Machine code
    private interface ICursorStateMachine
    {
        CursorState GetState();
        ICursorStateMachine NextState(GameCursor cursor);
    }

    private sealed class Idle : ICursorStateMachine
    {
        public CursorState GetState() => CursorState.Idle;

        public ICursorStateMachine NextState(GameCursor cursor)
        {
            // Check tile below cursor
            var tile = cursor.TileUnderCursor;
            // Update drag penalty
            if (cursor._hasCrossedGridSquare) cursor._dragPenalty = tile.DragPenalty();
            // If click, transition to MouseDown state
            if (LeftClick)
            {
                cursor._hasCrossedGridSquare = false;
                return MouseDown.State(cursor.CurrentCursorPosition);
            }
            // Otherwise stay Idle
            return this;
        }

        // Singleton design
        private static readonly Idle _idle = new Idle();
        public static Idle State() => _idle;
        
        private Idle(){}

    }

    private sealed class MouseDown : ICursorStateMachine
    {
        // Position where clicking started
        private Vector3 _initialCursorPosition = Vector3.zero;
        private IHunterInteractable _tile = EmptySquare.At(Vector2.negativeInfinity);
        public CursorState GetState() => CursorState.MouseDown;
        public ICursorStateMachine NextState(GameCursor cursor)
        {
            // If mouse button released, transit to idle
            if (LeftUp)
            {
                if(_tile is IClickable clickedTile) clickedTile.OnClick();
                return Idle.State();
            }

            // If the cursor exited the grid or exceeds the bounds from where the click started, transit to dragging
            if (cursor._hasCrossedGridSquare || ExceedBounds(cursor))
            {
                Debug.Log(cursor.TileUnderCursor is IDraggable);
                return Dragging.State(_initialCursorPosition, _tile).NextState(cursor);
            }

            // Otherwise stay in MouseDown state
            return this;
        }

        // Helper method to check if cursor exceeds the bounds from where the click started
        private bool ExceedBounds(GameCursor cursor)
        {
            return Vector2.Distance(cursor.CurrentCursorPosition, _initialCursorPosition) > cursor.minDragDistanceThreshold;
        }

        // Singleton design
        private static readonly MouseDown _mouseDown = new MouseDown();

        public static MouseDown State(Vector3 cursorCoords)
        {
            _mouseDown._initialCursorPosition = cursorCoords;
            _mouseDown._tile = GridManager.GetInteractable((Vector2)GridManager.GetGridPosition(cursorCoords));
            return _mouseDown;
        }

        private MouseDown(){}
    }
    
    private sealed class Dragging:ICursorStateMachine
    {
        private Vector3 _initialCursorPosition;
        private IHunterInteractable _tileUnderDrag = EmptySquare.At(Vector2.negativeInfinity);
        public CursorState GetState() => CursorState.Dragging;

        public ICursorStateMachine NextState(GameCursor cursor)
        {
            Debug.Log($"Tile Under Drag: {_tileUnderDrag}, IsDraggable {_tileUnderDrag is IDraggable}, HasCrossed {cursor._hasCrossedGridSquare}");
            // If the cursor has crossed the grid square, and is draggable
            if (_tileUnderDrag is IDraggable draggedTile && cursor._hasCrossedGridSquare)
            {
                var diff = cursor._gridPosition - (Vector2)GridManager.GetGridPosition(_initialCursorPosition);
                var transitionVec = Vector2.zero;
                if (diff.x*diff.x > diff.y*diff.y)
                {
                    transitionVec = new Vector2(diff.x, 0).normalized;
                }
                else
                {
                    transitionVec = new Vector2(0, diff.y).normalized;
                }
                var moveResult = draggedTile.OnMove(transitionVec);
                // Otherwise drop the dragged item and reset the drag penalty
                if(!moveResult)
                {
                    var empty = EmptySquare.At(Vector2.negativeInfinity);
                    cursor._dragPenalty = empty.DragPenalty();
                    return Dragging.State(cursor.CurrentCursorPosition, empty);
                }
            }
            // If release cursor, return to idle state
            if (LeftUp)
            {
                return Idle.State();
            }
            
            // Otherwise stay in dragging state
            return this;
        }
        
        // Singleton design
        private static readonly Dragging _dragging = new Dragging();

        public static Dragging State(Vector3 initialCursorPosition, IHunterInteractable tileUnderDrag)
        {
            _dragging._initialCursorPosition = initialCursorPosition;
            _dragging._tileUnderDrag = tileUnderDrag;
            return _dragging;
        }
        private Dragging(){}
    }
    
    public void GetRandomDebuff(int duration)
    {
        //Range delimiters:
        //Freeze - 0-1
        //Teleport - 1-3
        //Flip - 3-5
        //Invert - 5-8
        //Slow - 8-12
        var rand = Random.Range(0, 12);
        if (rand < 5)
        {
            if (rand > 3)
            {
                ApplyFlip(duration);
            }else if (rand > 1)
            {
                ApplyTeleport(duration);
            }
            else
            {
                ApplyFreeze(duration);
            }
        }
        else
        {
            if (rand < 8)
            {
                ApplyInvert(duration);
            }
            else
            {
                ApplySlow(duration);
            }
        }
    }
}

public static class GridManager
{
    [CanBeNull]
    public static Vector2? GetGridPosition(Vector2 rawCoords)
    {
        return GridMap.Instance.GetGridCoordinate(rawCoords);
    }
    [CanBeNull]
    public static IHunterInteractable GetInteractable(Vector2 position)
    {
        return GridMap.Instance.GetInteractable((int)position.x, (int)position.y);
    }

    public static Vector2 GetRunnerPosition()
    {
        return GridMap.Instance.GetRunnerPosition();
    }

    public static Vector3 GetGridBottomLeft()
    {
        return GridMap.Instance.FloorMinCoordinate();
    }

    public static Vector3 GetGridTopRight()
    {
        return GridMap.Instance.FloorMaxCoordinate();
    }

    public static bool IsGridOccupied(RoomObject currObject, Vector2 coordinate)
    {
        return GridMap.Instance.IsOccupied(currObject, (int)coordinate.x, (int)coordinate.y);
    }
    
    public static bool MoveObject(RoomObject currObject, Vector2 coordinate)
    {
        //TODO
        // Update occupancy grid with new pos
        return currObject.SetGridPosition((int)coordinate.x, (int)coordinate.y);
    }

    public static Vector3 GetPositionCoordinate(Vector2 gridSquare, RoomObject obj = null)
    {
        if (obj is null) return GridMap.Instance.GetPositionCoordinate((int)gridSquare.x, (int)gridSquare.y);
        return GridMap.Instance.GetPositionCoordinate((int)gridSquare.x, (int)gridSquare.y) + new Vector3((obj.SpriteWidth - 1) * 0.5f * GridMap.Instance.GridSize, (obj.SpriteHeight - 1) * 0.5f * GridMap.Instance.GridSize);
    }
}