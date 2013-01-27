using UnityEngine;
using System.Collections;

public class GenericEnemy : BeatReceiver
{
    public delegate void OnDestroyedNotify();
    public int healthPoints = 100;
    public int resourceValue = 1;

    public event OnDestroyedNotify notifyOnDestroyed;

    public virtual void OnSpawn() { }
    public virtual void OnHit() { }
    public virtual void OnGrabbed() { }
    public virtual void OnReleased() { }

    public virtual void OnDamaged(int damage)
    {
        // see subclasses for actual death behavior
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Boids boid = this.GetComponent<Boids>();
            boid.AvoidanceRule = boid.GoalSeekingRule = false;
        }
    }

    public virtual void OnDestroyed() 
    {
        var onDestroyed = notifyOnDestroyed;
        if (onDestroyed != null)
        {
            notifyOnDestroyed();
        }

        Player.Instance.GetComponent<TowerCreator>().GatherResources(resourceValue);
        Player.Instance.GetComponent<PlayerController>().OnGrabbedDeath();

        GameObject.Destroy(this.gameObject);
    }

    // Probably won't use this directly, but I'll add it anyway
    public override void OnBeat() { }
}
