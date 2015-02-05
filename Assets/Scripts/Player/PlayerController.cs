using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour, IMover {

    // TODO break out Input, Movement, and Firing into different Player classes

    private float moveX, moveZ;
    private float moveSpeed = 4.0f;
    private Boolean moving = false;

    public Transform weapon;    //currently equipped weapon
    
    //IMover properties
    public MoveNode[,] nodes { get; private set; }
    
    public MoveNode currentNode { get; set; }

    //---------------------------------------------//
    
	// Use this for initialization
	void Start ()
	{
        //Cache node grid
        //This is smelly code, I think -- relies on World class -- abstract out?
	    nodes = GameObject.Find("World").GetComponent<World>().nodes;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 crosshairPos = Crosshair.GetCrosshairInWorld();

	    if (!moving)
	    {

	        moveX = Input.GetAxis("Horizontal");
	        moveZ = Input.GetAxis("Vertical");

            //TODO add diagonal movement
	        if (moveX > 0) //if positive vertical, move forward
	        {
	            moving = true; 
	            MoveRight(1);
	        }
	        else if (moveX < 0) //if negative vertical, move backward
	        {
	            moving = true;
	            MoveLeft(1);
	        }
	        else if (moveZ > 0)
	        {
	            moving = true;
	            MoveForward(1);
	        }
	        else if (moveZ < 0)
	        {
	            moving = true;
	            MoveBackward(1);
	        }


	        //if positive horizontal move right?
	        //if negative horizontal move left?


	        //transform.Translate(new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime);

	    }

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
        int node_id = currentNode.x;
        int block_id = currentNode.z;

        if (block_id >= nodes.GetUpperBound(1))
        {
            Debug.Log("You hit the top! node_id= " + node_id + " z = " + block_id);
            return; //bust out early if you're at the top of the map
        }

        MoveNode targetNode = nodes[node_id, block_id + distance];
        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveBackward(int distance) {
        //throw new System.NotImplementedException();
        int node_id= currentNode.x;
        int block_id = currentNode.z;

        if (block_id <= 0) 
        {
            Debug.Log("You hit the bottom! node_id= " + node_id + " z = " + block_id);
            return; 
        }

        MoveNode targetNode = nodes[node_id, block_id - distance];
        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveLeft(int distance) {
        //throw new System.NotImplementedException();
        int node_id = currentNode.x;
        int block_id = currentNode.z;

        if (node_id <= 0) {
            Debug.Log("You hit the left! node_id= " + node_id + " z = " + block_id);
            return;
        }

        MoveNode targetNode = nodes[node_id - distance, block_id];
        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveRight(int distance) {
        //throw new System.NotImplementedException();
        int node_id = currentNode.x;
        int block_id = currentNode.z;

        if (node_id >= nodes.GetUpperBound(0)) {
            Debug.Log("You hit the right! node_id= " + node_id + " z = " + block_id);
            return;
        }

        MoveNode targetNode = nodes[node_id + distance, block_id];
        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void DetectCurrentNode() {
        //throw new System.NotImplementedException();
    }

    public IEnumerator MoveToNode(MoveNode targetNode)
    {
        Vector3 startPos = currentNode.transform.position;
        Vector3 endPos = targetNode.transform.position;
        Vector3 bending = Vector3.up;
        float totalTime = 1.0f;
        float startTime = Time.time;

        while (Time.time < totalTime + startTime)
        {
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, (Time.time - startTime)/totalTime);

            currentPos.x += bending.x * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / totalTime) * Mathf.PI);
            currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / totalTime) * Mathf.PI);
            currentPos.z += bending.z * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / totalTime) * Mathf.PI);

            transform.position = currentPos;

            yield return null;
        }

        moving = false;
    }
}
