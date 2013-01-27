using UnityEngine;
using System.Collections;

/// <summary>
/// oh god how did this get here i am not good with computer
/// http://archive.gamedev.net/archive/reference/programming/features/beatdetection/index.html
/// </summary>
public class NaiveSpectrumAnalyzer
{
    float beatLockout = 0.2f;
    float beatDecay = 0.0f;

    // Time windows for calculating average BPM
    float[] timeWindows;
    float secondsPerWindow = 1.0f;
    int timeWindowCount = 10;
    int timeWindowCurrent = 0;
    float timeAccumulated = 0.0f;

    // Sample windows for calculating energy
    float[] totalWindow;
    int totalWindowIndex = 0;
    int totalSampleRate = 44100;

    public NaiveSpectrumAnalyzer(int totalSampleRate)
    {
        this.totalSampleRate = totalSampleRate;
        this.totalWindow = new float[totalSampleRate];
        this.timeWindows = new float[timeWindowCount];
    }

    public bool AddSamples(float[] samples)
    {
        int sampleWindows = samples.Length;

        float energy = 0.0f;
        for (int i = 0; i < sampleWindows; i++)
        {
            energy += (samples[i] * samples[i]);
            int newIndex = (totalWindowIndex + i) % totalSampleRate;
            this.totalWindow[newIndex] = samples[i];
        }
        totalWindowIndex = 
            (totalWindowIndex + sampleWindows) % totalSampleRate;

        float totalEnergy = 0.0f;
        for (int i = 0; i < totalSampleRate; i++)
        {
            totalEnergy += (totalWindow[i] * totalWindow[i]);
        }
        totalEnergy *= (float)sampleWindows / (float)totalSampleRate;

        if (energy > 1.3f * totalEnergy && beatDecay <= 0.005f)
        {
            this.timeWindows[this.timeWindowCurrent]++;
            beatDecay = this.beatLockout;
            return true;
        }
        return false;
    }

    public void Update(float deltaTime)
    {
        timeAccumulated += deltaTime;
        if (timeAccumulated > this.secondsPerWindow)
        {
            this.timeAccumulated -= this.secondsPerWindow;
            this.timeWindowCurrent = 
                (this.timeWindowCurrent + 1) % this.timeWindowCount;
            this.timeWindows[this.timeWindowCurrent] = 0;
        }

        this.beatDecay -= deltaTime;
        if (beatDecay < 0.0f)
            beatDecay = 0.0f;
    }

    public float GetAverageBPM()
    {
        float totalBPM = 0.0f;
        for (int i = 0; i < this.timeWindowCount; i++)
        {
            if (i != this.timeWindowCurrent)
            {
                totalBPM += this.timeWindows[i];
            }
        }

        return (totalBPM / (float)this.timeWindowCount) * (60.0f);
    }
}
