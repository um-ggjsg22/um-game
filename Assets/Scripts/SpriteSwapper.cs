using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AnimatorSwap
{
    public string name;
    public RuntimeAnimatorController animator;
}

[RequireComponent(typeof(Animator))]
public class SpriteSwapper : MonoBehaviour
{
    [SerializeField]
    private AnimatorSwap[] spriteSwapList;

    private Dictionary<string, RuntimeAnimatorController> spriteSwapDict;

    private Animator animatorComponent;

    // Start is called before the first frame update
    void Awake()
    {
        spriteSwapDict = new Dictionary<string, RuntimeAnimatorController>();
        foreach (var sprite in spriteSwapList)
        {
            spriteSwapDict.Add(sprite.name, sprite.animator);
        }

        animatorComponent = GetComponent<Animator>();
    }

    public void SwapAnimator(string name)
    {
        animatorComponent.runtimeAnimatorController = spriteSwapDict[name];
    }
}
