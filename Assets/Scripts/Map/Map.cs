﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class Map : MonoBehaviour {
    private PlayerController _pc;
    private EnemiesController _ec;
    public Transform EnemyTransform;
    public int mapWidth, mapLength;
	public float fallTime;

    public Turn currentTurn { get; set; }
    public int turnsBetweenSpawns;
    public int turnsUntilNextSpawn;

    public MoveNode[,] Nodes;
    public List<MoveNode> SpawnPoints;

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
        for (int i = 1; i <= 3; i++) {
            int enemyX = Random.Range(0, mapWidth);
            int enemyZ = Random.Range(0, mapLength);

            if (Nodes[enemyX, enemyZ].objectsOnNode.Count == 0) {
                GameObject enemy = (GameObject) Instantiate(EnemyTransform.gameObject);
                enemy.GetComponent<EnemyMover>().currentNode = Nodes[enemyX, enemyZ];

                _ec.Enemies.Add(enemy.GetComponent<EnemyController>());
                enemy.transform.position = Nodes[enemyX, enemyZ].transform.position;
                EnemyController.totalEnemies++;
                enemy.name = "Enemy " + EnemyController.totalEnemies;
                Nodes[enemyX, enemyZ].AddToNode(enemy);
				enemy.AddComponent<FallingSpawn>();
				FallingSpawn fs = enemy.GetComponent<FallingSpawn>();
				fs.fallTime = fallTime;
				fs.finalY = 0f;
				StartCoroutine(fs.FallIntoPlace());
            }
        }

        turnsUntilNextSpawn = turnsBetweenSpawns;
    }

    private void GenerateNodeArray() {
        Nodes = new MoveNode[mapWidth, mapLength];

        GameObject[] unsortedNodeObjects = GameObject.FindGameObjectsWithTag("MoveNode");

        foreach (GameObject o in unsortedNodeObjects) {
            MoveNode node = o.GetComponent<MoveNode>();
            Nodes[node.x, node.z] = node;
            node.parentBlock.name = "WorldBlock (" + node.x + "," + node.z + ")";
        }

        //Grab all spawn points
        foreach (MoveNode n in Nodes) {
            if (n.enemySpawnPoint == true) {
                SpawnPoints.Add(n);
            } 
        }
    }

    // Update is called once per frame
    void Update() {

        if (turnsUntilNextSpawn <= 0) {
            int index = Random.Range(0, SpawnPoints.Count - 1);
            MoveNode spawnPoint = SpawnPoints[index];
            SpawnEnemy(spawnPoint.x, spawnPoint.z);
            turnsUntilNextSpawn = turnsBetweenSpawns;
        }
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

    public MoveNode FindClosestLOSNode(Transform t) {
        MoveNode currentNode = t.GetComponent<EnemyMover>().currentNode;
        MoveNode closestNode = null;
        int closestDistance = Mathf.Max(mapWidth, mapLength);
        //Check North, stop at first obstacle
        for (int i = currentNode.z+1; i < mapLength; i++) {
            bool breakTopLoop = false;
            MoveNode node = Nodes[currentNode.x, i];

            foreach (GameObject o in node.objectsOnNode) {
                if (o.tag == "Obstacle" || o.tag == "Enemy") {
                    breakTopLoop = true;
                    break;
                }
            }

            if (breakTopLoop) break;

            if (node.LOSToPlayer) {
                closestNode = node;
                closestDistance = node.z - currentNode.z;
                break;
            }
        }

        //Check South
        for (int i = currentNode.z-1; i >= 0; i--) {
            bool breakTopLoop = false;
            MoveNode node = Nodes[currentNode.x, i];

            foreach (GameObject o in node.objectsOnNode) {
                if (o.tag == "Obstacle" || o.tag == "Enemy") {
                    breakTopLoop = true;
                    break;
                }
            }

            if (breakTopLoop) break;

            if (node.LOSToPlayer) {
                int distance = currentNode.z - node.z;
                if (distance < closestDistance) {
                    closestNode = node;
                }
                break;
            }
        }

        //Check East
        for (int i = currentNode.x + 1; i < mapWidth; i++) {
            bool breakTopLoop = false;
            MoveNode node = Nodes[i, currentNode.z];

            foreach (GameObject o in node.objectsOnNode) {
                if (o.tag == "Obstacle" || o.tag == "Enemy") {
                    breakTopLoop = true;
                    break;
                }
            }

            if (breakTopLoop) break;

            if (node.LOSToPlayer) {
                int distance = node.x - currentNode.x;
                if (distance < closestDistance) {
                    closestNode = node;
                }
                break;
            }
        }

        //Check West 
        for (int i = currentNode.x - 1; i >= 0; i--) {
            bool breakTopLoop = false;
            MoveNode node = Nodes[i, currentNode.z];

            foreach (GameObject o in node.objectsOnNode) {
                if (o.tag == "Obstacle" || o.tag == "Enemy") {
                    breakTopLoop = true;
                    break;
                }
            }

            if (breakTopLoop) break;

            if (node.LOSToPlayer) {
                int distance = currentNode.x - node.x;
                if (distance < closestDistance) {
                    closestNode = node;
                }
                break;
            }
        }

        return closestNode;
    }

    public void SpawnEnemy(int nodeX, int nodeZ) {
        MoveNode node = Nodes[nodeX, nodeZ];

        if (node.objectsOnNode.Count == 0) {
            GameObject enemy = (GameObject) Instantiate(EnemyTransform.gameObject);
            int enemyX = nodeX;
            int enemyZ = nodeZ;
            enemy.GetComponent<EnemyMover>().currentNode = Nodes[enemyX, enemyZ];

            _ec.Enemies.Add(enemy.GetComponent<EnemyController>());
            enemy.transform.position = Nodes[enemyX, enemyZ].transform.position;
            EnemyController.totalEnemies++;
            enemy.name = "Enemy " + EnemyController.totalEnemies;
            Nodes[enemyX, enemyZ].AddToNode(enemy);
        }
        else {
            Debug.Log("Node (" + nodeX + "," +  nodeZ +") full, can't spawn here!");
        }
    }

}
