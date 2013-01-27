using UnityEngine;
using System.Collections;

public class DestroyOnStart : MonoBehaviour 
{
	void Start () 
    {
        GameObject.Destroy(this.gameObject);
	}
}
