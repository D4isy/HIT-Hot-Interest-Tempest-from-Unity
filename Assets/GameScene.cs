using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FootClickButton() {
		SceneManager.LoadScene (1);
		// MainManager.instance.backGround.SetActive (false);
	}

	public void SwordClickButton() {
		SceneManager.LoadScene (2);
		// MainManager.instance.backGround.SetActive (false);
	}

	public void OneTwoThreeClickButton() {
		SceneManager.LoadScene (3);
		// MainManager.instance.backGround.SetActive (false);
	}

	public void RockClickButton() {
		SceneManager.LoadScene (4);
		// MainManager.instance.backGround.SetActive (false);
	}
}
