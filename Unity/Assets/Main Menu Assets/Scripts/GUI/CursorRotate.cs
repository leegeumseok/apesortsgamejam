using UnityEngine;
using System.Collections;

public class CursorRotate : MonoBehaviour {
	public float rotationsPerSecond;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAroundLocal(transform.up, (Mathf.PI * 2) * rotationsPerSecond * Time.deltaTime);
	}
}
