using UnityEngine;
using System.Collections;

public class FallingSpawn : MonoBehaviour {
	public Vector3 startPos;
	public float finalY;
	public Vector3 finalPos;
	public float bounceAmt;
	public float fallTime;
	public float bounceTime;

	// Use this for initialization
	void Start () {
		//when added to a script, start falling into place

	}
	
	// Update is called once per frame
	void Update () {

	
	}

	public IEnumerator FallIntoPlace() {
		float startTime = Time.time;
		startPos = new Vector3(transform.position.x, 6.0f, transform.position.z);
		finalPos = new Vector3(transform.position.x, finalY, transform.position.z);
		transform.position = startPos;

		//TODO add random offset to speed of lerp
		while(Time.time < startTime + fallTime) {
			Vector3 currentPosition = Vector3.Lerp (startPos, finalPos, (Time.time-startTime)/fallTime);
			transform.position = currentPosition;

			yield return null;
		}

		//TODO add bounce
	}
}
