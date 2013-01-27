using UnityEngine;
using System.Collections;

public class BasicEnemy : GenericEnemy
{
    public static readonly float DEATH_DELAY = 1.0f;

    private float nextAttack;
    public bool isDying = false;
    public float deathTime = 0.0f;
    public int heartDamage = 1;
    public float attacksPerSecond = 1;

    void Start()
    {
        nextAttack = 1 / attacksPerSecond;
    }

    void Update()
    {
        if (this.isDying == true)
        {
            this.deathTime -= Time.deltaTime;
            if (this.deathTime < 0.001f)
            {
                OnDestroyed();
            }
        }
        else
        {
            nextAttack -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDying)
        {
            // TODO damage enemies if you hit them on your way flying out
        }
        else if (collision.gameObject.name == "Pedestal")
        {
            CollideWithPedestal(collision.gameObject);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Pedestal")
        {
            CollideWithPedestal(collision.gameObject);
        }
    }

    public override void OnSpawn() 
    {
        base.OnSpawn();
    }

    public override void OnHit() 
    {
        base.OnHit();
    }

    public override void OnGrabbed() 
    {
        base.OnGrabbed();
    }

    public override void OnReleased()
    {
        base.OnReleased();
    }

    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);

        if (healthPoints <= 0)
        {
            this.OnDying();
        }
    }

    public override void OnDestroyed() 
    {
        base.OnDestroyed();
    }

    public override void OnDying()
    {
        base.OnDying();
        this.isDying = true;
        this.deathTime = DEATH_DELAY;
    }

    // Probably won't use this directly, but I'll add it anyway
    public override void OnBeat() 
    {
        base.OnBeat();
    }

    private void CollideWithPedestal(GameObject pedestal)
    {
        if (nextAttack <= 0)
        {
            pedestal.SendMessage("OnDamage", heartDamage);
            nextAttack = 1 / attacksPerSecond;
        }
    }
}