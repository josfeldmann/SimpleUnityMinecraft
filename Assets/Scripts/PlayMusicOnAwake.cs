using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnAwake : MonoBehaviour
{
    public string trackName;

    public void Update()
    {
        AudioManager.PlayTrack(trackName);
        Destroy(this);
    }


}
