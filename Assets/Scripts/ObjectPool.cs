using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnObject(string name)
    {
        // Instantiate object
        /*GameObject newObject = Instantiate(roomObjectPrefab);
        newObject.SetActive(true);

        // Set parent
        newObject.transform.parent = spawnObjectParent.Find("GridRow" + gridY);

        // Set position
        newObject.GetComponent<GridPosition>().SetGridPosition(gridX, gridY);
        // Set offset position
        newObject.transform.position += new Vector3((obj.SpriteWidth - 1) * 0.5f * GridMap.Instance.GridSize, (obj.SpriteHeight - 1) * 0.5f * GridMap.Instance.GridSize);

        // Add to List
        roomObjects.Add(newObject.GetComponent<RoomObject>());
        // Add to occupancy grid
        for (int i = gridX; i < gridX + obj.BaseWidth; ++i)
        {
            for (int j = gridY; j < gridY + obj.BaseHeight; ++j)
            {
                occupancyGrid[i, j] = newObject.GetComponent<RoomObject>();
            }
        }*/
    }
}
