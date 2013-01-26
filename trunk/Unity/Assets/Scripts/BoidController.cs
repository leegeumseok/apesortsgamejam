using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour
{

    private Boids boidScript;


    void Start()
    {
        boidScript = transform.GetComponent<Boids>();
    }


    void Update()
    {

    }

    Vector3 GetMousePosition()
    {
        int layermask = 1 << 8; //Conforms to the user layer of the mouselook plane!
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit, 1000, layermask);
        return hit.point;
    }
}
