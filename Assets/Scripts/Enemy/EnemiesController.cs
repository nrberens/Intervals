using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesController : MonoBehaviour, ITurnBased {
    public List<EnemyController> Enemies; 

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }

	// Use this for initialization
	void Start () {
        Enemies = new List<EnemyController>();
        CurrentTurn = FindObjectOfType<Turn>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BeginPhase() {
        //Cycle through each enemies turn
        foreach (EnemyController enemy in Enemies) {
            enemy.BeginPhase();
        }

        EndPhase();
    }

    public void EndPhase() {
        if (CurrentTurn.CurrentPhase == Turn.Phase.Enemy)
            CurrentTurn.AdvancePhase();
    }

}
