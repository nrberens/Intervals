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
        if (!acting && CurrentTurn.CurrentPhase == Turn.Phase.Enemy) {
            ai.UpdateAI();
        }
    }

    public void EndPhase() {
        if(CurrentTurn.CurrentPhase == Turn.Phase.Enemy)
            CurrentTurn.AdvancePhase();
    }
}
