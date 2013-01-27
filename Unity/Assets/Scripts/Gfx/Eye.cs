using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour 
{
    public Transform target;

	void Update () 
    {
        transform.LookAt(target.transform.position);
	}
}
