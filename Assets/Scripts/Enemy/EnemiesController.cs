using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

public class EnemiesController : MonoBehaviour, ITurnBased {
	public static EnemiesController ec;
    public List<EnemyController> Enemies; 

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }
    public int totalCurrentEnemies;

	void Awake() {
		if (ec == null) {
			DontDestroyOnLoad(gameObject);
			ec = this;
		} else if (ec != this) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
        Enemies = new List<EnemyController>();
        CurrentTurn = FindObjectOfType<Turn>();
	}
	
	// Update is called once per frame
	void Update () {
	    totalCurrentEnemies = Enemies.Count;

		bool allTurnsFinished = true;
		foreach(EnemyController enemy in Enemies) {
			if(!enemy.turnFinished) allTurnsFinished = false;
		}

		// only trigger this when all enemies have gone
		if(allTurnsFinished) EndPhase ();
	}

    public void BeginPhase() {
        //Cycle through each enemies turn
		for(int i = Enemies.Count-1; i >= 0; i--) {
			EnemyController enemy = Enemies[i];
			enemy.BeginPhase();
		}
    }

    public void EndPhase() {
        if (CurrentTurn.CurrentPhase == Turn.Phase.Enemy)
            CurrentTurn.AdvancePhase();
    }

}
