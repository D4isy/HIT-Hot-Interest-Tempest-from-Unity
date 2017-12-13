using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char2_1 : MonoBehaviour {

	public static char2_1 instance = null;

	private bool isAttack = false;
	private Rigidbody2D rb2d;
	private Animator anim;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (SwordGameManager.instance.isgameStop) {
			return;
		}

		if (isAttack) {
			return;
		}

		if (Input.GetMouseButtonDown (0) || Input.GetKey(KeyCode.LeftArrow)) {
			SwordGameManager.instance.ischarLeft = true;
			transform.position = new Vector3 (-0.35f, -1.41f, 0);
			transform.rotation = new Quaternion (0, 0, 0, 0);
			anim.SetTrigger ("char2-1");
			isAttack = true;
		} else if (Input.GetMouseButtonDown (1) || Input.GetKey(KeyCode.RightArrow)) {
			SwordGameManager.instance.ischarLeft = false;
			transform.position = new Vector3 (0.45f, -1.41f, 0);
			transform.rotation = new Quaternion (0, 180, 0, 0);
			anim.SetTrigger ("char2-1");
			isAttack = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// Debug.Log (other.tag);

		if (other.gameObject.CompareTag ("Enemy")) {
			bool isAttack = SwordGameManager.instance.ischarAttack;
			bool isLeft = SwordGameManager.instance.ischarLeft;

			if (other.gameObject.transform.position.x <= transform.position.x) {
				// left enemy
				if (isAttack && isLeft) {
					// score 추가
					SwordGameManager.instance.gameScore += 10;
					SwordGameManager.instance.PrintText ();

					// retry possible attack
					this.isAttack = false;

					// Enemy Object 제거
					other.gameObject.SetActive (false);
				} else {
					SwordGameManager.instance.GameEndText ();
				}
			} else {
				// right enemy
				if (isAttack && !isLeft) {
					// score 추가
					SwordGameManager.instance.gameScore += 10;
					SwordGameManager.instance.PrintText ();

					// retry possible attack
					this.isAttack = false;

					// Enemy Object 제거
					other.gameObject.SetActive (false);
				} else {
					SwordGameManager.instance.GameEndText ();
				}
			}

			// List에 있는 Data도 제거
			// 하면 안되는 reason: because of Memory.
		}
	}

	public void InitAttack() {
		this.isAttack = false;
	}
}
