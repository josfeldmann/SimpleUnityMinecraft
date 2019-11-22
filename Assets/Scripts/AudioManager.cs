using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Animations;

[System.Serializable]
public class StringClipPair
{
    public string name;
    public AudioClip clip;
}


public class AudioManager :MyUnitySingleton
{
    public const string mainMenuTrack = "MainMenu";

    private static AudioManager instance;
    private static float musicVolume = 1;
    [SerializeField] private Dictionary<String, AudioClip> music;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private List<StringClipPair> clipPairing;
    public void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        transform.SetParent(null);
        GenerateDictionary();
    }

    public void GenerateDictionary()
    {

        music = new Dictionary<string, AudioClip>();
        foreach (StringClipPair s in clipPairing  )
        {
            music.Add(s.name, s.clip);
        }

    }

    public static void PlayTrack(String trackName)
    {
        if (!instance.music.ContainsKey(trackName))
        {
            Debug.Log("No track found with name: " + trackName);
            return;
        }
        instance.musicPlayer.clip = instance.music[trackName];
        instance.musicPlayer.volume = musicVolume;
        instance.musicPlayer.Play();
    }

    public static void SetVolume(float vol)
    {
        musicVolume = vol;
        instance.musicPlayer.volume = vol;
    }
    






}
