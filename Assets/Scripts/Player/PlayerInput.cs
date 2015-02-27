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

				if(Input.GetMouseButtonDown(0)) {
					Debug.Log ("Registered click");
					Transform target = GetTargetOfClick();
                	// Check for valid target
					if(target != null && CheckValidTarget(target)) {
						//shoot target
						Debug.Log("Shooting " + target);
					}
				}

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

	//On click, get target of mouse click
	private Transform GetTargetOfClick() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast (ray, out hit)) {
			return hit.transform;
		} else return null;
	}

	private bool CheckValidTarget (Transform target) {
		return (target.tag == "Enemy");
	}
}
