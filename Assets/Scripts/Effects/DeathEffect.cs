using UnityEngine;
using System.Collections;

public class DeathEffect : MonoBehaviour {

	public ParticleSystem ps;
	public float effectTime;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem>();
		StartCoroutine (PlayEffect());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator PlayEffect() {
		float startTime;

		startTime = Time.time;

		ps.Play ();
		yield return new WaitForSeconds(1.5f);
		ps.Stop ();
		Destroy (gameObject);
	}
}
