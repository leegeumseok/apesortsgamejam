using UnityEngine;
using System.Collections;

public class GenericEnemy : BeatReceiver
{
    public delegate void OnDestroyedNotify();

	public int healthPoints = 100;
    public event OnDestroyedNotify notifyOnDestroyed;

    public virtual void OnSpawn() { }
    public virtual void OnHit() { }
    public virtual void OnGrabbed() { }
    public virtual void OnDamaged(int damage) { }

    public virtual void OnDestroyed() 
    {
        var onDestroyed = notifyOnDestroyed;
        if (onDestroyed != null)
        {
            notifyOnDestroyed();
        }

        GameObject.Destroy(this.gameObject);
    }

    // Probably won't use this directly, but I'll add it anyway
    public override void OnBeat() { }
}
