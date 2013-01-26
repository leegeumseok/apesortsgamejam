using UnityEngine;
using System.Collections.Generic;

public class BoidController : MonoBehaviour
{

    private Boids boidScript;
    public float filterRange = 5;
    public LayerMask avoidanceMask = new LayerMask();
    public LayerMask goalMask = new LayerMask();
    public LayerMask mouselookMask = new LayerMask();
    public GameObject primaryGoal;
    public float acceleration;

    void Start()
    {
        boidScript = transform.GetComponent<Boids>();
        boidScript.AvoidanceRule = true;
        boidScript.GoalSeekingRule = true;
    }


    void FixedUpdate()
    {
        // Handle Avoidance Stuff
        Collider[] avoidanceColliderArray = Physics.OverlapSphere(transform.position, filterRange, avoidanceMask);
        List<GameObject> avoidanceList = new List<GameObject>();
        foreach (Collider collider in avoidanceColliderArray)
        {
            if (collider.gameObject != this.gameObject)
            {
                avoidanceList.Add(collider.gameObject);
            }
        }
        // Handle Goal Stuff
        Collider[] goalColliderArray = Physics.OverlapSphere(transform.position, filterRange, goalMask);
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

        // Add Acceleration to Rigidbody
        if (boidScript.CurrentVelocity.sqrMagnitude > 0 && rigidbody != null)
        {
            Vector3 newVelocity = boidScript.CurrentVelocity;
            newVelocity.y = 0;
            Vector3 newForce = Vector3.Normalize(newVelocity) * acceleration;
            transform.rigidbody.AddForce(newForce, ForceMode.Acceleration);
        }
    }

    Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit, 1000, mouselookMask);
        Vector3 answer = hit.point;
        answer.y = 0;
        return answer;
    }
}
