using UnityEngine;
using System.Collections;
using System;

public class EnemyAttack : MonoBehaviour {

    public enum AttackType { Smash, Shoot };

    Boids boidScript;
    public ParticleSystem particleAttack;
    public ParticleSystem particleWindUp;
    public float attackDamage;
    public float attackRange;
    public float attackForce;
    public AttackType attackType = AttackType.Smash;
    public float attackArea;
    public float windupTime;
    private float windUpTimer;
    private float attackRangeSqd;
    private GameObject target;
    private bool WindingUp = false;
    internal bool canAttack;
	// Use this for initialization
	void Start () {
        boidScript = gameObject.GetComponent<Boids>();
        attackRangeSqd = attackRange * attackRange;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!WindingUp)
        {
            target = boidScript.CurrentTarget;
        }
        windUpTimer -= Time.deltaTime;
        if (target && canAttack)
        {
            if (Vector3.SqrMagnitude(target.transform.position - transform.position) < attackRangeSqd)
            {
                WindUp();
            }
        }
	}

    private void WindUp()
    {
        boidScript.enabled = false;
        if (!WindingUp)
        {
            windUpTimer = windupTime;
            transform.LookAt(target.transform);
        }
        else if (windUpTimer <= 0)
        {
            Attack();
        }
    }

    private void Attack()
    {
        switch (attackType)
        {
            case AttackType.Smash:
                Smash();
                break;
            case AttackType.Shoot:
                Shoot();
                break;
        }
    }

    private void Smash()
    {
        Vector3 smashPosition = transform.forward * attackRange + transform.position;
        var unitsHit = Physics.OverlapSphere(smashPosition, attackArea);
        foreach(var unit in unitsHit)
        {
            if (unit.gameObject != gameObject)
            {
                unit.SendMessage("AssignDamage", attackDamage);
                if (unit.rigidbody)
                    unit.rigidbody.AddForce(attackForce * transform.forward, ForceMode.Impulse);
                if (particleAttack)
                    Instantiate(particleAttack, smashPosition, Quaternion.identity);
            }
        }
    }

    private void Shoot()
    {

    }

    void Reset()
    {
        boidScript.enabled = true;
        WindingUp = false;
    }
}
