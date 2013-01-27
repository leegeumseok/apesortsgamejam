using UnityEngine;
using System.Collections;

public class GenericEnemy : BeatReceiver
{
    public delegate void OnDestroyedNotify(GenericEnemy enemy);
    public int maxHealthPoints = 100;
    public int healthPoints = 100;
    public int resourceValue = 1;
    public int killPoints = 100;
    public ParticleSystem GrabbedParticle = null;
    public ParticleSystem DyingParticle = null;
    public bool IsGrabbed = false;

    public AudioClip[] AudioOnHit = null;
    public AudioClip[] AudioOnDeath = null;
    public AudioClip[] AudioOnWindUp = null;
    public AudioClip[] AudioOnAttack = null;
    public AudioClip[] AudioOnSpawn = null;

    public event OnDestroyedNotify notifyOnDestroyed;

    public virtual void OnSpawn() 
    {
        if (audio != null && AudioOnSpawn != null && AudioOnSpawn.Length > 0)
        {
            int index = Random.Range(0, this.AudioOnSpawn.Length);
            audio.clip = this.AudioOnSpawn[index];
            audio.Play();
        }
    }

    public virtual void OnHit() 
    {
        if (audio != null && AudioOnHit != null && AudioOnHit.Length > 0)
        {
            int index = Random.Range(0, this.AudioOnHit.Length);
            audio.clip = this.AudioOnHit[index];
            audio.Play();
        }
    }

    public virtual void OnGrabbed() 
    {
        this.IsGrabbed = true;
        if (this.GrabbedParticle != null)
            this.GrabbedParticle.Play();
    }

    public virtual void OnReleased() 
    {
        this.IsGrabbed = false;
        if (this.GrabbedParticle != null)
            this.GrabbedParticle.Stop();
    }

    public virtual void OnDamaged(int damage)
    {
        // see subclasses for actual death behavior
        healthPoints -= damage;

        Healthbar healthBar = GetComponent<Healthbar>();
        if (healthBar != null)
        {
            float lifePercent = (float)healthPoints / maxHealthPoints;
            healthBar.setPercentHealth(lifePercent);
        }

        if (healthPoints <= 0)
        {
            Boids boid = this.GetComponent<Boids>();
            boid.AvoidanceRule = boid.GoalSeekingRule = false;
        }
    }

    public virtual void OnDying()
    {
        if (this.DyingParticle != null)
            this.DyingParticle.Play();

        if (audio != null && AudioOnDeath != null && AudioOnDeath.Length > 0)
        {
            int index = Random.Range(0, this.AudioOnDeath.Length);
            audio.clip = this.AudioOnDeath[index];
            audio.Play();
        }
    }

    public virtual void OnDestroyed() 
    {
        var onDestroyed = notifyOnDestroyed;
        if (onDestroyed != null)
        {
            notifyOnDestroyed(this);
        }

        Player.Instance.GetComponent<TowerCreator>().GatherResources(resourceValue);
        Player.Instance.GetComponent<PlayerController>().OnGrabbedDeath();

        GameObject.Destroy(this.gameObject);
    }

    public virtual void OnWindup()
    {
        if (audio != null && AudioOnWindUp != null && AudioOnWindUp.Length > 0)
        {
            int index = Random.Range(0, this.AudioOnWindUp.Length);
            audio.clip = this.AudioOnWindUp[index];
            audio.Play();
        }
    }

    public virtual void OnAttack()
    {
        if (audio != null && AudioOnAttack != null && AudioOnAttack.Length > 0)
        {
            int index = Random.Range(0, this.AudioOnAttack.Length);
            audio.clip = this.AudioOnAttack[index];
            audio.Play();
        }
    }

    // Probably won't use this directly, but I'll add it anyway
    public override void OnBeat() { }
}
