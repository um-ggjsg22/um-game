using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private GameCursor hunterScript;

    private bool inputCooldown = false; // input cooldown from previous key input/animation playing
    private bool stunCooldown = false;

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
        if (inputCooldown || stunCooldown)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            spriteSwapper.SwapAnimator("Up");
            faceDirection = Direction.Dir_Up;

            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX, gridPosition.PosY + 1))
            {
                SetGridPosition(gridPosition.PosX, gridPosition.PosY + 1);

                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            spriteSwapper.SwapAnimator("Down");
            faceDirection = Direction.Dir_Down;

            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX, gridPosition.PosY - 1))
            {
                SetGridPosition(gridPosition.PosX, gridPosition.PosY - 1);

                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spriteSwapper.SwapAnimator("Left");
            faceDirection = Direction.Dir_Left;

            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX - 1, gridPosition.PosY))
            {
                SetGridPosition(gridPosition.PosX - 1, gridPosition.PosY);

                GetComponent<Animator>().SetTrigger("Walk");
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            spriteSwapper.SwapAnimator("Right");
            faceDirection = Direction.Dir_Right;

            if (!GridMap.Instance.IsOccupied(this, gridPosition.PosX + 1, gridPosition.PosY))
            {
                SetGridPosition(gridPosition.PosX + 1, gridPosition.PosY);

                GetComponent<Animator>().SetTrigger("Walk");
            }
        }

        // chop chop
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            // Chop Animation
            GetComponent<Animator>().SetTrigger("Chop");
            StartCoroutine(ChopAnimation());
            // Chop SFX
            AudioManager.instance.PlaySFX("Shing");

            // Chop Behaviour
            GridPosition offset = GetOffsetVector();
            GridPosition chopPos = new GridPosition(gridPosition.PosX + offset.PosX, gridPosition.PosY + offset.PosY);
            if (chopPos.PosX < 0 || chopPos.PosX >= GridMap.Instance.GridLength || chopPos.PosY < 0 || chopPos.PosY >= GridMap.Instance.GridLength)
                return;

            RoomObject roomObject = GridMap.Instance.OccupancyGrid(chopPos.PosX, chopPos.PosY);

            if (roomObject is Furniture furniture)
            {
                // Chop SFX
                AudioManager.instance.PlaySFX("Chop");
                if (furniture.BreakObject())
                {
                    // Apply Hunter debuff
                    hunterScript.GetRandomDebuff(5);
                }
            }
        }
    }

    private GridPosition GetOffsetVector()
    {
        switch (faceDirection)
        {
            case Direction.Dir_Up:
                return new GridPosition(0, 1);
            case Direction.Dir_Down:
                return new GridPosition(0, -1);
            case Direction.Dir_Left:
                return new GridPosition(-1, 0);
            case Direction.Dir_Right:
                return new GridPosition(1, 0);

            default:
                return new GridPosition(0, 0);
        }
    }

    private IEnumerator ChopAnimation()
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

    public void Stun(float seconds)
    {
        // Trigger SFX
        PlayHurtSFX();

        // Lock control
        StartCoroutine(StunCoroutine(seconds));
    }

    private IEnumerator StunCoroutine(float seconds)
    {
        stunCooldown = true;
        GetComponent<Image>().color = new Color(0.5f, 0f, 0f, 0.5f);
        yield return new WaitForSeconds(seconds);
        stunCooldown = false;
        GetComponent<Image>().color = Color.white;
    }

    private void PlayHurtSFX()
    {
        int rand = Random.Range(0, 6);

        switch (rand)
        {
            case 0: AudioManager.instance.PlaySFX("Oof");
                break;
            case 1:
                AudioManager.instance.PlaySFX("Ow");
                break;
            case 2:
                AudioManager.instance.PlaySFX("Ow2");
                break;
            case 3:
                AudioManager.instance.PlaySFX("Ow3");
                break;
            case 4:
                AudioManager.instance.PlaySFX("Ow4");
                break;
            case 5:
                AudioManager.instance.PlaySFX("Ow5");
                break;
            case 6:
                AudioManager.instance.PlaySFX("Ow6");
                break;

        }
        
    }
}
