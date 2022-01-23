using System;
using UnityEngine;

namespace Objects
{
    public class Door: MonoBehaviour
    {
        [SerializeField] private Runner runner;
        [SerializeField] private RoomSpawner spawner;
        [SerializeField] private Vector2 doorPosition;
        [SerializeField] private int levelToLoad = 2;

        [SerializeField] private SceneChanger sceneChanger;

        private bool activated = false;
        
        private void Update()
        {
            if (GridManager.GetRunnerPosition() == doorPosition && Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!activated)
                {
                    //spawner.LoadRoom(levelToLoad);

                    PlayerPrefs.SetInt("NextLevel", PlayerPrefs.GetInt("NextLevel") + 1);

                    activated = true;
                }

                sceneChanger.ChangeScene("GameScene");
            }
        }
    }
}