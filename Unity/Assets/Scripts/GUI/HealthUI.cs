using UnityEngine;
using System.Collections;

public class HealthUI : MonoBehaviour {
	
	public GameObject mHealthBar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetHealth(float percent)
	{
		mHealthBar.transform.localScale = new Vector3(mHealthBar.transform.localScale.x, 
			percent, mHealthBar.transform.localScale.z);		
	}
}
