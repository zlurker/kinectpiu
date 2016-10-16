using UnityEngine;
using System.Collections;

public class GameControlScript : MonoBehaviour {
	[Tooltip("Prefab used to create the scene boundary")]
	public GameObject cratePrefab;

	[Tooltip("GUI Window rectangle in pixels")]
	private Rect guiWindowRect = new Rect(10, 200, 200, 260);

	[Tooltip("GUI Window skin (optional)")]
	public GUISkin guiSkin;
	
	
	void Start() {
		Quaternion quatRot90 = Quaternion.Euler(new Vector3(0, 90, 0));
		GameObject newObj = null;
		
		for(int i = -50; i <= 50; i++) {
			newObj = (GameObject)GameObject.Instantiate(cratePrefab, new Vector3(i, 0.32f, 50), Quaternion.identity);
			newObj.transform.parent = transform;

			newObj = (GameObject)GameObject.Instantiate(cratePrefab, new Vector3(i, 0.32f, -50), Quaternion.identity);
			newObj.transform.parent = transform;

			newObj = (GameObject)GameObject.Instantiate(cratePrefab, new Vector3(50, 0.32f, i), quatRot90);
			newObj.transform.parent = transform;

			newObj = (GameObject)GameObject.Instantiate(cratePrefab, new Vector3(-50, 0.32f, i), quatRot90);
			newObj.transform.parent = transform;
		}
	}

	private void ShowGuiWindow(int windowID) {
		GUILayout.BeginVertical();

		//GUILayout.Label("");
		GUILayout.Label("<b>* FORWARD / GO AHEAD</b>");
		GUILayout.Label("<b>* BACK / GO BACK</b>");
		GUILayout.Label("<b>* TURN LEFT</b>");
		GUILayout.Label("<b>* TURN RIGHT</b>");
		GUILayout.Label("<b>* RUN</b>");
		GUILayout.Label("<b>* JUMP</b>");
		GUILayout.Label("<b>* STOP</b>");
		GUILayout.Label("<b>* HELLO / WAVE</b>");
		GUILayout.Label("<b>For more audio commands,\nlook at the grammar XML file</b>");
		
		GUILayout.EndVertical();
		
		// Make the window draggable.
		GUI.DragWindow();
	}
	
	void OnGUI() {
		GUI.skin = guiSkin;
		guiWindowRect = GUI.Window(0, guiWindowRect, ShowGuiWindow, "<b>AUDIO COMMANDS</b>");
	}
}