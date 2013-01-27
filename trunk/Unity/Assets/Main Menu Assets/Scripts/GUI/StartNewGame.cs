using UnityEngine;
using System.Collections;

public class StartNewGame : SpecialAction {
	public string SceneName;
	public SongFileFinder fileFinder;
	public GameObject songFilePathHolder;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	override public void Action(){
		GameObject s = Instantiate(songFilePathHolder) as GameObject;
		s.GetComponent<SongFileLocation>().FilePath = fileFinder.m_textPath;
		Application.LoadLevel(SceneName);
	}
}
