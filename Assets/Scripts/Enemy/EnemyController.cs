using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class EnemyController : MonoBehaviour, ITurnBased {
    public EnemyMover Mover;
    public FSM AI;
    public EnemyShooter Shooter;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }

    // Use this for initialization
    void Start() {
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
        Debug.Log("Updating AI");
        AI.UpdateAI();
		//HACK shoot every turn
        //Shooter.Shoot();
        Debug.Log("AI Updated");
    }

    public void EndPhase() {
        
    }
}
