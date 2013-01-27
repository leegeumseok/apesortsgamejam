using UnityEngine;
using System.Collections;

public class GameStatsUI : MonoBehaviour {

    public enum GameOutcome
    {
        NotOver,
        Won,
        Lost
    }

	public GUISkin customSkin;
	public int time, points, heartHealth, heartHealthMax, pulsePoints, pulsePointsMax, resources;
	public float respawnTimerCount;
    public GameOutcome outcome;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (outcome != GameOutcome.NotOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // return to main menu
            }
        }
	}
	
	void OnGUI(){
		GUI.skin = customSkin;
		
		//TOP
        if (time != 0)
        {
            GUI.Label(new Rect(0, 0, 100, 50), "Time: " + time);
        }
		
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

        if (outcome == GameOutcome.Won)
        {
            // tell the user he won
            GUI.Label(new Rect(Screen.width - 100, Screen.height - 300, Screen.width - 200, Screen.height - 600), "YOU WIN!\nPress ESC to exit");
        }
        else if (outcome == GameOutcome.Lost)
        {
            // tell the user he lost
            GUI.Label(new Rect(Screen.width - 100, Screen.height - 300, Screen.width - 200, Screen.height - 600), "YOU DIED! </3\nPress ESC to exit");
        }
        else
        {
            //RESPAWN
            if (respawnTimerCount > 0f)
            {
                GUI.Label(new Rect(Screen.width / 2 - 50f, Screen.height / 2 - 50f, 100f, 100f), "Respawn in: " + (((int)respawnTimerCount) + 1), "PointsLabel");
                respawnTimerCount -= Time.deltaTime;
            }
        }
	}
}
