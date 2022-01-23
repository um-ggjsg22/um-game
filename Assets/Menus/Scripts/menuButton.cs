using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public bool playButtonPressed;
    public GameObject selectCharacterPrefab;
    float delayTimer;
    public GameObject currentCanvas;
    void Start()
    {
        playButtonPressed = false;
    }

    void Update()
    {
        
        if(playButtonPressed)
        {
            //Debug.Log(Time.deltaTime);
            delayTimer += Time.deltaTime;
            if (delayTimer > 0.2)
            {
                Instantiate(selectCharacterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                Destroy(currentCanvas);
            }
        }
    }
    public void OnButtonPress()
    {
        if(!playButtonPressed)
        {
            delayTimer = 0;
        }
        playButtonPressed = true;
    }
}
