using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour, IMover {

    private float moveX, moveZ;
    private float moveSpeed = 4.0f;

    public Transform weapon;    //currently equipped weapon
    public MoveNode currentNode { get; set; }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 crosshairPos = Crosshair.GetCrosshairInWorld();

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

	    if (moveX > 0) //if positive vertical, move forward
	        MoveForward(1);
        else if (moveX < 0) //if negative vertical, move backward
            MoveBackward(1);


        //if positive horizontal move right?
        //if negative horizontal move left?


        //transform.Translate(new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime);

        Transform bulletSpawnPoint = weapon.Find("BulletPoint");
        transform.LookAt(crosshairPos);
        if (Input.GetButton("Fire1")) {
            weapon.SendMessage("Shoot", crosshairPos);
        } 
	}

    void LateUpdate() {
        //update player position
        GetCurrentNode();
    }


    public void MoveForward(int distance) {
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int y = currentNode.y;
        
        // TODO implement interface for player to talk to world
    }

    public void MoveBackward(int distance) {
        throw new System.NotImplementedException();
    }

    public void MoveLeft(int distance) {
        throw new System.NotImplementedException();
    }

    public void MoveRight(int distance) {
        throw new System.NotImplementedException();
    }

    public void GetCurrentNode() {
        throw new System.NotImplementedException();
    }
}
