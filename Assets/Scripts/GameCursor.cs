using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GameCursor : MonoBehaviour
{
    [SerializeField] private float baseXSpeed = 1;

    [SerializeField] private float baseYSpeed = 1;

    // Boolean to check if cursor is dragging anything
    private bool _isDragging = false;

    // The multiplier due to dragging the interactable object
    private float _dragPenalty = 1;
    
    //TODO: Add support for penalties

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseClick();
        var aX = Input.GetAxis("Mouse X");
        var aY = Input.GetAxis("Mouse Y");
        var xMove = baseXSpeed * aX * (_isDragging ? _dragPenalty : 1);
        var yMove = baseYSpeed * aY * (_isDragging ? _dragPenalty : 1);
        transform.position += new Vector3(aX, aY, 0);
    }
    
    private void UpdateMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var interactable = GetInteractable(GetCursorGrid());
            if (interactable is IDraggable draggable)
            {
                _isDragging = true;
                _dragPenalty = draggable.GetDragPenalty();
            }else if (interactable is IClickable clickable)
            {
                clickable.ExecuteClickEffect();
            }
        }
        
        //TODO mouse up and also draggable logic
    }

    private GridPosition GetCursorGrid()
    {
        throw new NotImplementedException();
    }

    [CanBeNull]
    private IHunterInteractable GetInteractable(GridPosition pos)
    {
        throw new NotImplementedException();

    }
}
