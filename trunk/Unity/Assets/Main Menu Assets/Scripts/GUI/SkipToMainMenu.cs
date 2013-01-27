using UnityEngine;
using System.Collections;

public class SkipToMainMenu : MonoBehaviour {
	public CameraDollyPosition mainMenuLocation;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Escape)){
			Skip();
		}
	}
	
	public void Skip(){
		this.transform.position = mainMenuLocation.transform.position;
		this.GetComponent<CameraDollyMovement>().setNewTarget(mainMenuLocation);
	}
}
