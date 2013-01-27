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
        Vector3 targetPos = targetObject.transform.position;
        targetPos.y = transform.position.y;
        this.transform.LookAt(targetPos);
	}
}
