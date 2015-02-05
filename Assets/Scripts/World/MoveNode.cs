using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;

public class MoveNode : MonoBehaviour {

    public Boolean playerSpawnPoint = false;
    public Boolean enemySpawnPoint = false;
    public Boolean itemSpawnPoint = false;

    public GameObject parentBlock;
    public WorldBlock parentBlockController;

    public int id;
    public int x, z;

	// Use this for initialization
	void Start () {
	    parentBlock = transform.parent.gameObject;
	    parentBlockController = parentBlock.GetComponent<WorldBlock>();

	    x = id;
	    z = parentBlockController.id;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //TODO method that detects collision and creates connection between node and colliding object?
}
