using UnityEngine;
using System.Collections.Generic;

public class Boids : MonoBehaviour
{
    public bool SeparationRule
    {
        get;
        set;
    }

    public bool AlignmentRule
    {
        get;
        set;
    }

    public bool CohesionRule
    {
        get;
        set;
    }

    public bool AvoidanceRule
    {
        get;
        set;
    }

    public bool GoalSeekingRule
    {
        get;
        set;
    }

    public float SeparationDistance
    {
        get;
        set;
    }

    public float GoalStrength
    {
        get;
        set;
    }

    public float GoalFilterDistance
    {
        get;
        set;
    }

    public float AvoidanceStrength
    {
        get;
        set;
    }

    public List<GameObject> GoalList = new List<GameObject>();

    public List<GameObject> AvoidanceList = new List<GameObject>();

    public List<GameObject> CohesionList = new List<GameObject>();

    public List<GameObject> AlignmentList = new List<GameObject>();

    public List<GameObject> SeparationList = new List<GameObject>();

    public Vector3 CurrentVelocity
    {
        get;
        protected set;
    }

    public void UpdateVelocity()
    {
        Vector3 newVelocity = new Vector3();
        if (SeparationRule)
        {
            foreach (GameObject boid in SeparationList)
            {
                float separationFilterSqd = SeparationDistance * SeparationDistance;
                if (Vector3.SqrMagnitude(transform.position - boid.transform.position) < separationFilterSqd || SeparationDistance != 0)
                {
                    newVelocity -= (boid.transform.position - transform.position);
                }
            }
        }
        if (GoalSeekingRule)
        {
            foreach (GameObject boid in GoalList)
            {
                newVelocity += Vector3.Normalize(boid.transform.position - transform.position) * (GoalStrength + 1f) / Vector3.SqrMagnitude(boid.transform.position - transform.position);
            }
        }
        if (AvoidanceRule)
        {
            foreach (GameObject boid in AvoidanceList)
            {
                newVelocity -= Vector3.Normalize(boid.transform.position - transform.position) * (GoalStrength + 1f) / Vector3.SqrMagnitude(boid.transform.position - transform.position);
            }
        }
        if (AlignmentRule)
        {
            foreach (GameObject boid in AlignmentList)
            {
                newVelocity += boid.GetComponent<Boids>().CurrentVelocity;
            }
        }
        if (CohesionRule)
        {
            Vector3 cohesion = new Vector3();

            foreach (GameObject boid in AlignmentList)
            {
                cohesion += boid.transform.position;
            }
            cohesion /= CohesionList.Count;
            newVelocity += cohesion;
        }

        CurrentVelocity += newVelocity;
    }
}
