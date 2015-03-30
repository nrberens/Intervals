using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	public static AudioController ac;

	public AudioSource MusicSource;
	public AudioSource GunShotSource;
    public AudioSource MoveNoiseSource;
    public AudioSource PhoneRingSource;

	public AudioClip gunshot;
	public AudioClip spawn;
	public AudioClip explosion;
	public AudioClip playerMove;
	public AudioClip enemyMove;
    public AudioClip phoneRing;
    public AudioClip phonePickUp;

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
	    //if (!GunShotSource.isPlaying) {
	        GunShotSource.clip = gunshot;
	        GunShotSource.volume = 1.0f;
            GunShotSource.Play();
	    //}
	}

	public void PlaySpawnNoise() {
	    if (!GunShotSource.isPlaying) {
	        GunShotSource.clip = spawn;
	        GunShotSource.volume = 1.0f;
	        GunShotSource.Play();
	    }
	}

	public void PlayDeathNoise() {
	    if (!GunShotSource.isPlaying) {
	        GunShotSource.clip = explosion;
	        GunShotSource.volume = 0.5f;
	        GunShotSource.Play();
	    }
	}

	public void PlayPlayerMoveNoise(){
	    if (!MoveNoiseSource.isPlaying) {
	        MoveNoiseSource.clip = playerMove;
	        MoveNoiseSource.volume = 0.7f;
	        MoveNoiseSource.Play();
	    }
	}

	public void PlayEnemyMoveNoise() {
	    if (!MoveNoiseSource.isPlaying) {
	        MoveNoiseSource.clip = enemyMove;
	        MoveNoiseSource.volume = 1.0f;
	        MoveNoiseSource.Play();
	    }
    }

    public void PlayPhoneRing() {
        PhoneRingSource.clip = phoneRing;
        PhoneRingSource.volume = 1.0f;
        PhoneRingSource.Play();
    }

    public void PlayPhonePickUp() {
        PhoneRingSource.clip = phonePickUp;
        PhoneRingSource.volume = 1.0f;
        PhoneRingSource.Play();
    }
}
