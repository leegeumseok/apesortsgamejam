using UnityEngine;
using System.Collections;

public class BasicEnemy : GenericEnemy
{
    public static readonly float DEATH_DELAY = 1.0f;

    bool isDying = false;
    float deathTime = 0.0f;

    void Update()
    {
        if (this.isDying == true)
        {
            this.deathTime -= Time.deltaTime;
            if (this.deathTime < 0.001f)
                GameObject.Destroy(this.gameObject);
        }
    }

    public override void OnSpawn() { }

    public override void OnHit() 
    {
        this.isDying = true;
        this.deathTime = DEATH_DELAY;
        Boids boid = this.GetComponent<Boids>();
        boid.AvoidanceRule = boid.GoalSeekingRule = false;
    }

    public override void OnGrabbed() { }
    public override void OnDamaged(int damage) { }
    public override void OnDestroyed() { }

    // Probably won't use this directly, but I'll add it anyway
    public override void OnBeat() { }
}