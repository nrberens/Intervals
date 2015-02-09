using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngineInternal;

public class PlayerMover : MonoBehaviour, IMover {

    // TODO break out Input, Movement, and Firing into different Player classes

    private float moveX, moveZ;
    public float moveTime;

    private PlayerInventory inventory;
    private PlayerInput input;
    
    //IMover properties
    public bool moving { get; set; }
    public MoveNode[,] nodes { get; private set; }
    public MoveNode currentNode { get; set; }

    //---------------------------------------------//
    
	// Use this for initialization
	void Start ()
	{
        //Cache node grid
        //This is smelly code, I think -- relies on World class -- abstract out?
	    nodes = GameObject.Find("World").GetComponent<World>().nodes;
	    inventory = GetComponentInParent<PlayerInventory>();
	    input = GetComponentInParent<PlayerInput>();
	}
	
	// Update is called once per frame
	void Update () {

	    Transform bulletSpawnPoint = inventory.weapon.Find("BulletPoint");
        transform.LookAt(input.crosshairPos);
	}

    void LateUpdate() {
        //update player position
        //DetectCurrentNode();
    }


    public void MoveForward(int distance) {
        //throw new System.NotImplementedException();
        int node_id = currentNode.x;
        int block_id = currentNode.z;

        if (block_id >= nodes.GetUpperBound(1)) //bust out early if you're at the top of the map
        {
            Debug.Log("You hit the top! node_id= " + node_id + " z = " + block_id);
            moving = false;
            return; 
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
            moving = false;
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
            moving = false;
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
            moving = false;
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
        float startTime = Time.time;

        while (Time.time < moveTime + startTime)
        {
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, (Time.time - startTime)/moveTime);

            currentPos.x += bending.x * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / moveTime) * Mathf.PI);
            currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / moveTime) * Mathf.PI);
            currentPos.z += bending.z * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / moveTime) * Mathf.PI);

            transform.position = currentPos;

            yield return null;
        }

        moving = false;
    }
}
