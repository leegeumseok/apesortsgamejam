using UnityEngine;
using System.Collections;

public class EKGeffect : MonoBehaviour {
	public int maxDistanceFromCenter; //how far you can go from where you start
	private Vector3 center; //where you start
	public float maxSpeed; //maximum speed of the EKG spikes
	public float minSpeed;
	private float curSpeed;
	//time between direction changes (in seconds)
	public float minWait;
	public float maxWait;
	//keeps track of time between changes
	private float timerCount;
	private float timerMax;
	
	// Use this for initialization
	void Start () {
		center = this.transform.position;
		curSpeed = 0f;
		timerCount = 0f;
		timerMax = Random.Range(minWait,maxWait);
	}
	
	// Update is called once per frame
	void Update () {
		//movement
		this.transform.Translate(new Vector3(0,curSpeed,0) * Time.deltaTime);
		
		//max distance
		if (Vector3.Distance(transform.position, center) > maxDistanceFromCenter){
			timerCount = timerMax; //switch directions
		}
		
		//direction changes
		timerCount += Time.deltaTime;
		if (timerCount >= timerMax){
			if (curSpeed == 0){
				curSpeed = Random.Range(minSpeed,maxSpeed);
			}
			else if (curSpeed > 0){
				curSpeed = Random.Range(-maxSpeed,-minSpeed);
			}
			else{
				curSpeed = 0;
			}
			timerMax = Random.Range(minWait,maxWait);
			timerCount = 0f;
		}
	}
}
