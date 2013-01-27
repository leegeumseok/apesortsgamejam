/******************************************************************
Example script for using the FMOD EX low level sound system.
Initializes the FMOD sound system and loads and plays some sounds.
Visit www.squaretangle.com for updates and more information.
Enjoy.
johnny tangle
********************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SongPlayer : MonoBehaviour
{
    private static SongPlayer _instance = null;
    public static SongPlayer Instance
    {
        get
        {
            return _instance;
        }

        private set
        {
            if (_instance != null)
                Debug.LogError("Multiple SongPlayer singletons");
            _instance = value;
        }
    }

    float timeStep = 0.0f;
    float nextWindow = 0.0f;
    int windowSize = 1024;
    float frequency;

    public NaiveSpectrumAnalyzer analyzer = null;
    public float AverageBPM 
    {
        get
        {
            if (this.analyzer != null)
            {
                return this.analyzer.GetAverageBPM();
            }
            return -1.0f;
        }
    }

    private FMOD.System system = null;
    private FMOD.Sound sound1 = null;
    private FMOD.Channel channel = null;

    void OnEnable()
    {
        SongPlayer.Instance = this;
    }

    void Start()
    {
        uint version = 0;
        FMOD.RESULT result;

        //Create an FMOD System object            
        result = FMOD.Factory.System_Create(ref system);
        if (!ERRCHECK(result)) return;

        //Check FMOD Version
        result = system.getVersion(ref version);
        if (!ERRCHECK(result)) return;

        if (version < FMOD.VERSION.number)
        {
            Debug.Log(
                "Error!  You are using an old version of FMOD " 
                + version.ToString("X") 
                + ".  This program requires " 
                + FMOD.VERSION.number.ToString("X") + ".");
        }

        //Initialize the FMOD system object
        result = system.init(32, FMOD.INITFLAGS.NORMAL, (IntPtr)null);
        if (!ERRCHECK(result)) return;

        if (result == FMOD.RESULT.OK)
        {
            Debug.Log("FMOD init! " + result);
        }
    }

    void Update()
    {
        if (analyzer != null)
        {
            analyzer.Update(Time.deltaTime);
            //Debug.Log(analyzer.GetAverageBPM());
        }
    }

    public void PlaySong(string songPath)
    {
        //Create some sound references to play
        FMOD.RESULT result = 
            system.createSound(songPath, FMOD.MODE.HARDWARE, ref sound1);
        if (!ERRCHECK(result))
        {
            Debug.LogError("Could not load file " + songPath);
            return;
        }

        result = system.playSound(
            FMOD.CHANNELINDEX.FREE, sound1, false, ref channel);
        if (!ERRCHECK(result))
        {
            Debug.LogError("Could not play sound on free channel");
            return;
        }

        frequency = 0.0f;
        this.channel.getFrequency(ref frequency);
        this.analyzer = new NaiveSpectrumAnalyzer((int)frequency);

        timeStep = 1.0f / (frequency / (float)windowSize);
        Debug.Log(frequency);
        Debug.Log(timeStep);
    }

    void OnDisable()
    {
        //Shut down
        FMOD.RESULT result;

        if (sound1 != null)
        {
            result = sound1.release();
            if (!ERRCHECK(result)) return;
        }

        if (system != null)
        {
            result = system.close();
            if (!ERRCHECK(result)) return;
            result = system.release();
            if (!ERRCHECK(result)) return;

            Debug.Log("FMOD release! " + result);
        }
    }

    void FixedUpdate()
    {
        if (channel != null && Time.time > nextWindow)
        {
            float[] spectrum = new float[windowSize];
            //channel.getWaveData(spectrum, sampleWindow, 0);
            channel.getSpectrum(
                spectrum, windowSize, 0, FMOD.DSP_FFT_WINDOW.BLACKMANHARRIS);

            float[] subspectrum = new float[1024];
            for (int i = 0; i < 1024; i++)
                subspectrum[i] = spectrum[i];

            if (analyzer.AddSamples(subspectrum) == true)
                BeatManager.Instance.DoBeat();

            nextWindow += timeStep;
        }
    }

    //FMOD error checking codes
    bool ERRCHECK(FMOD.RESULT result)
    {
        if (result != FMOD.RESULT.OK)
        {
            Debug.Log(
                "FMOD error! " 
                + result 
                + " - " 
                + FMOD.Error.String(result));
            return false;
        }
        return true;
    }
}

