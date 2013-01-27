using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenericTower : MonoBehaviour {

    private float timeBeforeNextAttack;

    public float attackRange;
    public float attacksPerSecond;
    public int bulletDamage;
    public float bulletSpeed;
    public float bulletForce;
    public int ammunition = int.MaxValue;
    public GameObject bulletTemplate;
    public Transform bulletSpawnPoint;
    public LayerMask targetLayer;

    public event Action notifyOnDestroyed;

    public virtual void OnDestroyed()
    {
        var onDestroyed = notifyOnDestroyed;
        if (onDestroyed != null)
            onDestroyed();
    }

    public virtual void OnAttack(Collider target)
    {

    }

	public void Start()
    {
        timeBeforeNextAttack = 1 / attacksPerSecond;

        if (bulletDamage == 0)
            Debug.LogError("Bullets will not do any damage");

        if (bulletSpeed == 0)
            Debug.LogError("Bullets have a zero speed");
	}
	
	// Update is called once per frame
	public void Update()
    {
        if (ammunition > 0)
        {
            timeBeforeNextAttack -= Time.deltaTime;
            var collidedWith = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

            Collider target = FindClosestTarget(collidedWith);
            if (target != null)
            {
                if (timeBeforeNextAttack <= 0)
                {
                    Attack(target);
                    timeBeforeNextAttack = 1 / attacksPerSecond;
                }
            }
        }
        else
        {
            Delete();
        }
	}

    private void Attack(Collider target)
    {
        ammunition--;
        GameObject bullet = (GameObject)Instantiate(bulletTemplate);
        bullet.transform.position = bulletSpawnPoint.transform.position;

        HomingBullet component = bullet.GetComponent<HomingBullet>();
        component.target = target;
        component.damage = bulletDamage;
        component.speed = bulletSpeed;
        component.force = bulletForce;

        OnAttack(target);
    }

    private Collider FindClosestTarget(Collider[] targets)
    {
        if (targets.Length == 0)
            return null;

        Collider closest = targets[0];
        float closestMagnitude = SquaredDistance(closest);
        for (int i = 1; i < targets.Length; i++)
        {
            Collider target = targets[i];
            float magnitude = SquaredDistance(target);
            if (magnitude < closestMagnitude)
            {
                closest = target;
                closestMagnitude = magnitude;
            }
        }
        return closest;
    }

    private float SquaredDistance(Collider collider)
    {
        return (collider.ClosestPointOnBounds(transform.position) - transform.position).sqrMagnitude;
    }

    private void Delete()
    {
        OnDestroyed();
        GameObject.Destroy(gameObject);
    }
}
