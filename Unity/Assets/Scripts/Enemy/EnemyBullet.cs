using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    public ParticleSystem travelingEffect;
    public ParticleSystem impactEffect;
    public ParticleSystem bulletEffect;
    internal GameObject firingEnemy;
    public float attackForce;
    public float attackDamage;
    public float bulletDeathCooldown;
    public float bulletSpeed;
    private bool bulletDying;

    void Start()
    {
        travelingEffect.Play();
        bulletEffect.Play();
        impactEffect.Stop();
        Debug.Log("Bullet Alive!");
    }

	void Update () 
    {
        transform.position += transform.forward * bulletSpeed *Time.deltaTime;
        if (bulletDying)
        {
            bulletDeathCooldown -= Time.deltaTime;
            Debug.Log("Bullet Dying!");
            if (bulletDeathCooldown <= 0)
            {
                Debug.Log("Bullet Dead!");
                impactEffect.Stop();
                GameObject.Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter(Collider unit)
    {
        if (unit.gameObject != firingEnemy && !bulletDying)
        {
            Debug.Log("Bullet Hit! " + unit.name);
            unit.SendMessage("AssignDamage", attackDamage);
            travelingEffect.Stop();
            impactEffect.Play();
            bulletEffect.Stop();
            if (unit.rigidbody)
                unit.rigidbody.AddForce(attackForce * transform.forward, ForceMode.Impulse);
        }
    }


}
