using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class EnemyController : MonoBehaviour, ITurnBased {
	private EnemiesController _ec;
    public EnemyMover Mover;
    public FSM AI;
    public EnemyShooter Shooter;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }
    public static int totalEnemies;

    // Use this for initialization
    void Start() {
		_ec = FindObjectOfType<EnemiesController>();
        Mover = GetComponentInParent<EnemyMover>();
        AI = GetComponentInParent<FSM>();
        Shooter = GetComponentInParent<EnemyShooter>();
        CurrentTurn = FindObjectOfType<Turn>();

        acting = false;
    }

    void Update() {

    }

    public void BeginPhase() {
        //AI DECISION PHASE
        //Debug.Log("Updating AI");
        AI.UpdateAI();
        //Debug.Log("AI Updated");
    }

    public void EndPhase() {
        
    }

	public void TakeDamage() {
		//kill enemy instantly
		//remove from list of enemies
		_ec.Enemies.Remove(this);
        Mover.currentNode.RemoveFromNode(gameObject);
		Destroy (gameObject);

	}
}
