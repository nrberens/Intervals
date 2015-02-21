using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour, ITurnBased
{

    private float moveX, moveZ;

    private IMover mover;
    private PlayerInventory inventory;
    public Turn CurrentTurn { get; set; }

    //public Vector3 crosshairPos;


	// Use this for initialization
	void Start ()
	{
	    mover = GetComponentInParent<PlayerMover>();
	    inventory = GetComponentInParent<PlayerInventory>();
	    CurrentTurn = FindObjectOfType<Turn>();
	}
	
	// Update is called once per frame
	void Update () {

        //crosshairPos = Crosshair.GetCrosshairInWorld();

	    if (!mover.moving && CurrentTurn.CurrentPhase == Turn.Phase.Player) 
	    {
	        moveX = Input.GetAxis("Horizontal");
	        moveZ = Input.GetAxis("Vertical");

	        if (moveX > 0) //if positive vertical, move forward
	        {
	            mover.moving = true; 
	            mover.MoveRight(1);
	        }
	        else if (moveX < 0) //if negative vertical, move backward
	        {
	            mover.moving = true;
	            mover.MoveLeft(1);
	        }
	        else if (moveZ > 0)
	        {
	            mover.moving = true;
	            mover.MoveUp(1);
	        }
	        else if (moveZ < 0)
	        {
	            mover.moving = true;
	            mover.MoveDown(1);
	        }

	        if (moveX != 0 || moveZ != 0) {
	            CurrentTurn.AdvancePhase();
	        }

	    }

	}
}
