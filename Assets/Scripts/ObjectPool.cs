using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectTemplate
{
    public string name;
    public RoomObject obj;
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private ObjectTemplate[] objectList;

    private Dictionary<string, RoomObject> objectsDict;

    // Start is called before the first frame update
    void Awake()
    {
        // Instantiate Dictionary of objects
        objectsDict = new Dictionary<string, RoomObject>();
        foreach (ObjectTemplate obj in objectList)
        {
            objectsDict.Add(obj.name, obj.obj);
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
