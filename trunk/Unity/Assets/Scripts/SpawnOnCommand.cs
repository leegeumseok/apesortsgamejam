using UnityEngine;
using System.Collections;

public class SpawnOnCommand : MonoBehaviour 
{

	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BoidSpawnManager.Instance.AddToSpawnQueue(EnemyEnum.Small);
            Debug.Log("Spawning small");
        }
	}
}
