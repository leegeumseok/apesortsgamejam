using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour 
{
    public Transform target;
    public Boids boid;

    void Start()
    {
        this.boid = transform.parent.GetComponent<Boids>();
    }

	void Update () 
    {
        GameObject targetObject = this.boid.CurrentTarget;
        if (targetObject == null)
            targetObject = Player.Instance.gameObject;
        this.target = targetObject.transform;
        this.transform.LookAt(target);
	}
}
