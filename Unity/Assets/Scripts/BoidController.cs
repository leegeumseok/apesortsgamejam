using UnityEngine;
using System.Collections.Generic;

public class BoidController : MonoBehaviour
{

    private Boids boidScript;
    public float filterRange = 5;
    private Collider[] colliderArray;
    public LayerMask avoidanceMask = new LayerMask();
    public LayerMask goalMask = new LayerMask();
    public LayerMask mouselookMask = new LayerMask();
    public GameObject primaryGoal;
    public float acceleration;
    public float dragCoefficient;

    void Start()
    {
        boidScript = transform.GetComponent<Boids>();
    }


    void Update()
    {
        colliderArray = Physics.OverlapSphere(transform.position, filterRange);
        List<GameObject> goalList = new List<GameObject>();
        foreach (Collider collider in colliderArray)
        {
            GameObject temp = collider.gameObject;
            int avoidanceTrue = (avoidanceMask.value & (1 << temp.layer));
            if (0 != avoidanceTrue)
            {

            }
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
