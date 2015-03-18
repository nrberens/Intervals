using System;
using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{

	private float moveX, moveZ;
	public bool allowInput, playerSelected;
	public Transform selectedBlock;

	//public Vector3 crosshairPos;


	// Use this for initialization
	void Start ()
	{
		selectedBlock = null;
	}

	// Update is called once per frame
	void Update ()
	{
		//TODO This is fucking awful spaghetti code and I hate it

		//crosshairPos = Crosshair.GetCrosshairInWorld();

		if (PlayerController.pc.CurrentTurn.CurrentPhase == Turn.Phase.Player) {
			//INPUT PHASE 
			//TODO track time between inputs, only allow input every half second or so
			if (allowInput && !PlayerController.pc.acting) {
				if (!playerSelected) {
					if (Input.GetMouseButtonDown (0)) {
						// create layerMask - test only player layer = 8
						int playerLayer = 8;
						int layerMask = 1 << playerLayer;
						Transform target = GetTargetOfClick (layerMask);
						//If target is player
						if (target.tag == "Player") {
							playerSelected = true;
							PlayerController.pc.Mover.TagMovableNodes ();
							PlayerController.pc.Shooter.TagShootableEnemies ();
						} else if (target == null) {
							//Do nothing
						}
					}
				} else if (playerSelected) {
					if (Input.GetMouseButton (0)) {
						for (int i = 0; i < PlayerController.pc.Shooter.shootableEnemies.Count; i++) {
							EnemyController enemy = PlayerController.pc.Shooter.shootableEnemies [i];
							ShootableNode shootableNode = enemy.Mover.currentNode.transform.parent.GetComponentInChildren<ShootableNode> ();
							if (!PlayerController.pc.Shooter.CheckMeleeRange (enemy.transform))
								shootableNode.currentState = ShootableNode.NodeState.ShootableUnselected;
						}

						//drag to node or enemy
						//create layerMask = test enemies and world layer
						int enemyLayer = 10;
						int worldLayer = 11;
						int layerMask = 1 << enemyLayer | 1 << worldLayer;
						Transform target = GetTargetOfClick (layerMask);
						if (target != null) {
							if (target.tag == "WorldBlock") {
								//no selected block, select new block
								if (selectedBlock == null) {
									MoveNode node = target.GetComponentInChildren<MoveNode> ();
									if (node.movable) {
										selectedBlock = target;
										MovableNode nodeController = target.GetComponentInChildren<MovableNode> ();
										nodeController.currentState = MovableNode.NodeState.MovableSelected;
									}
									//mouse over new block
								} else if (target != selectedBlock) {
									//unselect previous block
									MovableNode previousNode = selectedBlock.GetComponentInChildren<MovableNode> ();
									previousNode.currentState = MovableNode.NodeState.MovableUnselected;
									selectedBlock = null;
									//select new block
									MoveNode node = target.GetComponentInChildren<MoveNode> ();
									if (node.movable) {
										selectedBlock = target;
										MovableNode nodeController = target.GetComponentInChildren<MovableNode> ();
										nodeController.currentState = MovableNode.NodeState.MovableSelected;
									}
								}
							} else {
								//not a block, unselect previous block
								//MovableNode previousNode = target.GetComponentInChildren<MovableNode>();
								//previousNode.currentState = MovableNode.NodeState.MovableUnselected;

								if (target.tag == "Enemy")  {
									if (PlayerController.pc.Shooter.CheckMeleeRange (target)) {
										EnemyController enemy = target.GetComponent<EnemyController> ();
											MovableNode node =
												enemy.Mover.currentNode.transform.parent
													.GetComponentInChildren<MovableNode> ();
											node.currentState = MovableNode.NodeState.MeleeableSelected;
									} else {
										EnemyController enemy = target.GetComponent<EnemyController> ();
										if (enemy.shootable) {
											ShootableNode node =
	                                            enemy.Mover.currentNode.transform.parent
	                                                .GetComponentInChildren<ShootableNode> ();
											node.currentState = ShootableNode.NodeState.ShootableSelected;
										}
									}
								}
							}
						} else if (target == null) {
							MovableNode previousNode = selectedBlock.GetComponentInChildren<MovableNode> ();
							previousNode.currentState = MovableNode.NodeState.MovableUnselected;
							selectedBlock = null;
						}
						
					} else if (Input.GetMouseButtonUp (0)) {
						playerSelected = false;
						selectedBlock = null;
						PlayerController.pc.Mover.UnTagMovableNodes ();
						PlayerController.pc.Shooter.UnTagShootableEnemies ();

						// create layerMask = test enemies and world layer
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
                //END OF TURN
			} else if (!allowInput && !PlayerController.pc.acting) {
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
