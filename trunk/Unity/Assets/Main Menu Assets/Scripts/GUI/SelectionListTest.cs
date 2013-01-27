using UnityEngine;
using System.Collections;
 
public class SelectionListTest : MonoBehaviour {
 
	int currentSelection;
	public GUISkin skinWithListStyle;
	string[] myList = new string[] {"First", "Second", "Third"};
 
	protected void OnGUI() {
		GUI.skin = skinWithListStyle;
		currentSelection = GUILayoutx.SelectionList(currentSelection, myList, DoubleClickItem);
	}
 
	protected void DoubleClickItem(int index) {
		Debug.Log("Clicked " + myList[index]);
	}
}