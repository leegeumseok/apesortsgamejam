using UnityEngine;
using System.Collections.Generic;

public class Boids : MonoBehaviour
{
    public bool enabled = true;

    public bool SeparationRule;

    public bool AlignmentRule;

    public bool CohesionRule;

    public bool AvoidanceRule;

    public bool GoalSeekingRule;

    public float SeparationDistance;

    public float GoalStrength;

    public float AvoidanceStrength;

    public List<GameObject> GoalList = new List<GameObject>();

    public List<GameObject> AvoidanceList = new List<GameObject>();

    public List<GameObject> CohesionList = new List<GameObject>();

    public List<GameObject> AlignmentList = new List<GameObject>();

    public List<GameObject> SeparationList = new List<GameObject>();

    public float boundingBufferRangeSqd;

    public float boundingMaxRangeSqd;

    public float boundingBufferStrength;

    public Vector3 BoundingCenter;

    public Vector3 CurrentVelocity
    {
        get;
        protected set;
    }

    public GameObject CurrentTarget
    {
        get;
        protected set;
    }

    public Vector3 UpdateVelocity()
    {
        Vector3 newVelocity = new Vector3();
        if (enabled)
        {
            if (boundingMaxRangeSqd < Vector3.SqrMagnitude(BoundingCenter - transform.position))
            {
                newVelocity = BoundingCenter - transform.position;
            }
            else
            {
                if (boundingBufferRangeSqd < Vector3.SqrMagnitude(BoundingCenter - transform.position))
                {
                    float currentDistSqdFromCenter = Vector3.SqrMagnitude(BoundingCenter - transform.position);
                    Vector3 addVelocity = Vector3.Normalize(BoundingCenter - transform.position) * (boundingMaxRangeSqd - currentDistSqdFromCenter) * boundingBufferStrength;
                    newVelocity += addVelocity;
                }
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
                if (GoalSeekingRule && GoalList.Count > 0)
                {
                    Vector3 goalVelocity = new Vector3();
                    float maxMagnitude = 0;
                    foreach (GameObject boid in GoalList)
                    {
                        float magnitude = Vector3.SqrMagnitude(boid.transform.position - transform.position);
                        if (magnitude > maxMagnitude)
                        {
                            maxMagnitude = magnitude;
                            goalVelocity = boid.transform.position - transform.position;
                            CurrentTarget = boid.gameObject;
                        }
                    }
                    newVelocity += goalVelocity;
                }
                if (AvoidanceRule)
                {
                    Vector3 avoidanceVelocity = new Vector3();
                    foreach (GameObject boid in AvoidanceList)
                    {
                        avoidanceVelocity += Vector3.Normalize(boid.transform.position - transform.position) * (GoalStrength + 1f) / Vector3.SqrMagnitude(boid.transform.position - transform.position);
                    }
                    newVelocity += avoidanceVelocity;
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
            }
        }
        return newVelocity; 
    }
}
