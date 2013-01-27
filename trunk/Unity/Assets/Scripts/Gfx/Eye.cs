using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour 
{
    public Transform target;

    public Boids boid;

    void Start()
    {
        this.boid = GetComponent<Boids>();
    }

	void Update () 
    {
        this.target = this.boid.CurrentTarget.transform;
        this.transform.LookAt(target);
	}
}
