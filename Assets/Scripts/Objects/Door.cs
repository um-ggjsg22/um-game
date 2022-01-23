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
        
        private void Update()
        {
            if (GridManager.GetRunnerPosition() == doorPosition && Input.GetKeyDown(KeyCode.UpArrow))
            {
                spawner.LoadRoom(levelToLoad);
            }
        }
    }
}