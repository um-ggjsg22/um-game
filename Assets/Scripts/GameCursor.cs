using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GameCursor : MonoBehaviour
{
    [SerializeField] private float baseXSpeed = 1;

    [SerializeField] private float baseYSpeed = 1;

    // Boolean to check if cursor is dragging anything
    public bool isDragging = false;

    // The multiplier due to dragging the interactable object
    public float clickPenalty = 1;
    
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
        var xMove = baseXSpeed * aX * (isDragging ? clickPenalty : 1);
        var yMove = baseYSpeed * aY * (isDragging ? clickPenalty : 1);
        transform.position += new Vector3(aX, aY, 0);
    }
    
    private void UpdateMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }

    public GridPosition GetCursorGrid()
    {
        throw new NotImplementedException();
    }
    

}
