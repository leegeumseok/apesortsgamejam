using UnityEngine;
using System.Collections;

public class HomingBullet : MonoBehaviour
{
    public GameObject explosion;
    public Collider target;
    public int damage;
    public float speed;
    public float force;

	public void Start()
    {
        Rigidbody body = target.rigidbody;
        if (body != null)
        {
            body.WakeUp();
        }

        target.GetComponent<GenericEnemy>().notifyOnDestroyed += Delete;
    }

    public void OnTriggerEnter(Collider collider)
    {
        Rigidbody body = collider.rigidbody;
        if (body != null)
        {
            Vector3 direction = (collider.transform.position - transform.position).normalized;
            body.AddForce(direction * force, ForceMode.Impulse);
        }

        if (explosion != null)
            Instantiate(explosion, collider.transform.position, Quaternion.identity);

        collider.gameObject.SendMessage("OnDamaged", damage);
        Delete();
    }

    public void Update()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Vector3 delta = direction * speed * Time.deltaTime;
        transform.position += delta;
    }

    private void Delete()
    {
        target.GetComponent<GenericEnemy>().notifyOnDestroyed -= Delete;
        GameObject.Destroy(gameObject);
    }
}
