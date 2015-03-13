using UnityEngine;
using System.Collections;

public class SpawnWorld : MonoBehaviour {

    private Map map;

	// Use this for initialization
	void Start () {
	    map = GetComponent<Map>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator ManageSpawnTiming() {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(SpawnInWorldBlocks());
        StartCoroutine(SpawnInPlayer());
        StartCoroutine(SpawnInEnemies());
        yield return new WaitForSeconds(1.0f);
    }

    public IEnumerator SpawnInWorldBlocks() {
        //spawn in world
        foreach (Transform t in transform) {
            FallingSpawn fs = t.GetComponent<FallingSpawn>();
            if (fs != null) StartCoroutine(fs.FallIntoPlace());
        }
        yield return null;
    }

    public IEnumerator SpawnInPlayer() {
        //find spawn point
        //FOR NOW HARDCODE IN 0,0
        MoveNode spawnPoint = map.Nodes[0, 0];
        //instantiate player at spawn point
        //TODO make random spawn point
        map.pc.transform.position = spawnPoint.transform.position;
        map.pc.Mover.currentNode = spawnPoint;
        map.pc.Mover.currentNode.AddToNode(map.pc.gameObject);
        FallingSpawn fs = map.pc.GetComponent<FallingSpawn>();
        StartCoroutine(fs.FallIntoPlace());
        yield return null;
    }

    public IEnumerator SpawnInEnemies() {
        //instantiate enemy at random position
        //TODO check for valid spawn point for enemies
        for (int i = 1; i <= 3; i++) {
            int enemyX = Random.Range(0, map.mapWidth);
            int enemyZ = Random.Range(0, map.mapLength);

            if (map.Nodes[enemyX, enemyZ].objectsOnNode.Count == 0) {
                GameObject enemy = (GameObject) Instantiate(map.EnemyTransform.gameObject);
                enemy.GetComponent<EnemyMover>().currentNode = map.Nodes[enemyX, enemyZ];

                map.ec.Enemies.Add(enemy.GetComponent<EnemyController>());
                enemy.transform.position = map.Nodes[enemyX, enemyZ].transform.position;
                EnemyController.totalEnemies++;
                enemy.name = "Enemy " + EnemyController.totalEnemies;
                map.Nodes[enemyX, enemyZ].AddToNode(enemy);
				//enemy.AddComponent<FallingSpawn>();
                enemy.SetActive(true);
				FallingSpawn fs = enemy.GetComponent<FallingSpawn>();
				//fs.fallTime = fallTime;
				//fs.finalY = 0f;
				StartCoroutine(fs.FallIntoPlace());
                yield return null;
            }
        }
    }
}
