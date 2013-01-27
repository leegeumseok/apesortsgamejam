using UnityEngine;
using System.Collections;

public class PlaySomething : MonoBehaviour 
{
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SongPlayer.Instance.PlaySong("C:\\Users\\Alex\\Downloads\\Hotline Miami Soundtrack -  Perturbator.mp3");
        }
	}
}
