using UnityEngine;
using System.Collections;

public class KeyPressSong : MonoBehaviour 
{
    public SongPlayer songPlayer;

	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.P) == true)
            songPlayer.StartSong("Assets/Audio/Seventeen Years-Ratatat.mp3");
	}
}
