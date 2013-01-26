using UnityEngine;

public class AudioEnergy : MonoBehaviour
{

    public static readonly float totalSampleRate = 48000;
    public static readonly float smallSampleRate = 1024;

    void Update()
    {
        float[] spectrum = new float[1024];
        audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        int i = 1;
        string s = "";

        while (i < 1024 - 1)
        {
            s += spectrum[i] + " ";
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
            i++;
        }
    }
}
