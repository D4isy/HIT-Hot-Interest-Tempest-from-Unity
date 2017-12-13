using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordGameManager : MonoBehaviour {

	public static SwordGameManager instance = null;

	public int decreaseCreateTime = 50;
	public int maxEnemyCount = 15;

	public Text scoreText;
	public GameObject enemy;
	public GameObject player;
	public GameObject map;

	[HideInInspector] public int gameScore = 0;
	[HideInInspector] public bool isgameStop = false;
	[HideInInspector] public bool ischarAttack = false;
	[HideInInspector] public bool ischarLeft = true;

	private GameObject SceneImage;
	private Text SceneText;
	private GameObject AgainButton;
	private GameObject ExitButton;

	private int startTickCount;
	private int leftTickCount;		// for create left enemy
	private int rightTickCount;		// for create right enemy
	private int createTime;

	private float normalSpeed = 0.05f;
	private float fastSpeed = 0.065f;

	private Vector3 leftPos;
	private Quaternion leftRotation;

	private Vector3 rightPos;
	private Quaternion rightRotation;

	private Vector3 charPos;

	private List<ObjectManager> enemyList = new List<ObjectManager>();

	[Serializable]
	public class ObjectManager {

		public bool isLeft;
		public float speed;
		public GameObject instance;

		public ObjectManager(GameObject instance, float speed, bool left) {
			this.isLeft = left;
			this.instance = instance;
			this.speed = speed;
		}
	}

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

		isgameStop = true;

		// figure in initialize TickCount
		startTickCount = Environment.TickCount & Int32.MaxValue;
		leftTickCount = 0;
		rightTickCount = 0;
		createTime = 5000;	// 5 sec

		// figure in initialize enemy's position
		leftPos = new Vector3(-4.36f, -1.27f, 0f);
		leftRotation = Quaternion.identity;
		rightPos = new Vector3(4.41f, -1.27f, 0f);
		rightRotation = new Quaternion(0f, 180f, 0f, 0f);

		// figure in initialize character's position
		charPos = new Vector3(0.04f, -1.43f, 0f);

		enemyList.Clear ();

		SceneImage = GameObject.Find ("Image");
		SceneText = GameObject.Find("ImageText").GetComponent<Text>();
		AgainButton = GameObject.Find ("AgainButton");
		ExitButton = GameObject.Find ("ExitButton");
		AgainButton.SetActive (false);
		ExitButton.SetActive (false);

		Invoke ("GameStartText", 2f);
	}
	
	// Update is called once per frame
	void Update () {
		ObjectManager dataEnemy = null;
		Vector3 movePos;

		if (isgameStop) {
			return;
		}

		for (int i = 0; i < enemyList.Count; i++) {
			dataEnemy = enemyList[i];
			if (dataEnemy != null) {
				if (dataEnemy.isLeft) {
					// move right
					movePos = new Vector3(dataEnemy.speed, 0f, 0f);
					dataEnemy.instance.transform.position += movePos;
				} else {
					// move left
					movePos = new Vector3(dataEnemy.speed, 0f, 0f);
					dataEnemy.instance.transform.position -= movePos;
				}
			}
		}
	}

	void FixedUpdate () {
		GameObject instance;

		if (isgameStop) {
			return;
		}

		// Debug.Log ("Enemy Count: " + enemyList.Count);
		if (enemyList.Count < maxEnemyCount) {
			if (isLeftCreate ()) {
				instance = Instantiate (enemy, leftPos, leftRotation) as GameObject;

				int speedRandom = UnityEngine.Random.Range (0, 100);
				if (speedRandom < 5) {
					enemyList.Add (new ObjectManager (instance, fastSpeed, true));
				} else {
					enemyList.Add (new ObjectManager (instance, normalSpeed, true));
				}
			}
		} else {
			// 생성 되고 꽉 찬 후에 관리하기
			int idx = GetDontActiveEnemy();
			if (idx == -1) {
				return;
			}

			if (isLeftCreate ()) {
				int speedRandom = UnityEngine.Random.Range (0, 100);
				if (speedRandom < 5) {
					enemyList [idx].speed = fastSpeed;
				} else {
					enemyList [idx].speed = normalSpeed;
				}
				enemyList [idx].isLeft = true;
				enemyList [idx].instance.transform.position = leftPos;
				enemyList [idx].instance.transform.rotation = leftRotation;
				enemyList [idx].instance.SetActive (true);
			}
		}

		if (enemyList.Count < maxEnemyCount) {
			if (isRightCreate ()) {
				instance = Instantiate (enemy, rightPos, rightRotation) as GameObject;

				int speedRandom = UnityEngine.Random.Range (0, 100);
				if (speedRandom < 5) {
					enemyList.Add (new ObjectManager (instance, fastSpeed, false));
				} else {
					enemyList.Add (new ObjectManager (instance, normalSpeed, false));
				}
			}
		} else {
			// 생성 되고 꽉 찬 후에 관리하기
			int idx = GetDontActiveEnemy();
			if (idx == -1) {
				return;
			}

			if (isRightCreate ()) {
				int speedRandom = UnityEngine.Random.Range (0, 100);
				if (speedRandom < 5) {
					enemyList [idx].speed = fastSpeed;
				} else {
					enemyList [idx].speed = normalSpeed;
				}
				enemyList [idx].isLeft = false;
				enemyList [idx].instance.transform.position = rightPos;
				enemyList [idx].instance.transform.rotation = rightRotation;
				enemyList [idx].instance.SetActive (true);
			}
		}
	}

	private int GetDontActiveEnemy() {
		ObjectManager dataEnemy = null;

		for (int i = 0; i < enemyList.Count; i++) {
			dataEnemy = enemyList[i];
			if (dataEnemy.instance.active == false) {
				return i;
			}
			// Debug.Log("[" + i + "]active: " + dataEnemy.instance.active);
		}

		return -1;
	}

	private bool isRightCreate() {
		bool isCreate = false;
		int TickCount = Environment.TickCount & Int32.MaxValue;

		// Debug.Log ("Now Tick: " + (TickCount - rightTickCount) + " >= " + createTime);

		// TickCount - startTickCount 의 값이 증가 할 때마다 생성 속도가 빨라짐
		if ((TickCount - rightTickCount) >= createTime) {
			// createTime이 지났다면..
			int randomInt = UnityEngine.Random.Range (0, 100);
			rightTickCount = TickCount;
			if (randomInt < 70) {	// 30% 확률로 생성 안함
				isCreate = true;
				// increasingly fast speed
				normalSpeed += 0.0005f;
				fastSpeed += 0.0005f;
				if (createTime > 1000) {	// upto 1sec
					createTime -= decreaseCreateTime;
				}
			}

			randomInt = UnityEngine.Random.Range (0, 100);
			if (randomInt < 30) {	// 30% 확률
				rightTickCount = TickCount - createTime + UnityEngine.Random.Range (300, 500);
			}
		}

		return isCreate;
	}

	private bool isLeftCreate() {
		bool isCreate = false;
		int TickCount = Environment.TickCount & Int32.MaxValue;

		// Debug.Log ("Now Tick: " + (TickCount - rightTickCount) + " >= " + createTime);

		// TickCount - startTickCount 의 값이 증가 할 때마다 생성 속도가 빨라짐
		if ((TickCount - leftTickCount) >= createTime) {
			// createTime이 지났다면..
			int randomInt = UnityEngine.Random.Range (0, 100);
			leftTickCount = TickCount;
			if (randomInt < 70) {	// 30% 확률로 생성 안함
				isCreate = true;
				// increasingly fast speed
				normalSpeed += 0.0005f;
				fastSpeed += 0.0005f;
				if (createTime > 1000) {	// upto 1sec
					createTime -= decreaseCreateTime;
				}
			}

			randomInt = UnityEngine.Random.Range (0, 100);
			if (randomInt < 30) {	// 30% 확률
				rightTickCount = TickCount - createTime + UnityEngine.Random.Range (300, 500);
			}
		}

		return isCreate;
	}

	public void GameStartText() {
		SceneText.text = "Game Start";
		SceneImage.SetActive (false);
		isgameStop = false;
	}

	public void GameEndText() {
		SceneText.text = "You have " + gameScore + " score\n\nRetry?";
		isgameStop = true;
		AgainButton.SetActive (true);
		ExitButton.SetActive (true);
		SceneImage.SetActive (true);
	}

	public void PrintText() {
		scoreText.text = gameScore.ToString();
	}

	private void OnTriggerEnter2D(Collider2D other) {
	}

	public void GameInit() {

		char2_1.instance.InitAttack ();
		ischarLeft = true;

		gameScore = 0;
		normalSpeed = 0.05f;
		fastSpeed = 0.065f;

		// figure in initialize TickCount
		startTickCount = Environment.TickCount & Int32.MaxValue;
		leftTickCount = 0;
		rightTickCount = 0;
		createTime = 5000;	// 5 sec

		// figure in initialize enemy's position
		leftPos = new Vector3(-4.36f, -1.27f, 0f);
		leftRotation = Quaternion.identity;
		rightPos = new Vector3(4.41f, -1.27f, 0f);
		rightRotation = new Quaternion(0f, 180f, 0f, 0f);

		// figure in initialize character's position
		charPos = new Vector3(0.04f, -1.43f, 0f);

		for (int i = 0; i < enemyList.Count; i++) {
			enemyList [i].instance.SetActive (false);
		}

		AgainButton.SetActive (false);
		ExitButton.SetActive (false);

		GameStartText();
		PrintText ();
	}
}
