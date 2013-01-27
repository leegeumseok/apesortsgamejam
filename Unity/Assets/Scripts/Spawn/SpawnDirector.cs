using UnityEngine;
using System.Collections;

public class SpawnDirector : MonoBehaviour 
{
    public Random random;

    public float AvgTimeInterval = 6.0f;
    public float TimeRandomEffect = 1.0f;
    public float TimeIntensityEffect = 2.0f;
    public float NextSpawnTime = 0.0f;

    public float AvgSpawnCount = 7.0f;
    public float CountIntensityEffect = 5.0f;
    public float CountRandomEffect = 1.0f;

    public float EnemyTypeIntensityEffect = 3.0f;
    public float EnemyTypeRandomEffect = 1.0f;

    void Start()
    {
        this.random = new Random();
    }

	void Update () 
    {
        if (SongPlayer.Instance.IsPlaying == true 
            && Time.time > NextSpawnTime)
        {
            this.DoSpawn();
            NextSpawnTime += this.CalcNextSpawnTime();
        }
	}

    protected float CalcNextSpawnTime()
    {
        float intensity = BeatManager.Instance.Intensity;
        float intensityRange = this.TimeIntensityEffect * 2.0f;
        float modifiedRange = intensityRange * intensity;

        float randRange = 
            Random.Range(-this.TimeRandomEffect, this.TimeRandomEffect);

        return AvgTimeInterval - modifiedRange + randRange;
    }

    protected int CalcSpawnCount()
    {
        float intensity = BeatManager.Instance.Intensity;
        float intensityRange = this.CountIntensityEffect * 2.0f;
        float modifiedRange = intensityRange * intensity;

        float randRange =
            Random.Range(-this.CountRandomEffect, this.CountRandomEffect);

        return (int)(AvgSpawnCount + modifiedRange + randRange + 0.5f);
    }

    protected float BaddieChance()
    {
        float intensity = BeatManager.Instance.Intensity;
        float intensityRange = this.EnemyTypeIntensityEffect * intensity;

        float rand =
            Random.Range(0, this.EnemyTypeRandomEffect);
        float randRange = this.EnemyTypeRandomEffect * rand;

        float totalDivisor = 
            this.EnemyTypeIntensityEffect + this.EnemyTypeRandomEffect;

        float finalRange = (intensityRange + randRange) / (totalDivisor);
        return finalRange;
    }

    protected EnemyEnum CalculateEnemyType()
    {
        int SpawnCount = this.CalcSpawnCount();
        float baddieChance = this.BaddieChance();

        EnemyEnum enemyType = EnemyEnum.Small;
        if (baddieChance < 0.33f)
            enemyType = EnemyEnum.Small;
        else if (baddieChance < 0.66f)
            enemyType = EnemyEnum.Medium;
        else
            enemyType = EnemyEnum.Big;

        return enemyType;
    }

    void DoSpawn()
    {
        int enemyCount = this.CalcSpawnCount();
        string s = "";
        for (int i = 0; i < enemyCount; i++)
        {
            EnemyEnum type = this.CalculateEnemyType();
            BoidSpawnManager.Instance.AddToSpawnQueue(type);
            s += type.ToString() + " ";
        }
        Debug.Log(BeatManager.Instance.Intensity + ": " + enemyCount + " " + s);
    }
}
