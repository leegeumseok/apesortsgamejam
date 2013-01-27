using UnityEngine;
using System.Collections;

public class AudioTestGUI : MonoBehaviour
{
    public string stringToEdit = "C:\\Users\\Alex\\Documents\\GameJam\\trunk\\Unity\\Assets\\Audio\\deadmau5 - Raise Your Weapon.mp3";
    public GUIText text;

    void OnGUI()
    {
        stringToEdit = GUI.TextArea(new Rect(50, 50, 600, 100), stringToEdit, 200);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SongPlayer.Instance.PlaySong(stringToEdit);
        float averageBPM = SongPlayer.Instance.AverageBPM;
        float intensity = BeatManager.Instance.Intensity;

        if (intensity < 0.33f)
            text.material.color = Color.green;
        else if (intensity < 0.66f)
            text.material.color = Color.yellow;
        else
            text.material.color = Color.red;
        text.text = intensity.ToString();

    }
}