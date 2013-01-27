using UnityEngine;
using System.Collections.Generic;

public class BoidController : MonoBehaviour
{

    private Boids boidScript;
    public float avoidanceFilterRange = 10;
    public LayerMask avoidanceMask = new LayerMask();
    public LayerMask goalMask = new LayerMask();
    public GameObject defaultGoal;
    public float acceleration = 10;
    public bool deathOnContact = false;

    void Start()
    {
        boidScript = transform.GetComponent<Boids>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (deathOnContact && gameObject.layer != collision.gameObject.layer && deathOnContact)
        {
            BoidSpawnManager.Instance.AddToSpawnQueue(EnemyEnum.Big);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // Handle Avoidance Stuff
        Collider[] avoidanceColliderArray = Physics.OverlapSphere(transform.position, avoidanceFilterRange, avoidanceMask);
        List<GameObject> avoidanceList = new List<GameObject>();
        foreach (Collider collider in avoidanceColliderArray)
        {
            if (collider.gameObject != this.gameObject)
            {
                avoidanceList.Add(collider.gameObject);
            }
        }
        // Handle Goal Stuff
        Collider[] goalColliderArray = Physics.OverlapSphere(transform.position, avoidanceFilterRange, goalMask);
        List<GameObject> goalList = new List<GameObject>();
        if (defaultGoal != null && goalColliderArray.Length == 0)
        {
            goalList.Add(defaultGoal);
        }
        else
        {
            foreach (Collider collider in goalColliderArray)
            {
                GameObject collidedObject = collider.gameObject;
                if (collidedObject != this.gameObject
                    && collidedObject != defaultGoal)
                {
                    if (Player.Instance.gameObject == collidedObject)
                    {
                        if (Player.Instance.Alive)
                        {
                            goalList.Add(collider.gameObject);
                        }
                    }
                    else
                    {
                        goalList.Add(collider.gameObject);
                    }
                }
            }
        }

        // Set Up BoidScript
        boidScript.GoalList = goalList;
        boidScript.AvoidanceList = avoidanceList;
        Vector3 boidVelocity = boidScript.UpdateVelocity();

        // Add Force to Rigidbody
        if (boidVelocity.sqrMagnitude > 0 && rigidbody != null)
        {
            boidVelocity.y = 0;
            Vector3 newForce =
                Vector3.Normalize(boidVelocity) 
                * acceleration 
                * BoidSpawnManager.Instance.BoidSpeedMultiplier 
                / rigidbody.mass;
            transform.rigidbody.AddForce(newForce, ForceMode.Force);
        }
    }

    /*
    public LayerMask mouselookMask = new LayerMask();
    
    Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit, 1000, mouselookMask);
        Vector3 answer = hit.point;
        answer.y = 0;
        return answer;
    }
     * */
}
