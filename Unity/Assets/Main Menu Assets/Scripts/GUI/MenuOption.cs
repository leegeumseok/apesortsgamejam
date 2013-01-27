using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class MenuOption : MonoBehaviour {
	public GameObject cursor;
	public GameObject cursorPosition;
	public CameraDollyPosition whereTheCameraGoesOnClick;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnMouseEnter(){
		renderer.material.color = Color.red;
		if (cursor != null){
			cursor.transform.position = cursorPosition.transform.position;
		}
	}
	
	void OnMouseExit(){
		renderer.material.color = Color.white;
	}
	
	void OnMouseDown(){
		//this is a bad way to do this: I have a better way - "special actions"
		//but I made those later and I didnt' want to update yet
		if (whereTheCameraGoesOnClick != null){
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraDollyMovement>().setNewTarget(whereTheCameraGoesOnClick);
		}
		//sound fx
		if (this.audio != null){
			this.audio.Play();
		}
		//activate all special actions
		foreach (SpecialAction act in GetComponents<SpecialAction>()){
			act.Action();
		}
	}
	
}
