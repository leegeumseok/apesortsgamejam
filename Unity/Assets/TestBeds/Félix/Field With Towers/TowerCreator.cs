using UnityEngine;
using System.Collections;

public class TowerCreator : MonoBehaviour {

    // Towers
    public Transform spawnLocation;
    public Transform towerTemplate;
    public int resources = 0;
    public int towerResourceCost = 10;
    public int maxResources = 15;
	
	// Update is called once per frame
    void Update()
    {
        // Place a tower?
        if (Input.GetKeyDown("q") && resources > towerResourceCost)
        {
            Transform tower = (Transform)Instantiate(towerTemplate, spawnLocation.position, Quaternion.identity);
            GenericTower controller = tower.GetComponent<GenericTower>();
            controller.bulletSpeed = 10;
            controller.bulletDamage = 100;
            controller.bulletForce = 60;
            controller.attacksPerSecond = 1.75f;
            controller.ammunition = 50;

            resources -= towerResourceCost;
        }
	}

    public void GatherResources(int resources)
    {
        this.resources += resources;
        if (this.resources > maxResources)
            this.resources = maxResources;
    }
}
