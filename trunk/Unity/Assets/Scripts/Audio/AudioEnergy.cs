using UnityEngine;
using BPMDetect;

// http://archive.gamedev.net/archive/reference/programming/features/beatdetection/index.html

public class AudioEnergy : MonoBehaviour
{
    public static readonly int smallSampleRate = 2048;
    int totalSampleRate;

    public AudioClip clip;
    public Shrink shrink;
    float timeStep = 0.0f;
    float nextWindow = 0.0f;

    float[] totalWindow;
    int totalWindowIndex = 0;

    void Start()
    {
        this.totalSampleRate = clip.frequency;
        this.timeStep = (float)smallSampleRate / (float)totalSampleRate;
        this.totalWindow = new float[totalSampleRate];

        for (int i = 0; i < totalWindowIndex; i++)
            this.totalWindow[i] = 0;
    }

    void FixedUpdate()
    {
        float[] spectrum = new float[smallSampleRate];
        audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        if (Time.time > nextWindow)
        {
            float energy = 0.0f;
            for (int i = 0; i < smallSampleRate; i++)
            {
                energy += (spectrum[i] * spectrum[i]);
                this.totalWindow[(totalWindowIndex + i) % totalSampleRate] = spectrum[i];
            }
            totalWindowIndex = (totalWindowIndex + smallSampleRate) % totalSampleRate;

            float totalEnergy = 0.0f;
            for (int i = 0; i < totalSampleRate; i++)
            {
                totalEnergy += (totalWindow[i] * totalWindow[i]);
            }
            totalEnergy *= (float)smallSampleRate / (float)totalSampleRate;

            if (energy > 1.3f * totalEnergy)
            {
                shrink.scale = 2.0f;
            }

            nextWindow += timeStep;
        }
        else
        {
        }

        for (int i = 1; i < smallSampleRate - 1; i++)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
        }


    }
}
