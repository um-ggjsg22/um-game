using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private float delay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DelayedChangeScene(string sceneName)
    {
        StartCoroutine(ChangeSceneDelayed(sceneName));
    }

    private IEnumerator ChangeSceneDelayed(string sceneName)
    {
        yield return new WaitForSeconds(delay);
        ChangeScene(sceneName);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
