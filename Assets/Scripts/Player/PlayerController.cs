﻿using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour, ITurnBased {

    public PlayerMover mover;
    public PlayerInventory inventory;
    public PlayerInput input;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }

    private bool doneWithTurn;

	// Use this for initialization
	void Start () {
	    mover = GetComponentInParent<PlayerMover>();
	    inventory = GetComponentInParent<PlayerInventory>();
	    input = GetComponentInParent<PlayerInput>();
	    CurrentTurn = FindObjectOfType<Turn>();
	    Transform bulletSpawnPoint = inventory.weapon.Find("BulletPoint");

	    acting = false;
	}

    public void EndPhase() {
        if(CurrentTurn.CurrentPhase == Turn.Phase.Player)
            CurrentTurn.AdvancePhase();   
    }
}
