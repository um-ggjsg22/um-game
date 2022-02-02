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

    [SerializeField]
    [Tooltip("If true, load current room level from PlayerPrefs")]
    private bool readPlayerPref = true;
    [SerializeField]
    [Tooltip("If readPlayerPref is false, load roomLevel instead")]
    private int roomLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        int level = roomLevel;
        if (readPlayerPref)
            level = PlayerPrefs.GetInt("NextLevel", roomLevel);

        LoadRoom(level);

        if (level == roomData.Length - 1)
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
