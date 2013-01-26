using UnityEngine;
using System.Collections;

public class LazyCamera : MonoBehaviour 
{
    public Transform Target = null;
    public float DistanceThreshold = 0.0f;

    private Vector3 CalcDelta()
    {
        Vector3 tarPos = Target.transform.position;
        Vector3 curPos = transform.position;

        Vector3 tarPosPlanar = new Vector3(tarPos.x, 0.0f, tarPos.z);
        Vector3 curPosPlanar = new Vector3(curPos.x, 0.0f, curPos.z);

        return curPosPlanar - tarPosPlanar;
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector3 delta = CalcDelta();
        float deltaDist = delta.magnitude;

        Vector3 deltaCapped = delta.normalized * (deltaDist - DistanceThreshold);
        transform.position -= deltaCapped * Time.fixedDeltaTime * (deltaDist);
	}
}
