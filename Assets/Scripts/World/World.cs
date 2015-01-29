using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class World : MonoBehaviour {

    private const int BlockWidth = 20;
    private const int BlockHeight = 3;

    public GameObject[] worldBlocks = new GameObject[10];

    private Queue<GameObject> worldQ = new Queue<GameObject>();
    private Vector3 InitPosition = new Vector3(0, -1, 0);

	// Use this for initialization
	void Start () {
	    GenerateInitialWorld();
	}

    private void GenerateInitialWorld() {
        Vector3 thisPosition = InitPosition;
        for (int i = 0; i < 10; i++) {
            int randomBlock = Random.Range(0, 10);
            GameObject worldBlock = (GameObject)Instantiate(worldBlocks[randomBlock]);
            worldBlock.transform.position = thisPosition;
            worldQ.Enqueue(worldBlock);
            thisPosition.z += BlockHeight;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
