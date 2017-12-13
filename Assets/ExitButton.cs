using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ButtonClick() {
		// MainManager.instance.SetExitMenuActive (true);
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		// #elif UNITY_WEBPLAYER
		//	Application.OpenURL("http://google.com");
		#else
		Application.Quit ();
		#endif
	}
}
