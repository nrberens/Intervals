using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{

    private float moveX, moveZ;

    private PlayerController pc;

    //public Vector3 crosshairPos;


	// Use this for initialization
	void Start () {
	    pc = GetComponentInParent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {

        //crosshairPos = Crosshair.GetCrosshairInWorld();

	    if (!pc.mover.moving && pc.CurrentTurn.CurrentPhase == Turn.Phase.Player) 
	    {
	        moveX = Input.GetAxis("Horizontal");
	        moveZ = Input.GetAxis("Vertical");

	        if (moveX > 0) //if positive vertical, move forward
	        {
	            pc.mover.moving = true; 
	            pc.mover.MoveRight(1);
	        }
	        else if (moveX < 0) //if negative vertical, move backward
	        {
	            pc.mover.moving = true;
	            pc.mover.MoveLeft(1);
	        }
	        else if (moveZ > 0)
	        {
	            pc.mover.moving = true;
	            pc.mover.MoveUp(1);
	        }
	        else if (moveZ < 0)
	        {
	            pc.mover.moving = true;
	            pc.mover.MoveDown(1);
	        }

	    }

	}
}
