using UnityEngine;
using System.Collections;

public class PlaySomething : MonoBehaviour 
{
    public string pathToSong = "C:\\Temp\\blackdragon.mp3";

	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SongPlayer.Instance.PlaySong(pathToSong);
        }
	}
}
