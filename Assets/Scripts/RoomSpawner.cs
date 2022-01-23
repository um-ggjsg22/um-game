using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField]
    private RoomManager roomManager;
    [SerializeField]
    private ObjectPool objectPool;
    [SerializeField]
    private TextAsset[] roomData;

    [SerializeField]
    private GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        int level = PlayerPrefs.GetInt("NextLevel");
        LoadRoom(level);

        if (level == 3)
            door.SetActive(false);
    }

    public void LoadRoom(int id)
    {
        // Deserialize RoomData
        RoomData data = JsonUtility.FromJson<RoomData>(roomData[id].text);

        // 1. Set Floor
        roomManager.FloorImage.sprite = Resources.Load<Sprite>(/*"Floors/" + */data.floor);

        // 2. Set Wallpaper
        roomManager.WallImage.sprite = Resources.Load<Sprite>(/*"Walls/" + */data.wall);

        // 3. Spawn objects
        foreach (RoomObjectData objData in data.objects)
        {
            // Instantiate the object
            RoomObject roomObject = objectPool.SpawnObject(objData.objectType, objData.gridX, objData.gridY);

            // Add to Object List & Occupancy Grid
            roomManager.AddObjectToList(roomObject, objData.gridX, objData.gridY);
        }
    }
}
