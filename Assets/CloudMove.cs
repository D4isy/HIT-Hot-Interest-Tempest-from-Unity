using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour {

	private float speed;
	private SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		speed = 0.015f;
		sprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		// end of screen
		if (sprite.transform.position.x >= 11f) {
			sprite.transform.position = new Vector2 (-11f, sprite.transform.position.y);
		} else {
			sprite.transform.position = new Vector2 (sprite.transform.position.x + speed, sprite.transform.position.y);
		}
	}
}
