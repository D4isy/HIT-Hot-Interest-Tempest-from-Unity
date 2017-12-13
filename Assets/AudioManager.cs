using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance = null;

	public AudioSource efxSource;
	// public AudioClip BackgroundMusic;

	public float lowPitchrange = .95f;
	public float highPitchRange = 1.05f;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		float randomPitch = Random.Range(lowPitchrange, highPitchRange);

		efxSource.pitch = randomPitch;
		// efxSource.clip = BackgroundMusic;
		efxSource.Play();
		efxSource.loop = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
