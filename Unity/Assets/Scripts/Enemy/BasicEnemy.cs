using UnityEngine;
using System.Collections;

public class BasicEnemy : GenericEnemy
{
    public static readonly float DEATH_DELAY = 1.0f;

    public bool isDying = false;
    public float deathTime = 0.0f;

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
    }

    public override void OnSpawn() { }
    public override void OnHit()  { }
    public override void OnGrabbed() { }

    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);

        if (healthPoints < 100)
        {
            this.isDying = true;
            this.deathTime = DEATH_DELAY;
        }
    }

    public override void OnDestroyed() 
    {
        base.OnDestroyed();
    }

    // Probably won't use this directly, but I'll add it anyway
    public override void OnBeat() { }
}