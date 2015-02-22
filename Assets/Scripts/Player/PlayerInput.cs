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

	    if (allowInput) 
	    {
	        moveX = Input.GetAxis("Horizontal");
	        moveZ = Input.GetAxis("Vertical");

	        if (moveX > 0) //if positive vertical, move forward
	        {
	            pc.acting = true;
	            pc.mover.MoveRight(1);
	        }
	        else if (moveX < 0) //if negative vertical, move backward
	        {
	            pc.acting = true;
	            pc.mover.MoveLeft(1);
	        }
	        else if (moveZ > 0)
	        {
	            pc.acting = true;
	            pc.mover.MoveUp(1);
	        }
	        else if (moveZ < 0)
	        {
	            pc.acting = true;
	            pc.mover.MoveDown(1);
	        }

            // TODO HERE
	    }

	}
}
