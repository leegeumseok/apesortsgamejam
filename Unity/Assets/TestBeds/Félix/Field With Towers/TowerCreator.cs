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
            TowerController controller = tower.GetComponent<TowerController>();
            controller.bulletSpeed = 4;
            controller.bulletDamage = 0;
            controller.bulletForce = 100;

            resources -= towerResourceCost;
        }
	}

    void GatherResources(int resources)
    {
        this.resources += resources;
        if (this.resources > maxResources)
            this.resources = maxResources;
    }
}
