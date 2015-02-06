using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class World : MonoBehaviour {
    //public GameObject player;
    //public PlayerMovement playerController;
    public PlayerMovement player;

    public const int NODES_PER_BLOCK = 7;
    public const int WORLD_LENGTH = 10;

    private const int BlockWidth = 20;
    private const int BlockHeight = 3;

    public GameObject[] worldBlockPrefabs = new GameObject[10];
    public GameObject[] worldBlocks; //to be initialized using queue.length in GenerateInitialWorld()
    public MoveNode[,] nodes = new MoveNode[NODES_PER_BLOCK, WORLD_LENGTH];

    private Vector3 InitBlockPosition = new Vector3(0, -1, 0);

	// Use this for initialization
	void Start () { 
        //CACHE CONTROLLER SCRIPTS

        //GENERATE WORLD
	    GenerateInitialWorld();
	    GenerateNodeArray();

        //PLACE OBJECTS
        //find spawn point
        //FOR NOW HARDCODE IN 0,0
	    MoveNode spawnPoint = nodes[0, 0];
	    //instantiate player at spawn point
	    player.transform.position = spawnPoint.transform.position;
	    player.currentNode = spawnPoint;
	}

    private void GenerateInitialWorld() {
        Vector3 thisPosition = InitBlockPosition;
        Queue<GameObject> queue = new Queue<GameObject>();

        for (int i = 0; i < WORLD_LENGTH; i++) {
            int randomBlock = Random.Range(0, 10);
            GameObject worldBlock = (GameObject)Instantiate(worldBlockPrefabs[randomBlock]);
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
            int node_id = 0;
            foreach (Transform node in block.transform) {
                WorldBlock b = block.GetComponent<WorldBlock>(); 
                MoveNode n = node.GetComponent<MoveNode>(); 
                node.SetSiblingIndex(node_id);
                n.id = node_id;
                nodes[n.id, b.id] = n;
                node_id++;
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
