using UnityEngine;
using System.Collections;

public class DumbEnemy : MonoBehaviour {

    const float speed = 0.75f;
    public Vector3 direction;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
        transform.position += direction * speed * Time.deltaTime;
	}
}
