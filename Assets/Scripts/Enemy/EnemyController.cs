using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class EnemyController : MonoBehaviour, ITurnBased {
    public EnemyMover Mover;
    public EnemyAI AI;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }

    // Use this for initialization
    void Start() {
        Mover = GetComponentInParent<EnemyMover>();
        AI = GetComponentInParent<EnemyAI>();
        CurrentTurn = FindObjectOfType<Turn>();

        acting = false;
    }

    void Update() {

    }

    public void BeginPhase() {
        //AI DECISION PHASE
        Debug.Log("Updating AI");
        AI.UpdateAI();
        Debug.Log("AI Updated");
    }

    public void EndPhase() {
        
    }
}
