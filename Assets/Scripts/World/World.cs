using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class World : MonoBehaviour {
    private PlayerController _pc;
    private EnemiesController _ec;
    public Transform EnemyTransform;

    public const int NODES_PER_BLOCK = 7;
    public const int WORLD_LENGTH = 10;

    private const int BlockWidth = 20;
    private const int BlockHeight = 3;

    public GameObject[] WorldBlockPrefabs = new GameObject[10];
    public GameObject[] WorldBlocks; //to be initialized using queue.length in GenerateInitialWorld()
    public MoveNode[,] Nodes = new MoveNode[NODES_PER_BLOCK, WORLD_LENGTH];

    private readonly Vector3 InitBlockPosition = new Vector3(0, -1, 0);

	// Use this for initialization
	void Start () { 
        //CACHE CONTROLLER SCRIPTS
	    _pc = FindObjectOfType<PlayerController>();
	    _ec = FindObjectOfType<EnemiesController>();

        //GENERATE WORLD
	    GenerateInitialWorld();
	    GenerateNodeArray();

        //PLACE OBJECTS
        //find spawn point
        //FOR NOW HARDCODE IN 0,0
	    MoveNode spawnPoint = Nodes[0, 0];
	    //instantiate player at spawn point
	    _pc.transform.position = spawnPoint.transform.position;
	    _pc.Mover.currentNode = spawnPoint;

        //instantiate enemy at random position
	    for (int i = 1; i <= 3; i++) {
	        GameObject enemy = (GameObject) Instantiate(EnemyTransform.gameObject);
	        int enemyX = Random.Range(0, 7);
	        int enemyZ = Random.Range(0, WORLD_LENGTH);
	        enemy.GetComponent<EnemyMover>().currentNode = Nodes[enemyX, enemyZ];

	        _ec.Enemies.Enqueue(enemy);
	    }
	}

    private void GenerateInitialWorld() {
        Vector3 thisPosition = InitBlockPosition;
        Queue<GameObject> queue = new Queue<GameObject>();

        for (int i = 0; i < WORLD_LENGTH; i++) {
            int randomBlock = Random.Range(0, 10);
            GameObject worldBlock = (GameObject)Instantiate(WorldBlockPrefabs[randomBlock]);
            WorldBlock b = worldBlock.GetComponent<WorldBlock>();
            b.id = i;
            worldBlock.transform.parent = this.transform;
            worldBlock.transform.SetSiblingIndex(i);
            worldBlock.transform.position = thisPosition;
            thisPosition.z += BlockHeight;
        }

        //worldBlocks = queue.ToArray();
    }
	
    private void GenerateNodeArray() {
        foreach (Transform block in transform) {
            int nodeId = 0;
            foreach (Transform node in block.transform) {
                WorldBlock b = block.GetComponent<WorldBlock>(); 
                MoveNode n = node.GetComponent<MoveNode>(); 
                node.SetSiblingIndex(nodeId);
                n.id = nodeId;
                Nodes[n.id, b.id] = n;
                nodeId++;
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
