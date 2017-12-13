using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneTwoThreeGameManager : MonoBehaviour {

	public static OneTwoThreeGameManager instance = null;
	public GameObject[] enemy;

	public Text CountText;
	public Text scoreText;

	[HideInInspector] public bool isgameStop = false;
	[HideInInspector] public bool isgameOver = false;
	[HideInInspector] public bool isPlayerCall = false;

	//Canvas
	private GameObject SceneImage;
	private Text SceneText;
	private GameObject AgainButton;
	private GameObject ExitButton;
	private GameObject Menu;

	// enemy callNumber
	private List<float> EnemyCallNumberTick = new List<float>();
	private bool[] isObjectCall = new bool[5];	// [0-3] is Enemy, [4] is Player
	private int[] ObjectCallNumber = new int[5];	// [0-3] is Enemy, [4] is Player

	// tickCount
	private int prevTickCount;
	private int startTickCount;
	private int countDownNumber = 10;
	private int gameLevel = 0;
	private int callNumber = 0;

	// no initialize
	private float checkSecond = 1f;
	private int realGameLevel = 0;

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
		SceneImage = GameObject.Find ("Image");
		SceneText = GameObject.Find("ImageText").GetComponent<Text>();
		AgainButton = GameObject.Find ("AgainButton");
		ExitButton = GameObject.Find ("ExitButton");

		AgainButton.SetActive (false);
		ExitButton.SetActive (false);

		isgameStop = true;
		Invoke ("GameStartText", 2f);
	}
	
	// Update is called once per frame
	void Update () {

		if (isgameStop) {
			return;
		}

		if (isPlayerCall && ObjectCallNumber [ObjectCallNumber.Length - 1] == 0) {
			isPlayerCall = false;
		}

		if (!PrintTimeCount ()) {
			// game end & stop
		}
	}

	bool PrintTimeCount() {
		int countTick = (countDownNumber * 1000) - (gameLevel * 100);
		// count down
		int time = (Environment.TickCount & Int32.MaxValue) - startTickCount;
		float sec = (countTick/1000f) - (time / 1000f);
		if (sec < 0f) {
			// game end
			return false;
		}

		// print Text
		CountText.text = ((int)sec).ToString();

		float tick = ((Environment.TickCount & Int32.MaxValue) - prevTickCount) / 1000f;
		// Debug.Log ((float)(tick) + " >= " + (float)(checkSecond));
		if (tick >= checkSecond) {
			CallNumber (sec);
			prevTickCount = Environment.TickCount & Int32.MaxValue;
		}
		return true;
	}

	public void CallNumberPlayer() {
		int countTick = (countDownNumber * 1000) - (gameLevel * 100);
		int time = (Environment.TickCount & Int32.MaxValue) - startTickCount;
		float sec = (countTick/1000f) - (time / 1000f);
		isPlayerCall = true;
		EnemyCallNumberTick.Add(sec);
	}

	void CallNumber(float time) {
		int number = callNumber;
		bool isCall = false;

		float minimum = time - checkSecond;
		float maximum = time + checkSecond;

		// Call Check
		// Debug.Log (minimum + " <= x <= " + maximum);

		try {
			for (int i = 0; i < EnemyCallNumberTick.Count; i++) {
				if (isObjectCall [i])
					continue;
				
				if (minimum <= EnemyCallNumberTick [i] && EnemyCallNumberTick[i] <= maximum) {
					ObjectCallNumber [i] = number + 1;
					Debug.Log ("[" + i + "]: " + (number+1) + ": " + EnemyCallNumberTick[i] );
					isObjectCall [i] = true;
					isCall = true;
				}
			}
		} catch (IndexOutOfRangeException e) {
			isCall = true;
		}

		// Debug.Log ("retry!");

		// increase Call-Number.
		if (isCall) {
			callNumber++;
		} else {
			return;
		}
			
		// initialize Who's Call
		for (int i = 0; i < ObjectCallNumber.Length; i++) {
			if (ObjectCallNumber [i] != 0) {
				// say number!

			}
		}

		bool isSayPlayer = false;
		bool isSayDupplicate = false;
		for (int i = 0; i < ObjectCallNumber.Length; i++) {
			if (ObjectCallNumber [i] == 0)
				continue;
			
			for (int j = i + 1; j < ObjectCallNumber.Length; j++) {
				if (ObjectCallNumber [i] == ObjectCallNumber [j]) {
					isSayDupplicate = true;
					if (i == (ObjectCallNumber.Length-1) || j == (ObjectCallNumber.Length-1)) {	// player?
						isSayPlayer = true;
					}
				}
			}
		}

		if (isSayDupplicate) {
			if (isSayPlayer) {
				// Game Over
				Debug.Log("게임 끝남!");
				isgameStop = true;
				isgameOver = true;
				GameEndText ();
			} else {
				// Next Game
				Debug.Log("중복자 나옴!");
				if (gameLevel < 50)
					gameLevel++;
				if (checkSecond > 0.1f)
					checkSecond -= 0.1f;
				isgameStop = true;
				SceneImage.SetActive (true);
				SceneText.text = "Next Level " + realGameLevel + "!";
				Invoke ("GameNextGameText", 2f);
			}
			return;
		}

		if (ObjectCallNumber [ObjectCallNumber.Length - 1] == 5) {
			Debug.Log("게임 끝남!");
			isgameStop = true;
			isgameOver = true;
			GameEndText ();
			return;
		}

		bool isAnybodyCall = true;
		for (int i = 0; i < ObjectCallNumber.Length - 1; i++) {
			if (ObjectCallNumber [i] == 0) {
				isAnybodyCall = false;
			}
		}

		if (!isAnybodyCall && ObjectCallNumber [ObjectCallNumber.Length - 1] != 0) {
			Debug.Log ("다음 게임!");
			if (gameLevel < 50)
				gameLevel++;
			if (checkSecond > 0.1f)
				checkSecond -= 0.1f;
			isgameStop = true;
			SceneImage.SetActive (true);
			SceneText.text = "Next Level " + realGameLevel + "!";
			Invoke ("GameNextGameText", 2f);
		} else if (isAnybodyCall && ObjectCallNumber [ObjectCallNumber.Length - 1] == 0) {
			Debug.Log ("게임 끝남!");
			isgameStop = true;
			isgameOver = true;
			GameEndText ();
			return;
		} else {
			for (int i = 0; i < ObjectCallNumber.Length - 1; i++) {
				if (ObjectCallNumber [i] == 5) {
					Debug.Log ("다음 게임!");
					if (gameLevel < 50)
						gameLevel++;
					if (checkSecond > 0.1f)
						checkSecond -= 0.1f;
					isgameStop = true;
					SceneImage.SetActive (true);
					SceneText.text = "Next Level " + realGameLevel + "!";
					Invoke ("GameNextGameText", 2f);
				}
			}
		}
	}

	public void PrintScore() {
		scoreText.text = (realGameLevel*10).ToString();
	}

	public void SceneStart(bool isReset) {

		// isgameStop = false;

		// initilize
		for (int i = 0; i < isObjectCall.Length; i++) {
			isObjectCall [i] = false;
		}

		for (int i=0; i<ObjectCallNumber.Length; i++) {
			ObjectCallNumber [i] = 0;
		}

		EnemyCallNumberTick.Clear ();

		prevTickCount = 0;
		startTickCount = Environment.TickCount & Int32.MaxValue;
		countDownNumber = 10;
		gameLevel = 0;
		callNumber = 0;
		isPlayerCall = false;

		float random;
		float countTick = countDownNumber - (gameLevel / 10f) - 0.1f;
		for (int i = 0; i < 4; i++) {
			random = UnityEngine.Random.Range (0f, countTick);
			// random = UnityEngine.Random.Range (8f, 10f);
			EnemyCallNumberTick.Add (random);
			// Debug.Log("[" + i + "] initialize (" + random + ")");
		}

		if (isReset) {
			checkSecond = 1f;
			realGameLevel = 0;
		}

		// temp
		// gameLevel = 50;
		// checkSecond = 0.1f;

		PrintScore();
		realGameLevel++;

		AgainButton.SetActive (false);
		ExitButton.SetActive (false);
	}

	public void GameStartText() {
		SceneText.text = "Game Start";
		SceneStart(true);
		SceneImage.SetActive (false);
		isgameStop = false;
	}

	public void GameNextGameText() {
		PrintScore();
		SceneImage.SetActive (false);
		isgameStop = false;
		SceneStart (false);
	}

	public void GameEndText() {
		SceneText.text = "You have " + ((realGameLevel-1)*10) + " score\n\nRetry?";
		isgameStop = true;
		AgainButton.SetActive (true);
		ExitButton.SetActive (true);
		SceneImage.SetActive (true);

		for (int i = 0; i < isObjectCall.Length; i++) {
			isObjectCall [i] = false;
		}
	}

	private void OnGUI() {
		GUIStyle style = new GUIStyle ();
		style.richText = true;

		// GUI.Label(new Rect(500,285,75,25),"adsfa1f5as6f1as56123123");

		if (isObjectCall [0]) {
			GUILayout.BeginArea (new Rect (1000, 575, 60, 160)); 
			GUILayout.Label ("<size=40><color=green>" + ObjectCallNumber[0] + "</color></size>", style);
			//GUILayout.Label ("<size=30>Some <color=yellow>RICH</color> text</size>", style);
			//GUILayout.Label ("Hello World!"); 
			GUILayout.EndArea (); 
		}

		if (isObjectCall [1]) {
			GUILayout.BeginArea (new Rect (1220, 575, 60, 160)); 
			GUILayout.Label ("<size=40><color=green>" + ObjectCallNumber[1] + "</color></size>", style);
			GUILayout.EndArea (); 
		}

		if (isObjectCall [2]) {
			GUILayout.BeginArea (new Rect (1320, 575, 60, 160)); 
			GUILayout.Label ("<size=40><color=green>" + ObjectCallNumber[2] + "</color></size>", style);
			GUILayout.EndArea (); 
		}

		if (isObjectCall [3]) {
			GUILayout.BeginArea (new Rect (980, 675, 60, 160)); 
			GUILayout.Label ("<size=40><color=green>" + ObjectCallNumber[3] + "</color></size>", style);
			GUILayout.EndArea (); 
		}

		if (isObjectCall [4]) {
			GUILayout.BeginArea (new Rect (1260, 675, 60, 160)); 
			GUILayout.Label ("<size=40><color=red>" + ObjectCallNumber[4] + "</color></size>", style);
			GUILayout.EndArea (); 
		}
	}
}
