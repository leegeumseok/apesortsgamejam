using UnityEngine;
using System.Collections;

public class CameraDollyMovement : MonoBehaviour {
	public float speed; //how fast does the camera move?
	public float maxDistance; //distance from target that's allowed
	public CameraDollyPosition startPosition; //where does the camera start?
	private CameraDollyPosition target; //where are we heading?
	private float waitTimeCount;
	private float waitTimeMax;
	
	// Use this for initialization
	void Start () {
		this.transform.position = startPosition.transform.position;
		target = startPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null && Vector3.Distance(this.transform.position, target.transform.position) < maxDistance){
			this.transform.position = target.transform.position;
			target.SpecialActions(); //special: activate any special actions tied to this camera location
			waitTimeCount = 0f;
			waitTimeMax = target.waitTime;
			target = target.nextLocation;
		}
		
		waitTimeCount += Time.deltaTime;
		if (target != null && waitTimeCount > waitTimeMax){
			Vector3 directionTowardTarget = (target.transform.position - this.transform.position).normalized;
			this.transform.Translate(directionTowardTarget * speed * Time.deltaTime);
		}
		
	}
	
	public void setNewTarget(CameraDollyPosition newTarget){
		target = newTarget;
		waitTimeCount = waitTimeMax;
	}
}
