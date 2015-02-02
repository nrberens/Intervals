using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour {

    private float moveX, moveZ;
    private float moveSpeed = 4.0f;

    public Transform weapon;    //currently equipped weapon
    public GameObject currentNode;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 crosshairPos = Crosshair.GetCrosshairInWorld();

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

	    if (moveX > 0) //if positive vertical, move forward
	        MoveForward();
        else if (moveX < 0) //if negative vertical, move backward
            MoveBackward();


        //if positive horizontal move right?
        //if negative horizontal move left?


        //transform.Translate(new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime);

        Transform bulletSpawnPoint = weapon.Find("BulletPoint");
        transform.LookAt(crosshairPos);
        if (Input.GetButton("Fire1")) {
            weapon.SendMessage("Shoot", crosshairPos);
        } 
	}

    private void MoveBackward() {
        throw new System.NotImplementedException();
    }

    private void MoveForward() {
        throw new System.NotImplementedException();
    }

    void LateUpdate() {
        //update player position
        
    }
}
