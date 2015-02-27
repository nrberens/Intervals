using UnityEngine;
using System.Collections;

public class PlayerShooter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//On click, get target of mouse click
	public Transform GetTargetOfClick() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast (ray, out hit)) {
			return hit.transform;
		} else return null;
	}

	public bool CheckValidTarget (Transform target) {
		if(target.tag == "Enemy") {
			//Check for correct positioning

			//Check for line of sight
		}

	}
}
