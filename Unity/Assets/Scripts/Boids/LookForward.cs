using UnityEngine;
using System.Collections;

public class LookForward : MonoBehaviour
{
    public float driveSpeed = 120.0f;
    public float dragRadius = 0.5f;

    protected Vector3 lastPosition = Vector3.zero;

    void Update()
    {
        this.HandleOrientation();
        this.lastPosition = transform.position;
    }

    private void HandleOrientation()
    {
        Quaternion desiredOrientation = this.CalcHeadingOrientation();

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            desiredOrientation,
            driveSpeed * Time.deltaTime);
    }

    private Vector3 CalcDragArm()
    {
        Vector3 dragPoint = lastPosition - transform.forward * dragRadius;
        Vector3 pointArm = dragPoint - transform.position;
        pointArm = pointArm.normalized * dragRadius;
        pointArm.y = 0;
        return pointArm;
    }

    private Quaternion CalcHeadingOrientation()
    {
        return Quaternion.LookRotation(-CalcDragArm());
    }
}