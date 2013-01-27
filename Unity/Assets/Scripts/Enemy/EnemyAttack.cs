using UnityEngine;
using System.Collections;
using System;

public class EnemyAttack : MonoBehaviour {

    public enum AttackType { Smash, Shoot };

    public ShrinkEnemyAnim shrink = null;

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
    private LookForward lookScript;
    internal bool canAttack = true;
    public float attackCooldown;
    private float resetDelay = 0;
    private bool resetTrue = true;
	// Use this for initialization
	void Start () {
        boidScript = gameObject.GetComponent<Boids>();
        lookScript = gameObject.GetComponent<LookForward>();
        Reset();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!WindingUp)
        {
            target = boidScript.CurrentTarget;
        }
        windUpTimer -= Time.deltaTime;
        resetDelay -= Time.deltaTime;
        if (target && canAttack && Vector3.SqrMagnitude(target.transform.position - transform.position) < attackRangeSqd && !resetTrue && !WindingUp)
        {
            WindUp();
        }
        else if (windUpTimer <= 0 && WindingUp && !resetTrue)
        {
            Attack();
        }
        Reset();
	}

    private void WindUp()
    {
        boidScript.enabled = false;
        lookScript.enabled = false;
        WindingUp = true;
        particleWindUp.Play();
        if (shrink != null) shrink.Shrink();
        windUpTimer = windupTime;
        transform.LookAt(target.transform);

        this.SendMessage("OnWindup");
    }

    private void Attack()
    {
        Debug.Log("Attacking!");
        resetDelay = attackCooldown;
        resetTrue = true;
        particleWindUp.Stop();
        if (shrink != null) shrink.Expand();
        
        switch (attackType)
        {
            case AttackType.Smash:
                Smash();
                particleAttack.Play();
                break;
            case AttackType.Shoot:
                Shoot();
                break;
        }

        this.SendMessage("OnAttack");
    }

    private void Smash()
    {
        Vector3 smashPosition = transform.forward * attackRange + transform.position;
        var unitsHit = Physics.OverlapSphere(smashPosition, attackArea);
        foreach(var unit in unitsHit)
        {
            if (unit.gameObject != gameObject)
            {
                unit.SendMessage("AssignDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
                if (unit.rigidbody)
                    unit.rigidbody.AddForce(attackForce * transform.forward, ForceMode.Impulse);
            }
        }
    }

    private void Shoot()
    {

    }

    void Reset()
    {
        if (resetDelay <= 0 && resetTrue)
        {
            if (shrink != null) shrink.Expand();
            lookScript.enabled = true;
            particleWindUp.Stop();
            boidScript.enabled = true;
            WindingUp = false;
            canAttack = true;
            attackRangeSqd = attackRange * attackRange;
            resetTrue = false;
        }
    }
}
