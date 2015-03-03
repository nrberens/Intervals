using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : MonoBehaviour {

    public bool playerSpawnPoint = false;
    public bool enemySpawnPoint = false;
    public bool itemSpawnPoint = false;

    public bool blocksMovement = false;
    public bool blocksLOS = false;

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

}
