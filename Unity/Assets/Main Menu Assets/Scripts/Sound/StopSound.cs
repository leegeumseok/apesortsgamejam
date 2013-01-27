using UnityEngine;
using System.Collections;

public class StopSound : SpecialAction {
	
	public AudioSource sound;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	override public void Action(){
		sound.Stop();
	}
}
