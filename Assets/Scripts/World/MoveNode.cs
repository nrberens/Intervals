using System;
using UnityEngine;

public class MoveNode : MonoBehaviour {

    public Boolean playerSpawnPoint = false;
    public Boolean enemySpawnPoint = false;
    public Boolean itemSpawnPoint = false;

    public Transform parentBlock;

    public int x, z;

	// Use this for initialization
	void Awake() {
	    parentBlock = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //TODO method that detects collision and creates connection between node and colliding object?
}
