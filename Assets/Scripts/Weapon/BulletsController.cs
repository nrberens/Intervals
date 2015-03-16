using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletsController : MonoBehaviour, ITurnBased {

	public Turn CurrentTurn { get; set; }
	public bool acting { get; set; }

	public List<EnemyBullet> Bullets;

	// Use this for initialization
	void Start () {
		Bullets = new List<EnemyBullet>();
		CurrentTurn = FindObjectOfType<Turn>();
	}

	void Update() {
		//only end phase when all bullets have reported finished
		bool allBulletsFinished = true;
		foreach (EnemyBullet bullet in Bullets) {
			if(!bullet.finished) allBulletsFinished = false;
		}

		if(allBulletsFinished) EndPhase ();
	}
	
	public void BeginPhase() {
		//Cycle through each enemies turn
		Debug.Log ("Begin Bullets Phase");
		for(int i = Bullets.Count-1; i >= 0; i--) {
			EnemyBullet enemyBullet = Bullets[i];
			enemyBullet.BeginPhase();
		}

	}
	
	public void EndPhase() {
		Debug.Log ("All bullets finished, ending bullet phase.");
		if (CurrentTurn.CurrentPhase == Turn.Phase.Bullet)
			CurrentTurn.AdvancePhase();
	}
}
