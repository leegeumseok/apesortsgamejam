using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {
	public float height, width;
	public GameObject planeToInstantiateFrom;
	private GameObject healthbar;
	
	// Use this for initialization
	void Start () {
		healthbar = Instantiate(planeToInstantiateFrom, this.transform.position, Quaternion.identity) as GameObject;
		setPercentHealth(100f);
	}
	
	// Update is called once per frame
	void Update () {
		//keep the health bar following its owner
		healthbar.transform.position = this.transform.position;
	}
	
	public void setPercentHealth(float percent){
		healthbar.transform.localScale = (new Vector3(percent/100f * width, 1f, height));
		if (percent > 66f){
			healthbar.renderer.material.color = Color.green;
		}
		else if (percent > 33f){
			healthbar.renderer.material.color = Color.yellow;
		}
		else{
			healthbar.renderer.material.color = Color.red;
		}
	}
}
