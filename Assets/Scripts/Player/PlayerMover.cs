using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngineInternal;

public class PlayerMover : MonoBehaviour, IMover {

    private PlayerController pc;
    private Map map;
    public float MoveTime;


    //IMover properties
    public MoveNode[,] nodes { get; private set; }
    public MoveNode currentNode { get; set; }

    //---------------------------------------------//

    // Use this for initialization
    void Start() {
        //Cache node grid
        //This is smelly code, I think -- relies on World class -- abstract out?
        map = GameObject.Find("World").GetComponent<Map>();
        nodes = map.Nodes;

        pc = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        //transform.LookAt(input.crosshairPos);
    }

    void LateUpdate() {
        //update player position
        //DetectCurrentNode();
    }

    public void Move(Direction direction, int distance) {
        try {
            MoveNode targetNode = GetTargetNode(direction, distance);

            if (targetNode == null) {
                pc.acting = false;
                pc.EndPhase();
                return; //No node found, at edge of map
            }

            bool hasBlocking = targetNode.blocksMovement;
			bool hasBullet = false;
			bool hasEnemy = false;

			foreach(GameObject obj in targetNode.objectsOnNode) {
				if(obj.tag == "Bullet") {
					hasBullet = true;
				} else if (obj.tag == "Enemy") {
					hasEnemy = true;
				}
			}

            if (!hasBlocking && !hasEnemy) {

                switch (direction) {
                    case Direction.North:
                        pc.Mover.MoveUp(distance);
                        break;
                    case Direction.South:
                        pc.Mover.MoveDown(distance);
                        break;
                    case Direction.West:
                        pc.Mover.MoveLeft(distance);
                        break;
                    case Direction.East:
                        pc.Mover.MoveRight(distance);
                        break;
                }

				if(hasBullet) pc.GameOver();

                map.FlagNewLOSNodes();
            }
            else {
                //TODO allow player to input again if he didn't actually move
                pc.acting = false;
                pc.EndPhase();
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
            pc.acting = false;
            pc.EndPhase();
            return;
        }

        transform.forward = Vector3.forward;
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
            pc.acting = false;
            pc.EndPhase();
            return;
        }

        transform.forward = Vector3.back;
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
            pc.acting = false;
            pc.EndPhase();
            return;
        }

        transform.forward = Vector3.left;
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
            pc.acting = false;
            pc.EndPhase();
            return;
        }

        transform.forward = Vector3.right;
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
        //Vector3 bending = Vector3.up;
        float startTime = Time.time;

        while (Time.time < MoveTime + startTime) {
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / MoveTime);

            //currentPos.x += bending.x * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);
            //currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);
            //currentPos.z += bending.z * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);

            transform.position = currentPos;

            yield return null;
        }

        pc.acting = false;
        pc.EndPhase();
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
		} catch (IndexOutOfRangeException e) {}
        return null;
    }
}
