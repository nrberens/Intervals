using System;
using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{

	private float moveX, moveZ;
	public bool allowInput, playerSelected;

	//public Vector3 crosshairPos;


	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{

		//crosshairPos = Crosshair.GetCrosshairInWorld();

		if (PlayerController.pc.CurrentTurn.CurrentPhase == Turn.Phase.Player) {
			//INPUT PHASE 
			//TODO track time between inputs, only allow input every half second or so
			if (allowInput && !PlayerController.pc.acting) {
				if (!playerSelected) {
					if (Input.GetMouseButtonDown (0)) {
						//TODO create layerMask - test only player layer = 8
						int playerLayer = 8;
						int layerMask = 1 << playerLayer;
						Transform target = GetTargetOfClick (layerMask);
						//If target is player
						if (target.tag == "Player") {
							playerSelected = true;
							PlayerController.pc.Mover.TagMovableNodes ();
							//TODO doesn't seem to work
							PlayerController.pc.Shooter.TagShootableEnemies ();
						} else if(target == null) {
							//Do nothing
						}
					}
				} else if (playerSelected) {
					if (Input.GetMouseButton (0)) {
						//drag to node or enemy
					} else if (Input.GetMouseButtonUp (0)) {
						playerSelected = false;
						PlayerController.pc.Mover.UnTagMovableNodes ();
						PlayerController.pc.Shooter.UnTagShootableEnemies ();

						//TODO create layerMask = test enemies and world layer
						int enemyLayer = 10;
						int worldLayer = 11;
						int layerMask = 1 << enemyLayer | 1 << worldLayer;
						Transform target = GetTargetOfClick (layerMask);
						Debug.Log ("MouseUp - Target = " + target);
						if (target != null) {
							if (target.tag == "Enemy") {
								PlayerController.pc.acting = true;
								allowInput = false;
								PlayerController.pc.Shooter.BeginShot (target);
							} else if (target.tag == "WorldBlock") {
								Direction? dir = PlayerController.pc.Mover.GetTargetDirection (target.Find ("MoveNode").GetComponent<MoveNode> ());
								if (dir != null) {
									PlayerController.pc.acting = true;
									allowInput = false;
									Direction moveDir = (Direction)dir;
									PlayerController.pc.Mover.Move (moveDir, 1);
								}

							}
						}
					}
				}
			}
            //END OF TURN
			//TODO find another way of marking end of turn
            else if (!allowInput && !PlayerController.pc.acting) {
				PlayerController.pc.EndPhase ();
			}
		}
	}

	//On click, get target of mouse click
	public Transform GetTargetOfClick (int layerMask)
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction * 100, Color.green, 5.0f);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
			return hit.transform;
		} else
			return null;
	}

	private Direction GetAdjustedDirection (float moveX, float moveZ, Quaternion rotation)
	{
		float yRot = rotation.eulerAngles.y;
		int value = 0; //0 = North, 1 = East, 2 = South, 3 = West
		int offset = 0; //number of 90 deg turns clockwise

		if (moveX > 0) {
			value = 1;
		} else if (moveX < 0) {
			value = 3;
		} else if (moveZ > 0) {
			value = 0;
		} else if (moveZ < 0) {
			value = 2;
		}

		if ((yRot >= 0 && yRot < 45) || yRot >= 315 && yRot < 360) {
			value += 0;
		} else if (yRot >= 45 && yRot < 135) {
			value += 1;
		} else if (yRot >= 135 && yRot < 225) {
			value += 2;
		} else if (yRot >= 225 && yRot < 315) {
			value += 3;
		}

		value %= 4; //modulus clamps value between 0 and 3

		Direction direction = (Direction)value;

		return direction;
	}

}
