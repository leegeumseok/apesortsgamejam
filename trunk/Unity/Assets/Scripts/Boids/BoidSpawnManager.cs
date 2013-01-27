using System.Collections.Generic;
using UnityEngine;

public static class BoidSpawnManager
{
    public static float SpawnCooldownTime
    {
        get { return 2 + Random.value; }//- BeatManager.Instance.Intensity; }
    }

    public static float BoidSpeedMultiplier
    {
        get { return 1; }//+ BeatManager.Instance.Intensity; }
    }

    public static bool HasSpawns
    {
        get 
        {
            return (spawnQueue.Count > 0); 
        }
    }

    private static Queue<EnemyEnum> spawnQueue = new Queue<EnemyEnum>();

    public static void AddToSpawnQueue(EnemyEnum enemyType)
    {
        spawnQueue.Enqueue(enemyType);
    }

    public static EnemyEnum GetNextSpawn()
    {
        return spawnQueue.Dequeue();
    }
}
