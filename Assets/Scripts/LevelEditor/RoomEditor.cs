using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class RoomEditor : MonoBehaviour
{
    [SerializeField]
    private RoomManager roomManager;
    [SerializeField]
    private TextMeshProUGUI debugText;
    [SerializeField]
    private TMP_InputField fileNameField;
    [SerializeField]
    private int debugTextDisplayDuration = 5;

    // Start is called before the first frame update
    void Start()
    {
        debugText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFloorSprite(Image floor)
    {
        roomManager.FloorImage.sprite = floor.sprite;
    }
    public void SetWallSprite(Image wall)
    {
        roomManager.WallImage.sprite = wall.sprite;
    }



    public void SaveGame()
    {
        // Store data in a single object
        RoomData data = new RoomData();
        data.floor = roomManager.FloorImage.sprite.name;
        data.wall = roomManager.WallImage.sprite.name;
        data.objects = new RoomObjectData[roomManager.RoomObjects.Count];
        for (int i = 0; i < roomManager.RoomObjects.Count; ++i)
        {
            data.objects[i] = new RoomObjectData();
            data.objects[i].objectType = roomManager.RoomObjects[i].ObjectName;
            Vector2 gridPos = roomManager.RoomObjects[i].GetGridPosition();
            data.objects[i].gridX = (int)gridPos.x;
            data.objects[i].gridY = (int)gridPos.y;
        }

        // Serialize data to JSON
        string jsonData = JsonUtility.ToJson(data);

        // Export JSON data to text file
        string path = Application.persistentDataPath + "/" + fileNameField.text;
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(jsonData);
        writer.Close();

        Debug.Log(path);

        debugText.text = "Saved to " + path;

        // If saved successfully, display text "Saved" for 5 secondss
        StartCoroutine(ShowDebugText());
    }

    private IEnumerator ShowDebugText()
    {
        debugText.gameObject.SetActive(true);

        yield return new WaitForSeconds(debugTextDisplayDuration);

        debugText.gameObject.SetActive(false);
    }
}
