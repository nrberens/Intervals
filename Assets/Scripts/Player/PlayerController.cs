using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour, IMover {

    private float moveX, moveZ;
    private float moveSpeed = 4.0f;

    public Transform weapon;    //currently equipped weapon
    
    //IMover properties
    public MoveNode[,] nodes { get; private set; }
    
    public MoveNode currentNode { get; set; }

    //---------------------------------------------//
    
	// Use this for initialization
	void Start ()
	{
        //Cache node grid
        //This is smelly code, I think
	    nodes = GameObject.Find("World").GetComponent<World>().nodes;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 crosshairPos = Crosshair.GetCrosshairInWorld();

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

	    if (moveX > 0) //if positive vertical, move forward
	        MoveLeft(1);
        else if (moveX < 0) //if negative vertical, move backward
            MoveRight(1);
        else if (moveZ > 0)
            MoveForward(1);
        else if (moveZ < 0)
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
        //DetectCurrentNode();
    }


    public void MoveForward(int distance) {
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int y = currentNode.y;

        MoveNode targetNode = nodes[x, y + distance];
        currentNode = targetNode;
        transform.position = currentNode.transform.position;
    }

    public void MoveBackward(int distance) {
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int y = currentNode.y;

        MoveNode targetNode = nodes[x, y - distance];
        currentNode = targetNode;
        transform.position = currentNode.transform.position;
    }

    public void MoveLeft(int distance) {
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int y = currentNode.y;

        MoveNode targetNode = nodes[x + distance, y];
        currentNode = targetNode;
        transform.position = currentNode.transform.position;
    }

    public void MoveRight(int distance) {
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int y = currentNode.y;

        MoveNode targetNode = nodes[x - distance, y];
        currentNode = targetNode;
        transform.position = currentNode.transform.position;
    }

    public void DetectCurrentNode() {
        //throw new System.NotImplementedException();
    }
}
