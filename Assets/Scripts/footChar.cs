using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footChar : MonoBehaviour {

	public float characterSpeed = 0.02f;
	public float leftPosX;
	public float rightPosX;

	// Use this for initialization
	void Start () {
	}

	void FixedUpdate() {
	}

	// Update is called once per frame
	void Update () {

		// 게임 일시 중지
		if (GameManager.instance.gameStop) {
			return;
		}

		if (Input.GetMouseButtonDown (0) ||
			Input.GetKey(KeyCode.LeftArrow) || 
			Input.GetKey(KeyCode.RightArrow) || 
			Input.GetKey(KeyCode.UpArrow) || 
			Input.GetKey(KeyCode.DownArrow)) {
			GameManager.instance.isCharStop = true;
		}

		if (!GameManager.instance.isCharStop) {
			Vector2 charPos = new Vector2(transform.position.x - characterSpeed, transform.position.y);

			if (charPos.x <= leftPosX) {
				transform.Rotate (new Vector3 (0, 180, 0));
				characterSpeed = -characterSpeed;
			} else if (charPos.x >= rightPosX) {
				transform.Rotate (new Vector3 (0, -180, 0));
				characterSpeed = -characterSpeed;
			}

			transform.position = charPos;

			//Debug.Log ("* char: " + transform.position);
			//Debug.Log ("- cam: " + charSprite.size);
		}
	}
}
