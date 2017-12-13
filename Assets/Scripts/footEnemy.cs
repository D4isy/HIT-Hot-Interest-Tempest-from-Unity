using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footEnemy : MonoBehaviour {

	public GameObject circle;
	public float characterSpeed = 0.02f;
	public float leftPosX;
	public float rightPosX;

	private Rigidbody2D rb2d;
	private bool isLeftMode = true;
	private bool footUp = false;
	private bool footDown = false;
	private float footDownSpeed = 3f;
	private float blockPosY = 1.4f;
	private float startPosY;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		startPosY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

		// 게임 일시 중지
		if (GameManager.instance.gameStop) {
			return;
		}

		// Debug.Log (GameManager.instance.isFootStop + ", " + footDown + ", " + footUp);
		if (footUp) {
			if (transform.position.y <= startPosY) {
				// transform.position = new Vector2 (transform.position.x, transform.position.y + characterSpeed);
			} else {
				rb2d.gravityScale = 0f;
				rb2d.Sleep ();
				// 점수 증가!
				GameManager.instance.gameScore += 10;
				// 점수 표지판 보여주기
				// 점수 표시
				GameManager.instance.PrintText();
				GameManager.instance.isFootStop = false;
				GameManager.instance.isCharStop = false;
				if (GameManager.instance.dist >= 0.5f) {
					GameManager.instance.dist -= 0.1f;
				}
				if (footDownSpeed >= 0.5f) {
					footDownSpeed -= 0.1f;
				}
				footUp = false;
			}
		} else if (GameManager.instance.isFootStop == true) {
			if (!footDown) {
				circle.SetActive (true);
				Vector2 tmp = new Vector2 (transform.position.x, circle.transform.position.y);
				circle.transform.position = tmp;
				Invoke ("SetGravityByEnemy", footDownSpeed);
				footDown = true;
			}

			if (transform.position.y <= blockPosY) {
				rb2d.gravityScale = -1f;
				circle.SetActive (false);
				footUp = true;
				footDown = false;
				transform.position = new Vector2 (transform.position.x, 1.4f);
			}
		} else {
			Vector2 charPos = new Vector2 (transform.position.x - characterSpeed, transform.position.y);

			if (charPos.x <= leftPosX) {
				isLeftMode = false;
				GameManager.instance.enemyHitBlock++;
				transform.Rotate (new Vector3 (0, 180, 0));
				characterSpeed = -characterSpeed;
			} else if (charPos.x >= rightPosX) {
				isLeftMode = true;
				GameManager.instance.enemyHitBlock++;
				transform.Rotate (new Vector3 (0, -180, 0));
				characterSpeed = -characterSpeed;
			}

			Quaternion tmp;
			if (isLeftMode == false)
				tmp = new Quaternion(0f, 180f, 0f, 0f);
			else
				tmp = new Quaternion(0f, 0f, 0f, 0f);
			transform.rotation = tmp;
			transform.position = charPos;
		}
	}

	void SetGravityByEnemy() {
		rb2d.gravityScale = 1.0f;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			// 게임 멈추기
			GameManager.instance.gameStop = true;

			// 설정 초기화
			transform.position = new Vector2 (transform.position.x, startPosY);
			isLeftMode = true;
			footUp = false;
			footDown = false;
			footDownSpeed = 3f;
			blockPosY = 1.4f;
			rb2d.gravityScale = 0f;
			rb2d.Sleep ();

			// Game Over
			GameManager.instance.GameEndText();
		}
	}
}
