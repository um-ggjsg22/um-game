using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct SetAudioClip
{
    public string name;
    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour {

    public AudioSource SFXPrefab;   // to be used for initialising a new AudioSource
    public AudioSource musicPlayer;     // to instantiate conveniently in inspector

    public static AudioManager instance = null;

    // music clips
    public SetAudioClip[] musicClipsSerialize;  // to serialize in inspector
    static Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>(); // to access conveniently via name

    // SFX clips
    public SetAudioClip[] SFXClipsSerialize;       // to serialize in inspector
    static Dictionary<string, AudioClip> SFXClips = new Dictionary<string, AudioClip>();   // to access conveniently via name

    public static float SFXVolume = 1f;
    static string currentBGMName = "";  // name of currently playing BGM

    [SerializeField]
    private float sfxVolume = 1f;
    [SerializeField]
    private float bgmVolume = 1f;

    private void Awake()
    {
        if (instance != null)   // instance of AudioManager already exists
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        // add music clips to dictionary
        for (int i = 0; i < musicClipsSerialize.Length; ++i)
        {
            musicClips.Add(musicClipsSerialize[i].name, musicClipsSerialize[i].audioClip);
        }
        // add SFX clips to dictionary
        for (int i = 0; i < SFXClipsSerialize.Length; ++i)
        {
            SFXClips.Add(SFXClipsSerialize[i].name, SFXClipsSerialize[i].audioClip);
        }

        // play BGM
        SetBGMVolume(bgmVolume);
        
        SetSFXVolume(sfxVolume);

        DontDestroyOnLoad(this.gameObject);
    }

	// Update is called once per frame
	void Update () {
	
        // can iterate through GO children of this AudioManager here to do things
        // the GO children will be temporary SFX AudioSources
	}

    // generic
    public void PlayMusic(string key)
    {
        if (key == currentBGMName)  // this BGM is already playing
            return;

        if (musicClips.ContainsKey(key))
        {
            musicPlayer.clip = musicClips[key];
            musicPlayer.Play();
            currentBGMName = key;
        }
    }

    public float PlaySFX(string key)
    {
        if (SFXClips.ContainsKey(key))
        {
            // create a temp AudioSource GameObject; it'll destroy itself after its length
            AudioSource SFXAudioSource = Instantiate(SFXPrefab);
            SFXAudioSource.clip = SFXClips[key];
            //SFXAudioSource.volume = SettingsData.GetSFXVolumeRange();
            SFXAudioSource.volume = SFXVolume;
            SFXAudioSource.Play();
            Destroy(SFXAudioSource.gameObject, SFXAudioSource.clip.length);

            // newGO.volume = SettingsData.GetSFXVolumeRange();
            // newGO.Play();
            // Destroy(newGO, clip.Length);

            return SFXAudioSource.clip.length;
        }

        return 0f;
    }

    public void PlaySFXVoid(string key)
    {
        if (SFXClips.ContainsKey(key))
        {
            // create a temp AudioSource GameObject; it'll destroy itself after its length
            AudioSource SFXAudioSource = Instantiate(SFXPrefab);
            SFXAudioSource.clip = SFXClips[key];
            //SFXAudioSource.volume = SettingsData.GetSFXVolumeRange();
            SFXAudioSource.volume = SFXVolume;
            SFXAudioSource.Play();
            Destroy(SFXAudioSource.gameObject, SFXAudioSource.clip.length);
        }
    }

    // Set Volume
    public void SetBGMVolume(float _volume)
    {
        musicPlayer.volume = _volume;
    }
    public void SetSFXVolume(float _volume)
    {
        SFXVolume = _volume;
    }
    
    /*// Play Music
    public void PlayMenuBGM()
    {
        PlayMusic("MenuBGM");
    }

    public void PlayGameBGM()
    {
        PlayMusic("GameBGM");
    }

    // Play SFX
    public void PlayButtonMouseover()
    {
        PlaySFX("ButtonMouseover");
    }

    public void PlayButtonPress()
    {
        PlaySFX("ButtonPress");
    }

    public void PlayWaterSplashSFX()
    {
        PlaySFX("WaterSplash");
    }

    public void PlayFireballReleaseSFX()
    {
        PlaySFX("FireballRelease");
    }

    public void PlayChargedFireballReleaseSFX()
    {
        PlaySFX("ChargedRelease");
    }

    public void PlayFireballHitGroundSFX()
    {
        PlaySFX("FireballHit");
    }*/

}
