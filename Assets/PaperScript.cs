using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperScript : MonoBehaviour {

	private GameObject MenuObject;

	// Use this for initialization
	void Start () {
		MenuObject = GameObject.Find("Menu");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MouseClick() {
		if (!Rock_paper_scissorsGameManager.instance.isPlayerCall) {
			Rock_paper_scissorsGameManager.instance.SetPlayerStatus (3);
			MenuObject.SetActive (false);
		}
	}
}
