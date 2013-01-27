using UnityEngine;

public class BoidSpawner : MonoBehaviour {

    public GameObject BigEnemy;
    public GameObject MediumEnemy;
    public GameObject SmallEnemy;

    public Sphinctercontrol sphincter = null;
    public float closeSphincter = 0.0f;

    public GameObject defaultGoal;

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

        if (closeSphincter > 0.0f && Time.time > closeSphincter && sphincter != null)
        {
            sphincter.Close();
            closeSphincter = -1.0f;
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
        spawnedMinion.SendMessage("OnSpawn");

        BoidController controller = spawnedMinion.GetComponent<BoidController>();
        BoidSpawnManager.Instance.OnBoidCreated(controller);
        if (defaultGoal)
            controller.defaultGoal = defaultGoal;

        if (sphincter != null)
        {
            sphincter.Open();
            this.closeSphincter = Time.time + 2.0f;
        }
    }
}
