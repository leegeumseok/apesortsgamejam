using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BeatManager : MonoBehaviour 
{
    public static readonly float MIN_BPM = 60.0f;
    public static readonly float MAX_BPM = 120.0f;

    public static readonly float MIN_INTENSITY = 0.01f;
    public static readonly float MAX_INTENSITY = 0.99f;

    private static BeatManager _instance = null;
    public static BeatManager Instance
    {
        get
        {
            return _instance;
        }

        private set
        {
            if (_instance != null)
                Debug.LogError("Multiple IntensityManager singletons");
            _instance = value;
        }
    }

    protected class BeatDecay
    {
        public BeatReceiver receiver;
        public float curDecay;
        public float maxDecay;

        public BeatDecay(BeatReceiver receiver, float maxDecay)
        {
            this.receiver = receiver;
            this.maxDecay = maxDecay;
            this.curDecay = 0.0f;
        }
    }

    private List<BeatDecay> receivers;
    private List<Action> delayedActions;

    public float Intensity
    {
        get
        {
            float intensity = 
                (SongPlayer.Instance.AverageBPM - MIN_BPM) / MAX_BPM;
            return Mathf.Clamp(intensity, MIN_INTENSITY, MAX_INTENSITY);
        }
    }

    void Awake()
    {
        BeatManager.Instance = this;
        this.receivers = new List<BeatDecay>();
        this.delayedActions = new List<Action>();
    }

    void Update()
    {
        foreach (BeatDecay decay in this.receivers)
        {
            decay.curDecay -= Time.deltaTime;
            if (decay.curDecay < 0.0f)
                decay.curDecay = 0.0f;
        }
    }

    public void RegisterReceiver(BeatReceiver receiver, float maxDecay = 0.0f)
    {
        this.receivers.Add(new BeatDecay(receiver, maxDecay));
    }

    public void DelayUntilNextBeat(Action action)
    {
        this.delayedActions.Add(action);
    }

    public void DoBeat()
    {
        foreach (BeatDecay decay in this.receivers)
        {
            if (decay.curDecay <= 0.001f)
            {
                decay.receiver.OnBeat();
                decay.curDecay = decay.maxDecay;
            }
        }

        foreach (Action action in this.delayedActions)
            action.Invoke();
        this.delayedActions.Clear();
    }
}
