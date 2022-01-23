using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playButton : MonoBehaviour
{
    public bool playButtonPressed;
    float delayTimer;
    public GameObject currentCanvas;
    void Start()
    {
        playButtonPressed = false;
    }

    void Update()
    {

        if (playButtonPressed)
        {
            Debug.Log(delayTimer);
            delayTimer += Time.deltaTime;
            if (delayTimer > 0.2)
            {
                //Load the new scene here
                //Destroy(currentCanvas);

                SceneManager.LoadScene("CharacterSelectScene");
            }
        }

        // listen out for secret key
        if (Input.GetKeyDown(KeyCode.F))
            SceneManager.LoadScene("LevelEditorScene");
    }
    public void OnButtonPress()
    {
        if (!playButtonPressed)
        {
            delayTimer = 0;
        }
        playButtonPressed = true;
    }
}
