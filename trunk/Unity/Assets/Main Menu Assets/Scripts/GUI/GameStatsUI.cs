using UnityEngine;
using System.Collections;

public class GameStatsUI : MonoBehaviour {
	public GUISkin customSkin;
	public int time, points, heartHealth, heartHealthMax, pulsePoints, pulsePointsMax, resources;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		GUI.skin = customSkin;
		
		//TOP
		GUI.Label(new Rect(0,0,100,50), "Time: " + time);
		GUI.Label(new Rect(Screen.width/2 - 100,0,200,50), points + " points", "PointsLabel");
		
		//SIDE
		GUI.Label(new Rect(Screen.width - 100f, Screen.height*0.25f,100f,50f), "Heart: " + heartHealth + "/" + heartHealthMax, "HeartLabel");
		if (pulsePoints >= pulsePointsMax){
			GUI.Label(new Rect(Screen.width - 100f, Screen.height*0.5f,100f,80f), "Pulse: PRESS SPACE!", "PulseLabel");
		}
		else{
			GUI.Label(new Rect(Screen.width - 100f, Screen.height*0.5f,100f,50f), "Pulse: " + pulsePoints + "/" + pulsePointsMax);
		}
		GUI.Label(new Rect(Screen.width - 100f, Screen.height*0.75f,100f,50f), "Resources: " + resources);
	}
}
