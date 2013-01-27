using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour {

    private GameStatsUI overlay;
    public int currentScore;

	// Use this for initialization
	void Start () {
        BoidSpawnManager.Instance.notifyBoidCreated += OnEnemyCreated;

        var overlays = GameObject.FindGameObjectsWithTag("MainCamera");
        if (overlays.Length > 0)
        {
            overlay = overlays[0].GetComponent<GameStatsUI>();
        }
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

        if (overlay != null)
        {
            overlay.points = currentScore;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
