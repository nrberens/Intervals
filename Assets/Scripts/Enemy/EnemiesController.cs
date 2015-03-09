using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

public class EnemiesController : MonoBehaviour, ITurnBased {
    public List<EnemyController> Enemies; 

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }
    public int totalCurrentEnemies;

	// Use this for initialization
	void Start () {
        Enemies = new List<EnemyController>();
        CurrentTurn = FindObjectOfType<Turn>();
	}
	
	// Update is called once per frame
	void Update () {
	    totalCurrentEnemies = Enemies.Count;
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
