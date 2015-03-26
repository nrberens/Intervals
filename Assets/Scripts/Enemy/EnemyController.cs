using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class EnemyController : MonoBehaviour, ITurnBased {
    public Transform deathPrefab;
    public GameObject lowReadyMesh;
    public GameObject firingMesh;
    public GameObject meleeMesh;

    public EnemyMover Mover;
    public FSM AI;
    public EnemyShooter Shooter;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }
	public bool turnFinished;
    public bool shootable { get; set; }
    public static int totalEnemies; //reset during game over

    // Use this for initialization
    void Start() {
        Mover = GetComponentInParent<EnemyMover>();
        AI = GetComponentInParent<FSM>();
        Shooter = GetComponentInParent<EnemyShooter>();
        CurrentTurn = FindObjectOfType<Turn>();

        //meshes
        firingMesh.SetActive(false);
        meleeMesh.SetActive(false);
        acting = false;

    }

    void Update() {

    }

	void OnCollisionEnter(Collision collision) {
		if(collision.transform.tag == "PlayerBullet") {
			TakeDamage (collision.transform);
			collision.transform.GetComponent<PlayerBullet>().EndPhase ();
		}
	}

    public void BeginPhase() {
		turnFinished = false;
        //AI DECISION PHASE
        //Debug.Log("Updating AI");
        AI.UpdateAI();
        //Debug.Log("AI Updated");
    }

    public void EndPhase() {
		turnFinished = true;
    }


	public void TakeDamage(Transform bullet) {
		//kill enemy instantly
		//remove from list of enemies
		GameControl.gc.currentScore += 100;
		AudioController.ac.PlayDeathNoise();
        Vector3 deathPrefabPosition = transform.FindChild("DeathPrefab").position;
        Transform death = (Transform) Instantiate(deathPrefab, deathPrefabPosition, bullet.rotation);
		EnemiesController.ec.Enemies.Remove(this);
        Mover.currentNode.RemoveFromNode(gameObject);
		Destroy (gameObject);
	}

	public static void ResetEnemyController() {
		totalEnemies = 0;
	}
}
