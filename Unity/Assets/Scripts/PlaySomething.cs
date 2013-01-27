using UnityEngine;
using System.Collections;

public class PlaySomething : MonoBehaviour 
{
    public string pathToSong = "C:\\Users\\Alex\\Downloads\\Hotline Miami Soundtrack -  Perturbator.mp3";

	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SongPlayer.Instance.PlaySong(pathToSong);
        }
	}
}
