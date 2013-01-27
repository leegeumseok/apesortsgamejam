using UnityEngine;
using System.Collections.Generic;

public class BoidController : MonoBehaviour
{

    private Boids boidScript;
    public float avoidanceFilterRange = 10;
    public LayerMask avoidanceMask = new LayerMask();
    public LayerMask goalMask = new LayerMask();
    public GameObject primaryGoal;
    public float acceleration = 10;
    public bool deathOnContact;

    void Start()
    {
        boidScript = transform.GetComponent<Boids>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (deathOnContact && gameObject.layer != collision.gameObject.layer)

            Destroy(gameObject);
    }

    void Update()
    {

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
        foreach (Collider collider in goalColliderArray)
        {
            if (collider.gameObject != this.gameObject)
            {
                goalList.Add(collider.gameObject);
            }
        }
        if (primaryGoal != null)
        {
            goalList.Add(primaryGoal);
        }
        // Set Up BoidScript
        boidScript.GoalList = goalList;
        boidScript.AvoidanceList = avoidanceList;
        boidScript.UpdateVelocity();

        // Add Force to Rigidbody
        if (boidScript.CurrentVelocity.sqrMagnitude > 0 && rigidbody != null)
        {
            Vector3 newVelocity = boidScript.CurrentVelocity;
            newVelocity.y = 0;
            Vector3 newForce = Vector3.Normalize(newVelocity) * acceleration * GlobalVariables.speedMultiplier / rigidbody.mass;
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
