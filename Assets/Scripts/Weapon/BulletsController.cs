using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletsController : MonoBehaviour, ITurnBased {

	public Turn CurrentTurn { get; set; }
	public bool acting { get; set; }

	public List<Bullet> Bullets;

	// Use this for initialization
	void Start () {
		Bullets = new List<Bullet>();
		CurrentTurn = FindObjectOfType<Turn>();
	}
	
	public void BeginPhase() {
		//Cycle through each enemies turn
        //TODO create temporary list of bullets to enumerate through -- list is being modified during the turn
		for(int i = Bullets.Count-1; i >= 0; i--) {
			Bullet bullet = Bullets[i];
			bullet.BeginPhase();
		}

//		foreach (Bullet bullet in Bullets) {
//			bullet.BeginPhase();
//		}
		
		EndPhase();
	}
	
	public void EndPhase() {
		if (CurrentTurn.CurrentPhase == Turn.Phase.Bullet)
			CurrentTurn.AdvancePhase();
	}
}
