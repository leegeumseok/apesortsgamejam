using UnityEngine;
using System.Collections;

public class HomingBullet : MonoBehaviour
{
    public Collider target;
    public int damage;
    public float speed;

	public void Start()
    { }

    public void OnTriggerEnter()
    {
        Destroy(gameObject);
    }

	public void Update()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Vector3 delta = direction * speed * Time.deltaTime;
        transform.position += delta;
	}
}
