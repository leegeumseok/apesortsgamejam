using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour {

    public int currentScore;

	// Use this for initialization
	void Start () {
        BoidSpawnManager.Instance.notifyBoidCreated += OnEnemyCreated;
	}

    private void OnEnemyCreated(BoidController obj)
    {
        GenericEnemy enemy = obj.GetComponent<GenericEnemy>();
        if (enemy != null)
        {
            enemy.notifyOnDestroyed += OnEnemyKilled;
        }
    }

    private void OnEnemyKilled(GenericEnemy enemy)
    {
        currentScore += enemy.killPoints;
        enemy.notifyOnDestroyed -= OnEnemyKilled;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
