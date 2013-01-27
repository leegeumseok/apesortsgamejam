using UnityEngine;
using System.Collections;

public class OnReturnToMainMenuSkipIntro : MonoBehaviour {
	private bool beenHereBefore;
	
	// Use this for initialization
	void Start () {
		beenHereBefore = false;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject mc = GameObject.FindGameObjectWithTag("MainCamera");
		if (mc != null){
			SkipToMainMenu skipper = mc.GetComponent<SkipToMainMenu>();
			if (skipper != null){ //if you get here you know it's the main menu! (we hope)
				if (beenHereBefore){
					skipper.Skip(); //skip to the main section of the main menu
					Destroy(this.gameObject); //destroy this object (it holds the song information too)
				}
				else{
					beenHereBefore = true;
				}
			}
		}
	}
}
