using UnityEngine;
using System.Collections;

public class ActivateSongMenu : SpecialAction {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	override public void Action(){
		GetComponent<SongFileFinder>().isVisible = true;
	}
}
