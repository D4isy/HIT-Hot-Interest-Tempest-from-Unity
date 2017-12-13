using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public Text scoreText;
	public GameObject player;
	public GameObject enemy;
	public float dist = 3.5f;

	[HideInInspector] public bool gameStop = true;
	[HideInInspector] public int gameScore = 0;
	[HideInInspector] public int enemyHitBlock = 0;
	[HideInInspector] public bool isFootStop = false;
	[HideInInspector] public bool isCharStop = false;
	[HideInInspector] public bool isgameOver = false;
	private bool isCheckRange = false;
	private GameObject SceneImage;
	private Text SceneText;

	// private GameObject Menu;
	private GameObject AgainButton;
	private GameObject ExitButton;

	private Vector3 playerPosition;
	private Vector3 enemyPosition;
	private int limitTime;

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
		playerPosition = player.transform.position;

		enemy.transform.position = new Vector3 (player.transform.position.x-dist, enemy.transform.position.y);
		enemyPosition = enemy.transform.position;
		limitTime = Random.Range (0, 10);

		SceneImage = GameObject.Find ("Image");
		SceneText = GameObject.Find("ImageText").GetComponent<Text>();
		AgainButton = GameObject.Find ("AgainButton");
		ExitButton = GameObject.Find ("ExitButton");
		// Menu = GameObject.Find ("ExitMenu");
		// Menu.SetActive (false);
		AgainButton.SetActive (false);
		ExitButton.SetActive (false);

		Invoke ("GameStartText", 2f);
	}
	
	// Update is called once per frame
	void Update () {

		if (gameStop) {
			return;
		}

		// Debug.Log (dist + " : " + Mathf.Abs (player.transform.position.x - enemy.transform.position.x));
		// Debug.Log((!isFootStop && isCharStop) + ", " + Mathf.Abs (player.transform.position.x - enemy.transform.position.x));
		// Vector3 pos = player.transform.position;
		if (!isFootStop && isCharStop) {
			if (Mathf.Abs (player.transform.position.x - enemy.transform.position.x) <= 1f) {
				isFootStop = true;
				enemyHitBlock = 0;
			}
		}

		if (enemyHitBlock >= 5) {
			Invoke ("FootDown", Random.Range(1, 3));
		}
		if (Mathf.Abs (player.transform.position.x - enemy.transform.position.x) <= dist) {
			if (!isCheckRange && limitTime > 3) {
				isFootStop = true;
				enemyHitBlock = 0;
			}
			limitTime = Random.Range (0, 10);
			isCheckRange = true;
		} else {
			isCheckRange = false;
		}
		// enemy.transform.position = new Vector3 (player.transform.position.x-dist, enemy.transform.position.y);
	}

	public void GameInit() {
		dist = 3.5f;

		gameStop = true;
		enemyHitBlock = 0;
		isFootStop = false;
		isCharStop = false;
		isgameOver = false;
		isCheckRange = false;

		player.transform.position = playerPosition;
		enemy.transform.position = enemyPosition;

		enemy.transform.position = new Vector3 (player.transform.position.x-dist, enemy.transform.position.y);
		limitTime = Random.Range (0, 10);
		AgainButton.SetActive (false);
		ExitButton.SetActive (false);
		gameScore = 0;
		PrintText ();

		GameStartText();
	}

	public void GameStartText() {
		SceneText.text = "Game Start";
		SceneImage.SetActive (false);
		gameStop = false;
	}

	public void GameEndText() {
		SceneText.text = "You have " + gameScore + " score\n\nRetry?";
		gameStop = true;
		AgainButton.SetActive (true);
		ExitButton.SetActive (true);
		SceneImage.SetActive (true);
	}

	void FootDown() {
		isFootStop = true;
		enemyHitBlock = 0;
		limitTime = Random.Range (0, 10);
	}

	public void PrintText() {
		scoreText.text = gameScore.ToString();
	}
}
