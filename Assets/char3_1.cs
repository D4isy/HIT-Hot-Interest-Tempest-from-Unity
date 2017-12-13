using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class char3_1 : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (OneTwoThreeGameManager.instance.isgameStop)
			return;
		
		if (OneTwoThreeGameManager.instance.isPlayerCall)
			return;

		if (Input.GetMouseButtonDown (0) ||
			Input.GetKey(KeyCode.LeftArrow) || 
			Input.GetKey(KeyCode.RightArrow) || 
			Input.GetKey(KeyCode.UpArrow) || 
			Input.GetKey(KeyCode.DownArrow)) {
			OneTwoThreeGameManager.instance.CallNumberPlayer ();
		}
	}
}
