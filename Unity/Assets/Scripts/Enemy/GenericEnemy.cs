using UnityEngine;
using System.Collections;

public class GenericEnemy : BeatReceiver 
{
    public delegate void OnDestroyedNotify();
    public event OnDestroyedNotify notifyOnDestroyed;

    public virtual void OnSpawn() { }
    public virtual void OnHit() { }
    public virtual void OnGrabbed() { }
    public virtual void OnDamaged(int damage) { }

    public virtual void OnDestroyed() 
    {
        if (notifyOnDestroyed != null)
            notifyOnDestroyed.Invoke();
    }

    // Probably won't use this directly, but I'll add it anyway
    public override void OnBeat() { }
}
