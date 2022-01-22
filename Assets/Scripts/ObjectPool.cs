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

    // Update is called once per frame
    void Update()
    {
        
    }

    public RoomObject SpawnObject(string name, int gridX, int gridY, Transform parent)
    {
        // Instantiate object
        GameObject newObject = Instantiate(objectsDict[name].gameObject);
        newObject.SetActive(true);

        // Set parent
        newObject.transform.SetParent(parent.Find("GridRow" + gridY));

        RoomObject roomObjectScript = newObject.GetComponent<RoomObject>();

        // Set object size
        newObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GridMap.Instance.GridSize * roomObjectScript.SpriteWidth,
            GridMap.Instance.GridSize * roomObjectScript.SpriteHeight);

        // Set position
        roomObjectScript.SetGridPosition(gridX, gridY);
        // Set offset position
        /*newObject.transform.position += new Vector3(
            (roomObjectScript.SpriteWidth - 1) * 0.5f * GridMap.Instance.GridSize,
            (roomObjectScript.SpriteHeight - 1) * 0.5f * GridMap.Instance.GridSize
            );*/

        return roomObjectScript;
    }
}
