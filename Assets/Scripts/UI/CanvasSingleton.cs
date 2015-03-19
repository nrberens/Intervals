using UnityEngine;
using System.Collections;

public class CanvasSingleton : MonoBehaviour {

	public static CanvasSingleton canvas;

	// Use this for initialization
	void Start () {
		if(canvas == null) {
			DontDestroyOnLoad(gameObject);
			canvas = this;
		} else if (canvas != this) {
			Destroy (gameObject);
		}
	
	}
}
