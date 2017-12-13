using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgainButton4 : MonoBehaviour {

	private GameObject AgainButton;
	private GameObject ExitButton;

	// Use this for initialization
	void Start () {
		AgainButton = GameObject.Find ("AgainButton");
		ExitButton = GameObject.Find ("ExitButton");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void restart() {
		Rock_paper_scissorsGameManager.instance.GameStartText ();
		AgainButton.SetActive (false);
		ExitButton.SetActive (false);
		Rock_paper_scissorsGameManager.instance.ResetStatus ();
	}
}
