using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class Map : MonoBehaviour {
    private PlayerController _pc;
    private EnemiesController _ec;
    public Transform EnemyTransform;
    public int mapWidth, mapLength;

    public MoveNode[,] Nodes;

    void Awake() {
        GenerateNodeArray();
    }
    // Use this for initialization
    void Start() {
        //CACHE CONTROLLER SCRIPTS -- causes circular dependency
        _pc = FindObjectOfType<PlayerController>();
        _ec = FindObjectOfType<EnemiesController>();


        //PLACE OBJECTS
        //find spawn point
        //FOR NOW HARDCODE IN 0,0
        MoveNode spawnPoint = Nodes[0, 0];
        //instantiate player at spawn point
        //TODO make random spawn point
        _pc.transform.position = spawnPoint.transform.position;
        _pc.Mover.currentNode = spawnPoint;
        _pc.Mover.currentNode.AddToNode(_pc.gameObject);

        //instantiate enemy at random position
        //TODO check for valid spawn point for enemies
        for (int i = 1; i <= 6; i++) {
            GameObject enemy = (GameObject)Instantiate(EnemyTransform.gameObject);
            int enemyX = Random.Range(0, mapWidth);
            int enemyZ = Random.Range(0, mapLength);
            enemy.GetComponent<EnemyMover>().currentNode = Nodes[enemyX, enemyZ];

            _ec.Enemies.Add(enemy.GetComponent<EnemyController>());
            enemy.transform.position = Nodes[enemyX, enemyZ].transform.position;
            EnemyController.totalEnemies++;
            enemy.name = "Enemy " + EnemyController.totalEnemies;
            Nodes[enemyX, enemyZ].AddToNode(enemy);
        }
    }

    private void GenerateNodeArray() {
        Nodes = new MoveNode[mapWidth, mapLength];

        GameObject[] unsortedNodeObjects = GameObject.FindGameObjectsWithTag("MoveNode");

        foreach (GameObject o in unsortedNodeObjects) {
            MoveNode node = o.GetComponent<MoveNode>();
            Nodes[node.x, node.z] = node;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    //HELPER METHODS
    public MoveNode GetDistantNode(MoveNode currentNode, int newX, int newZ) {
        int thisX = currentNode.x;
        int thisZ = currentNode.z;
        return Nodes[thisX + newX, thisZ + newZ];
    }

    public void FlagNewLOSNodes() {
        //unflag all nodes
        foreach (MoveNode node in Nodes) {
            node.LOSToPlayer = false;
        }

        MoveNode currentNode = _pc.Mover.currentNode;
        //flag player's node
        currentNode.LOSToPlayer = true;

        //find LOS nodes in each direction
        //Check North, stop at first obstacle
       for (int i = currentNode.z; i < mapLength; i++) {
            MoveNode node = Nodes[currentNode.x, i];
            if (node.blocksLOS) break;
            node.LOSToPlayer = true;
        }

        //Check South
        for (int i = currentNode.z; i >= 0; i--) {
            MoveNode node = Nodes[currentNode.x, i];
            if (node.blocksLOS) break;
            node.LOSToPlayer = true; 
        }

        //Check East
        for (int i = currentNode.x; i < mapWidth; i++) {
            MoveNode node = Nodes[i, currentNode.z];
            if (node.blocksLOS) break;
            node.LOSToPlayer = true; 
        }

        //Check West 
        for (int i = currentNode.x; i >= 0; i--) {
            MoveNode node = Nodes[i, currentNode.z];
            if (node.blocksLOS) break;
            node.LOSToPlayer = true;
        }
    }
}
