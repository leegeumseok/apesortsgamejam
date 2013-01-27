using UnityEngine;
using System.Collections;

public class BoidSpawner : MonoBehaviour {

    
    public float spawnCooldown = 2;
    private float currentCooldown = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        currentCooldown -= Time.deltaTime;
	}


}
