using System;
using JetBrains.Annotations;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class GameCursor : MonoBehaviour
{
    /* ==================================
     *   CONFIG
     *  ================================== */    
    
    [SerializeField] private float baseXSpeed = 240;

    [SerializeField] private float baseYSpeed = 240;

    [SerializeField] private float minDragDistanceThreshold = 10;

    /* ==================================
     *   STATE
     *  ================================== */
    // The multiplier due to dragging the interactable object
    
    private float _dragPenalty = 1;
    
    //TODO: Add support for debuff penalties

    // Position of hunter in grid
    private Vector2 _gridPosition;

    // Checks if the hunter has moved to a new square in the current frame
    private bool _hasCrossedGridSquare = false;

    // The state the hunter is currently in
    private ICursorStateMachine _realState = Idle.State();

    // Property representing the state as enum
    private CursorState _state => _realState.GetState();

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //TODO: Set grid position?
        _gridPosition = new Vector2(0, 0);
        transform.position = new Vector3(0, 0, 0);
        _dragPenalty = TileUnderCursor.DragPenalty;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursorPosition();
        UpdateCursorState();
    }

    private void UpdateCursorPosition()
    {
        // Reset grid moved
        _hasCrossedGridSquare = false;
        
        // Get mouse movement
        var aX = Input.GetAxis("Mouse X");
        var aY = Input.GetAxis("Mouse Y");
        
        // Compute actual cursor movement
        var xMove = baseXSpeed * aX * Time.deltaTime * (_state is CursorState.Idle ? 1 : _dragPenalty);
        var yMove = baseYSpeed * aY * Time.deltaTime * (_state is CursorState.Idle ? 1 : _dragPenalty);
        
        // Update cursor position
        var position = CurrentCursorPosition;
        position += new Vector3(xMove, yMove, 0);
        transform.position = position;
        
        // Check if grid square changed
        var newPos = GridManager.GetGridPosition(position) ?? Vector2.zero; // TODO: CHANGE
        _hasCrossedGridSquare = newPos != _gridPosition;
        _gridPosition = newPos;
    }
    
    private void UpdateCursorState()
    {
        _realState = _realState.NextState(this);
    }
    
    // Properties to simplify code
    private static bool LeftClick => Input.GetMouseButtonDown(0);

    private static bool LeftUp => Input.GetMouseButtonUp(0);
    
    
    // Helper for streamlining logic and clarity
    private HunterInteractableBase TileUnderCursor =>
        GridManager.GetInteractable(_gridPosition) ?? EmptySquare.At(_gridPosition);

    private Vector3 CurrentCursorPosition => transform.position;
    
    private class EmptySquare : HunterInteractableBase
    {
        private Vector2 gridPosition = Vector2.zero;

        public override Vector2 GetGridPosition()
        {
            return gridPosition;
        }

        public override float DragPenalty => 1;

        private static EmptySquare _emptySquare = new EmptySquare();

        public static EmptySquare At(Vector2 coords)
        {
            _emptySquare.gridPosition = coords;
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
            if (cursor._hasCrossedGridSquare) cursor._dragPenalty = tile.DragPenalty;
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
        public CursorState GetState() => CursorState.MouseDown;
        public ICursorStateMachine NextState(GameCursor cursor)
        {
            // Retrieve the tile under the cursor
            var tile = cursor.TileUnderCursor;
            // If mouse button released, transit to idle
            if (LeftUp)
            {
                if(tile is IClickable clickedTile) clickedTile.OnClick();
                return Idle.State();
            }

            // If the cursor exited the grid or exceeds the bounds from where the click started, transit to dragging
            if (cursor._hasCrossedGridSquare || ExceedBounds(cursor))
            {
                return Dragging.State(tile, cursor);
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
            return _mouseDown;
        }

        private MouseDown(){}
    }
    
    private sealed class Dragging:ICursorStateMachine
    {
        private HunterInteractableBase _tileUnderDrag = EmptySquare.At(Vector2.zero);
        public CursorState GetState() => CursorState.Dragging;

        public ICursorStateMachine NextState(GameCursor cursor)
        {
            // If the cursor has crossed the grid square, and is draggable
            if (_tileUnderDrag is IDraggable draggedTile && cursor._hasCrossedGridSquare)
            {
                // Get current tile under cursor
                var tileUnderCursor = cursor.TileUnderCursor;
                // If the tile is empty or is the tile being dragged (multi-tile object) then move object
                if (tileUnderCursor is EmptySquare || tileUnderCursor == _tileUnderDrag)
                {
                    draggedTile.OnMove(cursor._gridPosition);
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

        public static ICursorStateMachine State(HunterInteractableBase tileUnderDrag, GameCursor cursor)
        {
            _dragging._tileUnderDrag = tileUnderDrag;
            return _dragging.NextState(cursor);
        }
        private Dragging(){}
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
    public static HunterInteractableBase GetInteractable(Vector2 position)
    {
        throw new NotImplementedException();

    }
}