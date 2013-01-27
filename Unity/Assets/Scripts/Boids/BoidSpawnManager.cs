using System.Collections.Generic;
using UnityEngine;

public class BoidSpawnManager : BeatReceiver
{
    private static BoidSpawnManager _instance = null;
    public static BoidSpawnManager Instance
    {
        get
        {
            return _instance;
        }

        private set
        {
            if (_instance != null)
                Debug.LogError("Multiple Player singletons");
            _instance = value;
        }
    }


    public float PulseBoost = 3.0f;
    public float PulseDecay = 30.0f;

    protected float curPulseBoost = 0.0f;

    void Awake()
    {
        BoidSpawnManager.Instance = this;
    }

    void Start()
    {
        BeatManager.Instance.RegisterReceiver(this);
    }

    void Update()
    {
        this.curPulseBoost -= Time.deltaTime * PulseDecay;
        if (this.curPulseBoost < 0.0f)
            this.curPulseBoost = 0.0f;
    }

    public override void OnBeat()
    {
        this.curPulseBoost = PulseBoost;
    }

    public float SpawnCooldownTime
    {
        get { return 2 + Random.value - BeatManager.Instance.Intensity; }
    }

    public float BoidSpeedMultiplier
    {
        get { return 1 + BeatManager.Instance.Intensity + this.curPulseBoost; }
    }

    public bool HasSpawns
    {
        get 
        {
            return (spawnQueue.Count > 0); 
        }
    }

    private Queue<EnemyEnum> spawnQueue = new Queue<EnemyEnum>();

    public void AddToSpawnQueue(EnemyEnum enemyType)
    {
        spawnQueue.Enqueue(enemyType);
    }

    public EnemyEnum GetNextSpawn()
    {
        return spawnQueue.Dequeue();
    }
}
