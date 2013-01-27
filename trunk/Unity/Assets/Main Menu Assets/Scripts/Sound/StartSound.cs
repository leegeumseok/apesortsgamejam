using UnityEngine;
using System.Collections;

public class StartSound : SpecialAction {
	public AudioSource sound;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	override public void Action(){
		if (!sound.isPlaying){
			sound.Play();
		}
	}
}
