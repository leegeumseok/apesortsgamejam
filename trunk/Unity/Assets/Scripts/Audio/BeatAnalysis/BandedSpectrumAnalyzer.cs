using UnityEngine;
using System.Collections;

// This is bad, don't use it. <3 Alex
public class BandedSpectrumAnalyzer
{
    int smallSampleWindow = 1024;
    int sampleHistory = 43;
    int subBands = 32;

    float[][] historyBands;
    int historyIndex = 0;

    public BandedSpectrumAnalyzer()
    {
        historyBands = new float[sampleHistory][];
        for (int i = 0; i < sampleHistory; i++)
            historyBands[i] = new float[subBands];
        for (int i = 0; i < sampleHistory; i++)
            for (int j = 0; j < subBands; j++)
                historyBands[i][j] = 0.0f;
    }

    public bool AddSamples(float[] samples)
    {
        float[] subBandData = new float[subBands];

        for (int i = 0; i < subBands; i++)
        {
            float bandSum = 0.0f;
            for (int j = 0; j < subBands; j++)
            {
                bandSum = samples[(i * subBands) + j];
            }
            subBandData[i] = bandSum / (float)subBands;
        }

        float[] totalEnergy = ComputeBandEnergy();

        int total = 0;
        for (int i = 28; i >= 21; i--)
            if (subBandData[i] > 1.3f * totalEnergy[i])
                total++;

        AddToHistory(subBandData);
        return (total >= 7);
    }

    void AddToHistory(float[] bands)
    {
        for (int i = 0; i < 32; i++)
            historyBands[historyIndex][i] = bands[i];
        historyIndex = (historyIndex + 1) % sampleHistory;
    }

    public float[] ComputeBandEnergy()
    {
        float[] bandEnergy = new float[subBands];
        for (int i = 0; i < subBands; i++)
        {
            float sum = 0.0f;
            for (int j = 0; j < sampleHistory; j++)
            {
                sum += historyBands[j][i];
            }
            bandEnergy[i] = sum / (float)sampleHistory;
        }
        return bandEnergy;
    }
}
