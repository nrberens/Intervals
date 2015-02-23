using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletsController : MonoBehaviour, ITurnBased {

	public Turn CurrentTurn { get; set; }
	public bool acting { get; set; }

	public Queue<Bullet> Bullets;

	// Use this for initialization
	void Start () {
		Bullets = new Queue<Bullet>();
		CurrentTurn = FindObjectOfType<Turn>();
	}
	
	public void BeginPhase() {
		//Cycle through each enemies turn
		foreach (Bullet bullet in Bullets) {
			bullet.BeginPhase();
		}
		
		EndPhase();
	}
	
	public void EndPhase() {
		if (CurrentTurn.CurrentPhase == Turn.Phase.Bullet)
			CurrentTurn.AdvancePhase();
	}
}
