using UnityEngine;

public class BoidSpawner : MonoBehaviour {

    public GameObject BigEnemy;
    public GameObject MediumEnemy;
    public GameObject SmallEnemy;

    public GameObject primaryGoal;

    private float currentCooldown = 0;
    private EnemyEnum enemyToSpawn;
    private bool spawningEnemy = false;
	
	// Update is called once per frame
	void Update () 
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0 && BoidSpawnManager.Instance.HasSpawns && !spawningEnemy)
        {
            currentCooldown = BoidSpawnManager.Instance.SpawnCooldownTime;
            spawningEnemy = true;
            enemyToSpawn = BoidSpawnManager.Instance.GetNextSpawn();
            BeatManager.Instance.DelayUntilNextBeat(this.SpawnNext);
        }
	}

    public void SpawnNext()
    {
        GameObject spawn = null;
        switch(enemyToSpawn)
        {
            case EnemyEnum.Big:
                spawn = BigEnemy;
                break;
            case EnemyEnum.Medium:
                spawn = MediumEnemy;
                break;
            case EnemyEnum.Small:
                spawn = SmallEnemy;
                break;
        }
        spawningEnemy = false;
        GameObject spawnedMinion = (GameObject)Instantiate(spawn, transform.position, Quaternion.identity);
        if (primaryGoal)
        {
            spawnedMinion.GetComponent<BoidController>().primaryGoal = primaryGoal;
        }
    }
}
