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
	
	public void BeginPhase() {
		//Cycle through each enemies turn
		for(int i = Bullets.Count-1; i >= 0; i--) {
			EnemyBullet enemyBullet = Bullets[i];
			enemyBullet.BeginPhase();
		}

//		foreach (EnemyBullet bullet in Bullets) {
//			bullet.BeginPhase();
//		}
		
		EndPhase();
	}
	
	public void EndPhase() {
		if (CurrentTurn.CurrentPhase == Turn.Phase.Bullet)
			CurrentTurn.AdvancePhase();
	}
}
