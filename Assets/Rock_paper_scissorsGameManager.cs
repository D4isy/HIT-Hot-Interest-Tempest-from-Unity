using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rock_paper_scissorsGameManager : MonoBehaviour {

	public static Rock_paper_scissorsGameManager instance = null;

	enum GAMESTATUS { NONE=0, ROCK=1, SCISSORS=2, PAPER=3 };

	public Text CountText;
	public Text scoreText;
	public GameObject MenuObject;

	[HideInInspector] public bool isgameStop = false;
	[HideInInspector] public bool isPlayerCall = false;

	private GameObject SceneImage;
	private Text SceneText;
	private GameObject AgainButton;
	private GameObject ExitButton;

	private GameObject enemyEmotion = null;
	private Animator enemyAnim = null;

	private GameObject playerEmotion = null;
	private Animator playerAnim = null;

	private GAMESTATUS enemyStatus;
	private GAMESTATUS playerStatus;

	// tickcount
	private int score = 0;
	private int startTickCount;
	private int countDownNumber = 5;

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
		enemyEmotion = GameObject.Find("emotion1");
		enemyAnim = GameObject.Find ("emotion1").GetComponent<Animator> ();
		playerEmotion = GameObject.Find("emotion2");
		playerAnim = GameObject.Find ("emotion2").GetComponent<Animator> ();

		MenuObject.SetActive (false);
		// temp //
		SceneStart(true);
		//////////
		SceneImage = GameObject.Find ("Image");
		SceneText = GameObject.Find("ImageText").GetComponent<Text>();
		AgainButton = GameObject.Find ("AgainButton");
		ExitButton = GameObject.Find ("ExitButton");

		AgainButton.SetActive (false);
		ExitButton.SetActive (false);

		// isgameStop = true;
		Invoke ("GameStartText", 2f);
	}
	
	// Update is called once per frame
	void Update () {

		if (isgameStop) {
			return;
		}

		// Player 5초 CountDown
		if (!PrintTimeCount () || isPlayerCall) {
			// game end & stop
			if (!isPlayerCall) {
				playerStatus = GetPlayerRandom ();
			}

			// Set animator trigger
			SetEnemyTrigger(enemyStatus);
			SetPlayerTrigger(playerStatus);

			// check win & lose
			int status = isPlayerWin (enemyStatus, playerStatus);
			switch (status) {
			case -1:	// lost
				Debug.Log ("lose..");
				isgameStop = true;
				Invoke ("GameEndText", 2f);
				break;

			case 0:		// same
				Debug.Log ("oops!");
				isgameStop = true;
				// restart
				Invoke ("InvokeSceneStart", 2f);
				Invoke ("ResetStatus", 2f);
				break;

			case 1:		// win
				Debug.Log ("win!!!");
				score = score + 10;
				PrintScore ();
				isgameStop = true;

				SceneText.text = "Next Level " + (score/10) + "!";
				Invoke ("ShowNextLevel", 2f);
				Invoke ("GameNextGameText", 4f);
				Invoke ("ResetStatus", 4f);
				break;
			}

		}
	}

	public void ShowNextLevel() {
		SceneImage.SetActive (true);
	}

	public void PrintScore() {
		scoreText.text = score.ToString();
	}

	public void SetPlayerStatus(int status) {
		switch (status) {
		case 1:
			playerStatus = GAMESTATUS.ROCK;
			break;
		case 2:
			playerStatus = GAMESTATUS.SCISSORS;
			break;
		case 3:
			playerStatus = GAMESTATUS.PAPER;
			break;
		default:
			playerStatus = GAMESTATUS.NONE;
			break;
		}
		isPlayerCall = true;
	}

	void SetEnemyTrigger(GAMESTATUS status) {
		switch (status) {
		case GAMESTATUS.PAPER:
			enemyAnim.SetTrigger ("paper");
			break;
		case GAMESTATUS.ROCK:
			enemyAnim.SetTrigger ("rock");
			break;
		case GAMESTATUS.SCISSORS:
			enemyAnim.SetTrigger ("scissors");
			break;
		default:
			enemyAnim.SetTrigger ("emotion1");
			break;
		}
	}

	void SetPlayerTrigger(GAMESTATUS status) {
		switch (status) {
		case GAMESTATUS.PAPER:
			playerAnim.SetTrigger ("paper");
			break;
		case GAMESTATUS.ROCK:
			playerAnim.SetTrigger ("rock");
			break;
		case GAMESTATUS.SCISSORS:
			playerAnim.SetTrigger ("scissors");
			break;
		default:
			playerAnim.SetTrigger ("emotion1");
			break;
		}
	}

	GAMESTATUS GetPlayerRandom() {
		return (GAMESTATUS)UnityEngine.Random.Range(1, 4);
	}

	bool PrintTimeCount() {
		// count down
		int time = (Environment.TickCount & Int32.MaxValue) - startTickCount;
		float sec = countDownNumber - (time / 1000f);
		if (sec <= 0f) {
			// game end
			return false;
		}

		// print Text
		CountText.text = ((int)sec).ToString();
		return true;
	}

	public void ResetStatus() {
		SetEnemyTrigger(GAMESTATUS.NONE);
		SetPlayerTrigger(GAMESTATUS.NONE);
	}

	void InvokeSceneStart() {
		SceneStart (false);
	}

	void SceneStart(bool isReset) {

		playerStatus = GAMESTATUS.NONE;

		int rand = UnityEngine.Random.Range (1, 4);
		switch (rand) {
		case 1:
			enemyStatus = GAMESTATUS.ROCK;
			break;
		case 2:
			enemyStatus = GAMESTATUS.SCISSORS;
			break;
		case 3:
			enemyStatus = GAMESTATUS.PAPER;
			break;
		default:
			enemyStatus = GAMESTATUS.ROCK;
			break;
		}

		isPlayerCall = false;

		startTickCount = Environment.TickCount & Int32.MaxValue;
		countDownNumber = 5;
		if (isReset) {
			score = 0;
		}
		PrintScore ();

		MenuObject.SetActive (true);

		isgameStop = false;
	}

	int isPlayerWin(GAMESTATUS enemyStatus, GAMESTATUS playerStatus) {
		if (enemyStatus == playerStatus) {
			// not win
			return 0;
		} else {
			switch (enemyStatus) {
			case GAMESTATUS.PAPER:
				if (playerStatus == GAMESTATUS.SCISSORS) {	// win
					return 1;
				} else {	// lost
					return -1;
				}

			case GAMESTATUS.ROCK:
				if (playerStatus == GAMESTATUS.SCISSORS) {	// lost
					return -1;
				} else {	// win
					return 1;
				}

			case GAMESTATUS.SCISSORS:
				if (playerStatus == GAMESTATUS.ROCK) {	// win
					return 1;
				} else {	// lost
					return -1;
				}
			}
		}

		// 응??? 버그...
		return 0;
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
		SceneText.text = "You have " + score + " score\n\nRetry?";
		isgameStop = true;
		AgainButton.SetActive (true);
		ExitButton.SetActive (true);
		SceneImage.SetActive (true);
	}
}
