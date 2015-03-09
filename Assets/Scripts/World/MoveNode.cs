using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : MonoBehaviour {

    public bool playerSpawnPoint = false;
    public bool enemySpawnPoint = false;
    public bool itemSpawnPoint = false;

    public bool blocksMovement = false;
    public bool blocksLOS = false;
    public bool LOSToPlayer = false;

    //TODO add flags for blocks that block movement or LOS
	public List<GameObject> objectsOnNode;

    public Transform parentBlock;

    public int x, z;

	// Use this for initialization
	void Awake() {
	    parentBlock = transform.parent;
		objectsOnNode = new List<GameObject>();

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void AddToNode(GameObject obj) {
		if(objectsOnNode.Contains (obj))
			Debug.Log (gameObject + " already contains " + obj);
		else objectsOnNode.Add (obj);
	}

	public void RemoveFromNode(GameObject obj) {
		if(objectsOnNode.Contains (obj))
			objectsOnNode.Remove (obj);
		else Debug.Log (obj + " isn't on " + gameObject);
	}

    public static Direction DirectionToNode(MoveNode startNode, MoveNode targetNode) {
        int absX = Mathf.Abs(startNode.x - targetNode.x);
        int absZ = Mathf.Abs(startNode.z - targetNode.z);

        if (absX > absZ) { //East or West
            if (startNode.x > targetNode.x) //East
                return Direction.East;
            else if (targetNode.x > startNode.x) 
                return Direction.West;
            else Debug.Log("On the same row!");
        } else if (absZ > absX) {
            //North or South
            if (startNode.z > targetNode.z) //South
                return Direction.South;
            else if (targetNode.z > startNode.z) {
                return Direction.North;
            }
            else Debug.Log("On the same column!");
        }
        else Debug.Log("Same distance either way!");
        throw new NotImplementedException();
    }

}
