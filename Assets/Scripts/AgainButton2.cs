﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgainButton2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void restart() {
		SwordGameManager.instance.GameInit ();
	}
}
