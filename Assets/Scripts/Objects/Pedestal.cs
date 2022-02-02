using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    [SerializeField]
    private GameObject will;

    // Start is called before the first frame update
    void Start()
    {
        will.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            will.SetActive(true);
        }
    }
}
