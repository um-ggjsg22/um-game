using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    private float panelHeight = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        panelHeight = transform.GetComponent<RectTransform>().sizeDelta.y;
    }

    public void ShowPanel()
    {
        transform.position = new Vector3(transform.position.x, 0.5f * panelHeight, 0f);
    }
    public void HidePanel()
    {
        transform.position = new Vector3(transform.position.x, -0.5f * panelHeight, 0f);
    }
}
