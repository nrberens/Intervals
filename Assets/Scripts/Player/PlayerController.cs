using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour, ITurnBased {

    public PlayerMover Mover;
    public PlayerInventory Inventory;
    public PlayerInput Input;
	public PlayerShooter Shooter;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }

	// Use this for initialization
	void Start () {
	    Mover = GetComponentInParent<PlayerMover>();
	    Inventory = GetComponentInParent<PlayerInventory>();
	    Input = GetComponentInParent<PlayerInput>();
		Shooter = GetComponentInParent<PlayerShooter>();
	    CurrentTurn = FindObjectOfType<Turn>();

	    acting = false;
	}

    void Update() {

    }

    public void BeginPhase() {
            Input.allowInput = true;
    }

    public void EndPhase() {
        if(CurrentTurn.CurrentPhase == Turn.Phase.Player)
            CurrentTurn.AdvancePhase();   
        else Debug.Log("Calling AdvancePhase from the wrong object!");
    }
}
