using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteSwapper))]
public class Runner : RoomObject
{
    public enum Direction
    {
        Dir_Up,
        Dir_Down,
        Dir_Left,
        Dir_Right,
    }

    private Direction faceDirection;    // direction the Runner is facing

    [SerializeField]
    private int startGridX = 4;
    [SerializeField]
    private int startGridY = 0;

    private SpriteSwapper spriteSwapper;

    private bool inputCooldown = false; // input cooldown from previous key input/animation playing

    void Awake()
    {
        faceDirection = Direction.Dir_Up;

        spriteSwapper = GetComponent<SpriteSwapper>();
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        // Set Runner's start position
        SetGridPosition(startGridX, startGridY, false);
    }

    public void DisableInput()
    {
        inputCooldown = true;
    }
    public void EnableInput()
    {
        inputCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputCooldown)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX, gridPosition.PosY + 1))
            {
                SetGridPosition(gridPosition.PosX, gridPosition.PosY + 1);
                faceDirection = Direction.Dir_Up;

                spriteSwapper.SwapAnimator("Up");
                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX, gridPosition.PosY - 1))
            {
                SetGridPosition(gridPosition.PosX, gridPosition.PosY - 1);
                faceDirection = Direction.Dir_Down;

                spriteSwapper.SwapAnimator("Down");
                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX - 1, gridPosition.PosY))
            {
                SetGridPosition(gridPosition.PosX - 1, gridPosition.PosY);
                faceDirection = Direction.Dir_Left;

                spriteSwapper.SwapAnimator("Left");
                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX + 1, gridPosition.PosY))
            {
                SetGridPosition(gridPosition.PosX + 1, gridPosition.PosY);
                faceDirection = Direction.Dir_Right;

                spriteSwapper.SwapAnimator("Right");
                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
    }
}
