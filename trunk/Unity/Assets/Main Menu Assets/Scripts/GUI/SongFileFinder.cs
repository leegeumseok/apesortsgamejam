using UnityEngine;
 
public class SongFileFinder : MonoBehaviour {
 
	public bool isVisible; //is this visible?
	
	public string m_textPath;
 
	protected FileBrowser m_fileBrowser;
	
	public GameObject message1, message2;
 
	[SerializeField]
	protected Texture2D	m_directoryImage,
						m_fileImage;
 
	protected void OnGUI () {
		if (isVisible){
			if (m_fileBrowser != null) {
				m_fileBrowser.OnGUI();
			} else {
				OnGUIMain();
			}
		}
	}
 
	protected void OnGUIMain() {
 
		GUILayout.BeginHorizontal();
			GUILayout.Label("MP3 File", GUILayout.Width(100));
			GUILayout.FlexibleSpace();
			GUILayout.Label(m_textPath ?? "none selected");
			if (GUILayout.Button("...", GUILayout.ExpandWidth(false))) {
				m_fileBrowser = new FileBrowser(
					new Rect(0, 0, Screen.width, Screen.height),
					"Choose MP3 File",
					FileSelectedCallback
				);
				m_fileBrowser.SelectionPattern = "*.mp3";
				m_fileBrowser.DirectoryImage = m_directoryImage;
				m_fileBrowser.FileImage = m_fileImage;
			}
		GUILayout.EndHorizontal();
	}
 
	protected void FileSelectedCallback(string path) {
		m_fileBrowser = null;
		m_textPath = path;
		//ARL (aka Adam added stuff below this point)
		if (path != null && path != ""){
			message1.renderer.enabled = false;
			message2.renderer.enabled = true;
			message2.collider.enabled = true;
		}
	}
}