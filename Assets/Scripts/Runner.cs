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
            spriteSwapper.SwapAnimator("Up");

            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX, gridPosition.PosY + 1))
            {
                SetGridPosition(gridPosition.PosX, gridPosition.PosY + 1);
                faceDirection = Direction.Dir_Up;

                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            spriteSwapper.SwapAnimator("Down");

            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX, gridPosition.PosY - 1))
            {
                SetGridPosition(gridPosition.PosX, gridPosition.PosY - 1);
                faceDirection = Direction.Dir_Down;

                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spriteSwapper.SwapAnimator("Left");

            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX - 1, gridPosition.PosY))
            {
                SetGridPosition(gridPosition.PosX - 1, gridPosition.PosY);
                faceDirection = Direction.Dir_Left;

                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            spriteSwapper.SwapAnimator("Right");

            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX + 1, gridPosition.PosY))
            {
                SetGridPosition(gridPosition.PosX + 1, gridPosition.PosY);
                faceDirection = Direction.Dir_Right;

                GetComponent<Animator>().SetTrigger("Walk");
            }
        }

        // chop chop
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("Chop");

            if (faceDirection != Direction.Dir_Up)
                StartCoroutine(Chop());
        }
    }

    public IEnumerator Chop()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(GridMap.Instance.GridSize / 200f * 294f, rt.sizeDelta.y);

        Vector3 translateBy = new Vector3(GridMap.Instance.GridSize / 200f * -47f, 0f);
        if (faceDirection == Direction.Dir_Right)
            translateBy.x *= -1f;
        //else if (faceDirection == Direction.Dir_Down)
        //    translateBy.x *= 0.5f;

        transform.position += translateBy;

        yield return new WaitForSeconds(0.1f);

        while (inputCooldown)
        {
            yield return null;
        }

        rt.sizeDelta = new Vector2(rt.sizeDelta.y, rt.sizeDelta.y);
        transform.position -= translateBy;
    }
}
