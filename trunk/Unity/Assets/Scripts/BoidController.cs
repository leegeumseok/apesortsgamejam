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
        boidScript.AvoidanceStrength = 7;
        boidScript.GoalStrength = 10;
    }


    void FixedUpdate()
    {
        // Handle Avoidance Stuff
        Debug.Log(gameObject.name + " position = " + transform.position);
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
            boidScript.GoalList.Add(primaryGoal);
        }
        // Set Up BoidScript
        Debug.Log(gameObject.name + " goalList count = " + goalList.Count);
        boidScript.GoalList = goalList;
        Debug.Log(gameObject.name + " avoidanceList count = " + avoidanceList.Count);
        boidScript.AvoidanceList = avoidanceList;
        boidScript.UpdateVelocity();

        // Add Acceleration to Rigidbody
        if (boidScript.CurrentVelocity.sqrMagnitude > 0 && rigidbody != null)
        {
            Vector3 newVelocity = boidScript.CurrentVelocity;
            newVelocity.y = 0;
            Vector3 newAcceleration = Vector3.Normalize(newVelocity) * acceleration;
            Debug.Log(gameObject.name + " newAcceleration = " + newAcceleration);
            transform.rigidbody.AddForce(newAcceleration, ForceMode.Acceleration);
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
