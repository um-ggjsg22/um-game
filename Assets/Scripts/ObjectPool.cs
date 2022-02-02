using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Dictionary<string, RoomObject> objectsDict;

    // Start is called before the first frame update
    void Awake()
    {
        // Instantiate Dictionary of objects
        objectsDict = new Dictionary<string, RoomObject>();
        foreach (Transform child in transform)
        {
            objectsDict.Add(child.gameObject.name, child.GetComponent<RoomObject>());
        }
    }

    public RoomObject SpawnObject(string name, int gridX, int gridY)
    {
        // Instantiate object
        Debug.Log(name);
        GameObject newObject = Instantiate(objectsDict[name].gameObject);
        newObject.SetActive(true);

        RoomObject roomObjectScript = newObject.GetComponent<RoomObject>();
        // Set object size
        roomObjectScript.SetObjectSize();
        // Set position
        roomObjectScript.SetGridPosition(gridX, gridY, false);

        return roomObjectScript;
    }
}
