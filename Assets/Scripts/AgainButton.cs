using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgainButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void restart() {
		GameManager.instance.GameInit ();
	}
}
