using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class Map : MonoBehaviour {
    public Transform EnemyTransform;
    public Transform PhoneTransform;
    public int mapWidth, mapLength;
	public float fallTime;

    public Turn currentTurn { get; set; }

    public MoveNode[,] Nodes;
    public List<MoveNode> SpawnPoints;

    void Awake() {
        GenerateNodeArray();
    }
    // Use this for initialization
    void Start() {

        //PLACE OBJECTS
        //turn off renderers -- world blocks are in final position when spawned
        foreach (Transform t in PlayerController.pc.transform) {
            Renderer[] renderers = t.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers) {
                r.enabled = false;
            }
        }
        foreach (Transform t in transform) {
            Renderer[] renderers = t.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers) {
                r.enabled = false;
            }
        }
        SpawnWorld sw = GetComponent<SpawnWorld>();
        StartCoroutine(sw.ManageSpawnTiming());
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

        MoveNode currentNode = PlayerController.pc.Mover.currentNode;
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
            EnemyController.totalEnemies++;
            Nodes[enemyX, enemyZ].AddToNode(enemy);
            EnemiesController.ec.Enemies.Add(enemy.GetComponent<EnemyController>());
            enemy.transform.position = Nodes[enemyX, enemyZ].transform.position;
            enemy.name = "Enemy " + EnemyController.totalEnemies;
            foreach (Transform t in enemy.transform) {
                Renderer[] renderers = t.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers) {
                    r.enabled = false;
                }
            }
            FallingSpawn fs = enemy.GetComponent<FallingSpawn>();
			StartCoroutine(fs.FallIntoPlace());
        }
        else {
            Debug.Log("Node (" + nodeX + "," +  nodeZ +") full, can't spawn here!");
        }
    }

    public void SpawnPhone(int nodeX, int nodeZ) {
        //Relies on the calling method having checked for a clear space
        MoveNode node = Nodes[nodeX, nodeZ];

        GameObject phone = Instantiate(PhoneTransform.gameObject);
        int phoneX = nodeX;
        int phoneZ = nodeZ;

        phone.GetComponent<Phone>().currentNode = Nodes[phoneX, phoneZ];
        PhoneController.pc.currentPhone = phone.GetComponent<Phone>();
        Nodes[phoneX, phoneZ].AddToNode(phone);
        phone.transform.position = Nodes[phoneX, phoneZ].transform.position;
        foreach (Transform t in phone.transform) {
            Renderer[] renderers = t.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers) {
                r.enabled = false;
            }
        }
        FallingSpawn fs = phone.GetComponent<FallingSpawn>();
        StartCoroutine(fs.FallIntoPlace());

    }

	public void ResetMap() {
		Nodes = new MoveNode[,] {};
		SpawnPoints.Clear ();
	}

}
