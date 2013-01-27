using UnityEngine;
using System.Collections;

public class ActivateMessage : SpecialAction {
	public GameObject message;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	override public void Action(){
		message.renderer.enabled = true;
	}
}
