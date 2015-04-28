using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnWorld : MonoBehaviour {

    private Map map;
	public int numberOfEnemies;

	// Use this for initialization
	void Start () {
	    map = GetComponent<Map>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator ManageSpawnTiming() {
        yield return new WaitForSeconds(1.0f);
		SpawnInWorldBlocks();
		SpawnInPlayer();
		SpawnInEnemies();
        yield return new WaitForSeconds(1.0f);
    }

    public void SpawnInWorldBlocks() {
        //spawn in world
        foreach (Transform t in transform) {
            FallingSpawn fs = t.GetComponent<FallingSpawn>();
            //if (fs != null) StartCoroutine(fs.FallIntoPlace());
			if(fs != null) fs.FallIntoPlaceTweened();
        }
    }

    public void SpawnInPlayer() {
        //find spawn point
        MoveNode spawnPoint = map.Nodes[6, 2];
        //instantiate player at spawn point
        //TODO make random spawn point
        PlayerController.pc.transform.position = spawnPoint.transform.position;
		PlayerController.pc.Mover.currentNode = spawnPoint;
        PlayerController.pc.Mover.currentNode.AddToNode(PlayerController.pc.gameObject);
        FallingSpawn fs = PlayerController.pc.GetComponent<FallingSpawn>();
        //StartCoroutine(fs.FallIntoPlace());
		fs.FallIntoPlaceTweened();
    }

    public void SpawnInEnemies() {
        //instantiate enemy at random position
        //TODO check for valid spawn point for enemies
		int spawnedEnemies = 0;
		do {
            int enemyX = Random.Range(0, map.mapWidth);
            int enemyZ = Random.Range(0, map.mapLength);

            if (map.Nodes[enemyX, enemyZ].objectsOnNode.Count == 0 && !map.Nodes[enemyX, enemyZ].blocksMovement) {
				spawnedEnemies++;
                int typeCoin = UnityEngine.Random.Range(0, map.EnemyTransforms.Length);
                Transform enemyTransform = map.EnemyTransforms[typeCoin];
                GameObject enemy = (GameObject) Instantiate(enemyTransform.gameObject);
                enemy.GetComponent<EnemyMover>().currentNode = map.Nodes[enemyX, enemyZ];

                EnemiesController.ec.Enemies.Add(enemy.GetComponent<EnemyController>());
                enemy.transform.position = map.Nodes[enemyX, enemyZ].transform.position;
                EnemyController.totalEnemies++;
                enemy.name = "Enemy " + EnemyController.totalEnemies;
                map.Nodes[enemyX, enemyZ].AddToNode(enemy);
				//enemy.AddComponent<FallingSpawn>();
                enemy.SetActive(true);
				FallingSpawn fs = enemy.GetComponent<FallingSpawn>();
				//StartCoroutine(fs.FallIntoPlace());
				fs.FallIntoPlaceTweened();
				AudioController.ac.PlaySpawnNoise();
				enemy.GetComponent<EnemyController>().turnFinished = true;
            }
		} while (spawnedEnemies < numberOfEnemies);
    }
}
