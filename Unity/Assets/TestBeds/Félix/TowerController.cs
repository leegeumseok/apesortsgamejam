using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerController : MonoBehaviour {

    private List<Collider> mTargets = new List<Collider>();
    private float timeBeforeNextAttack;

    public float attacksPerSecond;
    public int bulletDamage;
    public float bulletSpeed;
    public Transform bulletTemplate;

    private float AttackDelay
    {
        get { return 1 / attacksPerSecond; }
    }

	public void Start()
    {
        timeBeforeNextAttack = AttackDelay;
        if (bulletDamage == 0)
            Debug.LogError("Bullets will not do any damage");
        if (bulletSpeed == 0)
            Debug.LogError("Bullets have a zero speed");
	}

    public void OnTriggerEnter(Collider other)
    {
        mTargets.Add(other);
    }

    public void OnTriggerExit(Collider other)
    {
        mTargets.Remove(other);
    }
	
	// Update is called once per frame
	public void Update()
    {
        timeBeforeNextAttack -= Time.deltaTime;
        Collider target = FindClosestTarget();
        if (target != null)
        {
            Vector3 lookAt = target.transform.position;
            lookAt.y = transform.position.y;
            transform.LookAt(lookAt);

            if (timeBeforeNextAttack <= 0)
            {
                Transform bullet = (Transform)Instantiate(bulletTemplate);
                Bullet component = bullet.GetComponent<Bullet>();
                component.target = target;
                component.damage = bulletDamage;
                component.speed = bulletSpeed;

                timeBeforeNextAttack = AttackDelay;
            }
        }
	}

    private Collider FindClosestTarget()
    {
        if (mTargets.Count == 0)
            return null;

        Collider closest = mTargets[0];
        float closestMagnitude = closest.ClosestPointOnBounds(transform.position).sqrMagnitude;
        for (int i = 1; i < mTargets.Count; i++)
        {
            Collider target = mTargets[i];
            float magnitude = target.ClosestPointOnBounds(transform.position).sqrMagnitude;
            if (magnitude < closestMagnitude)
            {
                closest = target;
                closestMagnitude = magnitude;
            }
        }
        return closest;
    }
}
