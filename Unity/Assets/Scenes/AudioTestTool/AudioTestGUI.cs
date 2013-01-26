using UnityEngine;
using System.Collections;

public class AudioTestGUI : MonoBehaviour
{
    public string stringToEdit = "C:\\Users\\Alex\\Documents\\GameJam\\trunk\\Unity\\Assets\\Audio\\deadmau5 - Raise Your Weapon.mp3";

    public SongPlayer player;
    public GUIText text;

    void OnGUI()
    {
        stringToEdit = GUI.TextArea(new Rect(50, 50, 600, 100), stringToEdit, 200);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            player.StartSong(stringToEdit);
        float averageBPM = player.AverageBPM;

        text.text = averageBPM.ToString();
    }
}