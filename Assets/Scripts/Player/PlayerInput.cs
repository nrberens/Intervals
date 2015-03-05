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
	        //TODO track time between inputs, only allow input every half second or so
	        if (allowInput && !pc.acting) {

				//SHOOTING
				if(Input.GetMouseButtonDown(0)) {
					Debug.Log ("Registered click");
				    pc.Shooter.BeginShot();
				}

				//IF PLAYER DOESN'T SHOOT, HANDLE MOVEMENT
                // TODO reinterpret input based on camera rotation
                //When camera rotation between 0-45 and 315-360, normal inputs
                //45.1-135 a = up, w = right, d = down, s = left
                //135.1-225, a = right, w = down, d = left, s = up 
                //225.1 - 315, a = back, w = left, d = up, s = right
                //need to change inputs, not directions
	            moveX = Input.GetAxis("Horizontal");
	            moveZ = Input.GetAxis("Vertical");

	            if (moveX != 0 || moveZ != 0) {
	                pc.acting = true;
	                allowInput = false;
	            }

	            if (moveX > 0) //if positive vertical, move forward
	            {
                    pc.Mover.Move(Direction.Right, 1);
	            }
	            else if (moveX < 0) //if negative vertical, move backward
	            {
                    pc.Mover.Move(Direction.Left, 1);
	            }
	            else if (moveZ > 0) {
                    pc.Mover.Move(Direction.Up, 1);
	            }
	            else if (moveZ < 0) {
                    pc.Mover.Move(Direction.Down, 1);
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
