using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngineInternal;

public class PlayerMover : MonoBehaviour, IMover {

    private Map map;
    public float MoveTime;
    public float wiggleTime;
    public float wobbleAmount;
    public float wobbleRotateAmount;
    public float rotateTime;

    //IMover properties
    public MoveNode[,] nodes { get; private set; }
    public List<MoveNode> movableNodes { get; set; }
    public MoveNode currentNode { get; set; }

    //---------------------------------------------//

    // Use this for initialization
    void Start() {
        //Cache node grid
        //This is smelly code, I think -- relies on World class -- abstract out?
        map = GameObject.Find("World").GetComponent<Map>();
        nodes = map.Nodes;

    }

    // Update is called once per frame
    void Update() {
        //transform.LookAt(input.crosshairPos);
    }

    void LateUpdate() {
        //update player position
        //DetectCurrentNode();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Bullet") {
            //TODO move GameOver() to GameController?
            PlayerController.pc.GameOver(collision.transform);
        }
    }

    public void Move(Direction direction, int distance) {
        try {
            MoveNode targetNode = GetTargetNode(direction, distance);

            if (targetNode == null) {
                PlayerController.pc.acting = false;
                PlayerController.pc.EndPhase();
                return; //No node found, at edge of map
            }

            bool hasBlocking = targetNode.blocksMovement;
            bool hasBullet = false;
            Transform bullet = null;
            bool hasEnemy = false;

            foreach (GameObject obj in targetNode.objectsOnNode) {
                //if(obj.tag == "Bullet") {
                //	hasBullet = true;
                //    bullet = obj.transform;
                //TODO Add OnCollisionEnter for EnemyBullets
                //} else if (obj.tag == "Enemy") {
                if (obj.tag == "Enemy") {
                    hasEnemy = true;
                }
            }

            if (!hasBlocking && !hasEnemy) {

                switch (direction) {
                    case Direction.North:
                        PlayerController.pc.Mover.MoveUp(distance);
                        break;
                    case Direction.South:
                        PlayerController.pc.Mover.MoveDown(distance);
                        break;
                    case Direction.West:
                        PlayerController.pc.Mover.MoveLeft(distance);
                        break;
                    case Direction.East:
                        PlayerController.pc.Mover.MoveRight(distance);
                        break;
                }

                map.FlagNewLOSNodes();
            } else {
                //TODO allow player to input again if he didn't actually move
                PlayerController.pc.acting = false;
                PlayerController.pc.EndPhase();
            }
        } catch (NullReferenceException e) {
            Debug.Log(gameObject + " threw a NullReferenceException.");
        }
    }


    public void MoveUp(int distance) {
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int z = currentNode.z;

        if (z >= nodes.GetUpperBound(1)) //bust out early if you're at the top of the map
        {
            Debug.Log("You hit the top! node_id= " + x + " z = " + z);
            PlayerController.pc.acting = false;
            return;
        }

        MoveNode targetNode = nodes[x, z + distance];
        //remove from currentNode
        //add to targetNode
        currentNode.RemoveFromNode(gameObject);
        targetNode.AddToNode(gameObject);

        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveDown(int distance) {
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int z = currentNode.z;

        if (z <= 0) {
            Debug.Log("You hit the bottom! node_id= " + x + " z = " + z);
            PlayerController.pc.acting = false;
            return;
        }

        MoveNode targetNode = nodes[x, z - distance];
        //remove from currentNode
        //add to targetNode
        currentNode.RemoveFromNode(gameObject);
        targetNode.AddToNode(gameObject);

        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveLeft(int distance) {
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int z = currentNode.z;

        if (x <= 0) {
            Debug.Log("You hit the left! node_id= " + x + " z = " + z);
            PlayerController.pc.acting = false;
            return;
        }

        MoveNode targetNode = nodes[x - distance, z];
        //remove from currentNode
        //add to targetNode
        currentNode.RemoveFromNode(gameObject);
        targetNode.AddToNode(gameObject);

        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveRight(int distance) {
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int z = currentNode.z;

        if (x >= nodes.GetUpperBound(0)) {
            Debug.Log("You hit the right! node_id= " + x + " z = " + z);
            PlayerController.pc.acting = false;
            return;
        }

        MoveNode targetNode = nodes[x + distance, z];
        //remove from currentNode
        //add to targetNode
        currentNode.RemoveFromNode(gameObject);
        targetNode.AddToNode(gameObject);

        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void DetectCurrentNode() {
        //throw new System.NotImplementedException();
    }

    public IEnumerator MoveToNode(MoveNode targetNode) {
        Vector3 startPos = currentNode.transform.position;
        Vector3 endPos = targetNode.transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(targetNode.transform.position - transform.position);
        //Vector3 bending = Vector3.up;
        float startTime = Time.time;

        //Rotate Object before moving
        while (Time.time < rotateTime + startTime) {
            transform.rotation = Quaternion.Slerp(startRot, endRot, (Time.time - startTime) / MoveTime);
            yield return null;
        }

        startTime = Time.time;

        while (Time.time < MoveTime + startTime) {
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / MoveTime);

            //currentPos.x += bending.x * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);
            //currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);
            //currentPos.z += bending.z * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);

            transform.position = currentPos;

            yield return null;
        }

        StartCoroutine(ObjectWiggle(endPos));

        transform.position = endPos;

        PlayerController.pc.acting = false;
    }

    public IEnumerator ObjectWiggle(Vector3 centerPos) {
        float startTime = Time.time;

        //wiggle
        while (Time.time < wiggleTime + startTime) {
            float xWiggleOffset = (Mathf.Sin(Time.time * wobbleAmount) * 0.05f) / 2;// + (Random.Range(0,3)*.02f);
            float zWiggleOffset = (Mathf.Cos(Time.time * wobbleAmount) * 0.05f) / 2;// + (Random.Range(0, 3) * .02f); 
            Vector3 currentPos = new Vector3(centerPos.x + xWiggleOffset, centerPos.y, centerPos.z + zWiggleOffset);
            transform.position = currentPos;
            transform.Rotate(xWiggleOffset * wobbleRotateAmount, 0, zWiggleOffset * wobbleRotateAmount);

            yield return null;
        }

    }

    public MoveNode GetTargetNode(Direction dir, int distance) {
        try {
            switch (dir) {
                case Direction.North:
                    return nodes[currentNode.x, currentNode.z + distance];
                case Direction.South:
                    return nodes[currentNode.x, currentNode.z - distance];
                case Direction.West:
                    return nodes[currentNode.x - distance, currentNode.z];
                case Direction.East:
                    return nodes[currentNode.x + distance, currentNode.z];
            }
        } catch (IndexOutOfRangeException e) { }
        return null;
    }

	public Direction? GetTargetDirection(MoveNode node) {
		int x = currentNode.x;
		int z = currentNode.z;

		if(node.x == x && node.z == z+1) return Direction.North;
		else if (node.x == x && node.z == z-1) return Direction.South;
		else if (node.x == x+1 && node.z == z) return Direction.East;
		else if (node.x == x-1 && node.z == z) return Direction.West;
		else return null;
	}

    public void TagMovableNodes() {
        movableNodes = new List<MoveNode>();
        MoveNode node = GetTargetNode(Direction.North, 1);
        if (node != null) {
            if (!node.blocksMovement) {
                node.movable = true;
				MovableNode nodeController = node.transform.parent.GetComponentInChildren<MovableNode>();
				nodeController.currentState = MovableNode.NodeState.MovableUnselected;
                movableNodes.Add(node);
            }
        }

        node = GetTargetNode(Direction.South, 1);
        if (node != null) {
            if (!node.blocksMovement) {
                node.movable = true;
				MovableNode nodeController = node.transform.parent.GetComponentInChildren<MovableNode>();
				nodeController.currentState = MovableNode.NodeState.MovableUnselected;
                movableNodes.Add(node);
            }
        }

        node = GetTargetNode(Direction.East, 1);
        if (node != null) {
            if (!node.blocksMovement) {
                node.movable = true;
				MovableNode nodeController = node.transform.parent.GetComponentInChildren<MovableNode>();
				nodeController.currentState = MovableNode.NodeState.MovableUnselected;
                movableNodes.Add(node);
            }
        }

        node = GetTargetNode(Direction.West, 1);
        if (node != null) {
            if (!node.blocksMovement) {
                node.movable = true;
				MovableNode nodeController = node.transform.parent.GetComponentInChildren<MovableNode>();
				nodeController.currentState = MovableNode.NodeState.MovableUnselected;
                movableNodes.Add(node);
            }
        }
    }

    public void UnTagMovableNodes() {
        for (int i = 0; i < movableNodes.Count; i++) {
            MoveNode node = movableNodes[i];
			MovableNode nodeController = node.transform.parent.GetComponentInChildren<MovableNode>();
			nodeController.currentState = MovableNode.NodeState.Off;
            node.movable = false;
        }
        movableNodes.Clear();
    }
}
