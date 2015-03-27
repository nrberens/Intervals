﻿using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	public static AudioController ac;

	public AudioSource music;
	public AudioSource soundEffect;

	public AudioClip gunshot;
	public AudioClip spawn;
	public AudioClip explosion;
	public AudioClip playerMove;
	public AudioClip enemyMove;

	// Use this for initialization
	void Start () {
		if(ac == null) {
			DontDestroyOnLoad(gameObject);
			ac = this;
		} else if (ac != this) {
			Destroy (gameObject);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayGunshot() {
		soundEffect.PlayOneShot (gunshot, 0.7f);
	}

	public void PlaySpawnNoise() {
		soundEffect.PlayOneShot (spawn, 1.0f);
	}

	public void PlayDeathNoise() {
		soundEffect.PlayOneShot (explosion, 0.5f);
	}

	public void PlayPlayerMoveNoise(){
		soundEffect.PlayOneShot (playerMove, 1.0f);
	}

	public void PlayEnemyMoveNoise() {
		soundEffect.PlayOneShot(enemyMove, 1.0f);
	}
}
