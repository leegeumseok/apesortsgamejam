using UnityEngine;
using System.Collections;

public class GenericEnemy : BeatReceiver 
{
    public virtual void OnSpawn() { }
    public virtual void OnHit() { }
    public virtual void OnGrabbed() { }
    public virtual void OnDamaged() { }
    public virtual void OnDestroyed() { }

    // Probably won't use this directly, but I'll add it anyway
    public override void OnBeat() { }
}
