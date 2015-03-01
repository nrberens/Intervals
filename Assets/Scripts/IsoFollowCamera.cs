using UnityEngine;
using System.Collections;

public class IsoFollowCamera : MonoBehaviour {

	public Transform player;
	float x, z;

	void LateUpdate() {
		x = player.transform.position.x;
	    z = player.transform.position.z;

		transform.position = new Vector3(x, 0, z);
	}
}
