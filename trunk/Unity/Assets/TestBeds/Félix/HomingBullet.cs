using UnityEngine;
using System.Collections;

public class HomingBullet : MonoBehaviour
{
    public Collider target;
    public int damage;
    public float speed;
    public float force;

	public void Start()
    { }

    public void OnTriggerEnter(Collider collider)
    {
        Rigidbody body = collider.rigidbody;
        if (body != null)
        {
            Vector3 direction = (collider.transform.position - transform.position).normalized;
            body.AddForce(direction * force);
        }
        Destroy(gameObject);
    }

	public void Update()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Vector3 delta = direction * speed * Time.deltaTime;
        transform.position += delta;
	}
}
