using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{

    private float moveX, moveZ;

    private PlayerController pc;

    public bool allowInput; 

    //public Vector3 crosshairPos;


	// Use this for initialization
	void Start () {
	    pc = GetComponentInParent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {

        //crosshairPos = Crosshair.GetCrosshairInWorld();

	    if (pc.CurrentTurn.CurrentPhase == Turn.Phase.Player) {
	        //INPUT PHASE 
	        if (allowInput && !pc.acting) {

                //TODO mouse click to shoot
                // Get mouse click
                // Cast ray into world, get target

				//SHOOTING
				if(Input.GetMouseButtonDown(0)) {
					Debug.Log ("Registered click");
				    pc.Shooter.BeginShot();
				}

				//IF PLAYER DOESN'T SHOOT, HANDLE MOVEMENT
	            moveX = Input.GetAxis("Horizontal");
	            moveZ = Input.GetAxis("Vertical");

	            if (moveX != 0 || moveZ != 0) {
	                pc.acting = true;
	                allowInput = false;
	            }

	            if (moveX > 0) //if positive vertical, move forward
	            {
	                pc.Mover.MoveRight(1);
	            }
	            else if (moveX < 0) //if negative vertical, move backward
	            {
	                pc.Mover.MoveLeft(1);
	            }
	            else if (moveZ > 0) {
	                pc.Mover.MoveUp(1);
	            }
	            else if (moveZ < 0) {
	                pc.Mover.MoveDown(1);
	            }

	        }
	        //ACTION PHASE 
	        else if (!allowInput && pc.acting) {
                //wait for signal that movement or firing is done
	        }
	        //END OF TURN
	        else if (!allowInput && !pc.acting) {
                pc.EndPhase();
	        }
	    }
	}

}
