using UnityEngine;
using System.Collections;

public class Pulsate : MonoBehaviour {

	public bool pulsating;
	public float xOffset, yOffset, xNegOffset, yNegOffset;
	public float startTime, duration;

	// Use this for initialization
	void Start () {

		pulsating = true;

		startTime = Time.time;

		xOffset += Screen.width/2; 
		yOffset += Screen.height/2;
		xNegOffset -= Screen.width/2;
		yNegOffset -= Screen.height/2;
	}
	
	// Update is called once per frame
	void Update () {

		if(pulsating) {
			float time = (Time.time - startTime) / duration;

			float x = transform.position.x;
			float y = transform.position.y;
			float z = transform.position.z;

			float newX = x;
			float newY = y;

			transform.position = new Vector3(newX, newY, z);
		}


	}
}
