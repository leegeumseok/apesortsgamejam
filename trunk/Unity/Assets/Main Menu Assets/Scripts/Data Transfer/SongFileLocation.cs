using UnityEngine;
using System.Collections;

public class SongFileLocation : MonoBehaviour {
	public string FilePath;
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		this.tag = "SongFileLocation";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
