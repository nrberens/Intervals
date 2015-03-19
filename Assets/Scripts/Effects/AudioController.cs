using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	public static AudioController ac;

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
}
