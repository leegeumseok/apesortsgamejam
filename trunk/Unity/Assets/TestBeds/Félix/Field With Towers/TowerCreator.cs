using UnityEngine;
using System.Collections;

public class TowerCreator : MonoBehaviour {

    // Towers
    public Transform spawnLocation;
    public Transform towerTemplate;
    public int resources = 0;
    public int towerResourceCost = 10;
    public int maxResources = 15;

    public int bulletSpeed = 25;
    public int bulletDamage = 100;
    public int bulletForce = 60;

    public float attackRange = 5;
    public float attacksPerSecond = 1.75f;
    public int ammunition = 50;
	
	// Update is called once per frame
    void Update()
    {
        // Place a tower?
        if (Input.GetKeyDown("q") && resources > towerResourceCost)
        {
            Transform tower = (Transform)Instantiate(towerTemplate, spawnLocation.position, Quaternion.identity);
            GenericTower controller = tower.GetComponent<GenericTower>();
            controller.attackRange = attackRange;
            controller.bulletSpeed = bulletSpeed;
            controller.bulletDamage = bulletDamage;
            controller.bulletForce = bulletForce;
            controller.attacksPerSecond = attacksPerSecond;
            controller.ammunition = ammunition;

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
