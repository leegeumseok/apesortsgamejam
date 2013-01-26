using UnityEngine;
using System.Collections;

public class DumbEnemy : MonoBehaviour {

    const float speed = 0.75f;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
        transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
	}
}
