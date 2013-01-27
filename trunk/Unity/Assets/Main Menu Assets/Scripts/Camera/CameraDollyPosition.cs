using UnityEngine;
using System.Collections;

public class CameraDollyPosition : MonoBehaviour {
	
	public CameraDollyPosition nextLocation; //where does the camera go next?
	public float waitTime; //how long does the camera stay here?
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//extra stuff that can be activated at this spot
	public void SpecialActions (){
		foreach (SpecialAction act in GetComponents<SpecialAction>()){
			act.Action();
		}
	}
}
