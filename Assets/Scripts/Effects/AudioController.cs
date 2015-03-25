using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	public static AudioController ac;

	public AudioSource music;
	public AudioSource soundEffect;

	public AudioClip gunshot;
	public AudioClip spawn;
	public AudioClip explosion;

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
		soundEffect.PlayOneShot (gunshot, 0.4f);
	}

	public void PlaySpawnNoise() {
		soundEffect.PlayOneShot (spawn, 0.5f);
	}

	public void PlayDeathNoise() {
		soundEffect.PlayOneShot (explosion, 0.5f);
	}
}
