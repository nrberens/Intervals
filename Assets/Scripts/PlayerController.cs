using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private float moveX, moveZ;
    private float moveSpeed = 4.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime);
	}
}
