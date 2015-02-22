using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class EnemyController : MonoBehaviour, ITurnBased {

    public EnemyMover mover;
    public EnemyAI ai;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }

    private bool doneWithTurn;

    // Use this for initialization
    void Start() {
        mover = GetComponentInParent<EnemyMover>();
        ai = GetComponentInParent<EnemyAI>();
        CurrentTurn = FindObjectOfType<Turn>();

        acting = false;
    }

    void Update() {

    }

    public void BeginPhase() {
        //AI DECISION PHASE
        Debug.Log("Updating AI");
        ai.UpdateAI();
        Debug.Log("AI Updated");
    }

    public void EndPhase() {
        if (CurrentTurn.CurrentPhase == Turn.Phase.Enemy)
            CurrentTurn.AdvancePhase();
    }
}
