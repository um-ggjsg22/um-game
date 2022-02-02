using UnityEngine;
using System.Collections;

public class PlaySFX : MonoBehaviour {

    AudioManager AMInstance;   // reference to AudioManager instance

    private void Start()
    {
        AMInstance = AudioManager.instance;
    }

    public void PlaySoundEffect(string SFXName)
    {
        if (AMInstance)     // not null
        {
            AMInstance.PlaySFX(SFXName);
        }
    }

}
