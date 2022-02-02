using UnityEngine;
using System.Collections;

public class PlayBGM : MonoBehaviour {

    public string BGMName;

	// Use this for initialization
	void Start () {
        AudioManager.instance.PlayMusic(BGMName);
	}

}
