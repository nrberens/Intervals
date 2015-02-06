using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{

    private float moveX, moveZ;

    private IMover mover;
    private PlayerInventory inventory;

    public Vector3 crosshairPos;

	// Use this for initialization
	void Start ()
	{
	    mover = GetComponentInParent<PlayerMovement>();
	    inventory = GetComponentInParent<PlayerInventory>();
	}
	
	// Update is called once per frame
	void Update () {

        crosshairPos = Crosshair.GetCrosshairInWorld();

	    if (!mover.moving)
	    {
	        moveX = Input.GetAxis("Horizontal");
	        moveZ = Input.GetAxis("Vertical");

            //TODO add diagonal movement
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
	            mover.MoveForward(1);
	        }
	        else if (moveZ < 0)
	        {
	            mover.moving = true;
	            mover.MoveBackward(1);
	        }
	    }

        if (Input.GetButton("Fire1")) {
            inventory.weapon.SendMessage("Shoot", crosshairPos);
        } 
	
	}
}
