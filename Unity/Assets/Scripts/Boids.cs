using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour
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

    public float AvoidanceFilterDistance
    {
        get;
        set;
    }

    public float AlignmentFilterDistance
    {
        get;
        set;
    }

    public float CohesionFilterDistance
    {
        get;
        set;
    }

    public List<GameObject> GoalList
    {
        get;
        set;
    }

    public List<GameObject> AvoidanceList
    {
        get;
        set;
    }

    public List<GameObject> CohesionList
    {
        get;
        set;
    }

    public List<GameObject> AlignmentList
    {
        get;
        set;
    }

    public List<GameObject> SeparationList
    {
        get;
        set;
    }

    public Vector3 CurrentVelocity
    {
        get;
        protected set;
    }

    public void Update()
    {
        Vector3 newVelocity = new Vector3();
        if (SeparationRule)
        {
            foreach (GameObject boid in SeparationList)
            {
                float separationFilterSqd = SeparationDistance * SeparationDistance;
                if (Vector3.SqrMagnitude(transform.position - boid.transform.position) < separationFilterSqd && SeparationDistance != 0)
                {
                    newVelocity -= (boid.transform.position - transform.position);
                }
            }
        }
        if (GoalSeekingRule)
        {
            foreach (GameObject boid in GoalList)
            {
                float goalFilterSqd = GoalFilterDistance*GoalFilterDistance;
                if (Vector3.SqrMagnitude(boid.transform.position - transform.position) < goalFilterSqd && GoalFilterDistance != 0)
                {
                    newVelocity += Vector3.one * (GoalStrength + 1f) / Vector3.SqrMagnitude(boid.transform.position - transform.position);
                }
            }
        }
        if (AvoidanceRule)
        {
            foreach (GameObject boid in AvoidanceList)
            {
                float avoidanceFilterSqd = AvoidanceFilterDistance * AvoidanceFilterDistance;
                if (Vector3.SqrMagnitude(boid.transform.position - transform.position) < avoidanceFilterSqd && AvoidanceFilterDistance != 0)
                {
                    newVelocity -= Vector3.one * (GoalStrength + 1f) / Vector3.SqrMagnitude(boid.transform.position - transform.position);
                }
            }
        }
        if (AlignmentRule)
        {
            foreach (GameObject boid in AlignmentList)
            {
                float alignmentFilterSqd = AlignmentFilterDistance * AlignmentFilterDistance;
                if (Vector3.SqrMagnitude(boid.transform.position - transform.position) < alignmentFilterSqd && AlignmentFilterDistance != 0)
                {
                    newVelocity += boid.GetComponent<Boid>().CurrentVelocity;
                }
            }
        }
        if (CohesionRule)
        {
            Vector3 cohesion = new Vector3();

            foreach (GameObject boid in AlignmentList)
            {
                float cohesionFilterSqd = CohesionFilterDistance * CohesionFilterDistance;
                if (Vector3.SqrMagnitude(boid.transform.position - transform.position) < cohesionFilterSqd && CohesionFilterDistance != 0)
                {
                    cohesion += boid.transform.position;
                }
            }
            cohesion /= CohesionList.Count;
            newVelocity += cohesion;
        }

        CurrentVelocity += newVelocity;
    }
}
