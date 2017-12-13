using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

	public static MainManager instance = null;
	public GameObject backGround;

	// private GameObject Menu;

	// [HideInInspector] public bool isExit = false;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		// DontDestroyOnLoad (gameObject);
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Screen.SetResolution (1630, 980, false);
	}

	// Use this for initialization
	void Start () {
		// GameInit ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	// public void GameInit() {
	//	Debug.Log ("Init!");
	//	if (Menu == null) {
	//		Debug.Log ("Menu null!");
	//		Menu = GameObject.Find ("ExitMenu");
	//	}
	//	isExit = false;
	// }

	// public void SetExitMenuActive(bool isShow) {
	//	isExit = isShow;
	// }
}
