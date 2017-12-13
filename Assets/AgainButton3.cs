using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgainButton3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void restart() {
		OneTwoThreeGameManager.instance.isgameStop = true;
		Invoke ("call", 2f);
	}

	public void call() {
		OneTwoThreeGameManager.instance.GameStartText ();
	}
}
