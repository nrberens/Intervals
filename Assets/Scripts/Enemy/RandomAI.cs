using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/**
 * 1. Enemy Spawns
 * 2. Finds a place to move
 * 3. Looks in that direction
 * 4. Triggers move animation
 * 5. Decides what to do next
 * 6. Moves again OR Attacks
 * 7. Repeat
 **/

//[RequireComponent(typeof(Rigidbody))]
public class RandomAI : FSM
{

	public enum FSMState
	{
		None,
		Wander,
		Dead,
	}

	private EnemyController _ec;
	public FSMState CurState;
	public GameObject Bullet;
	public int Health;
	public float AttackDistance;
	public float AggroDistance;
	public int Distance;

	// Use this for initialization
	protected override void Initialize ()
	{
		_ec = GetComponentInParent<EnemyController> ();
	}

	// FSMUpdate is called once per frame
	protected override void FSMUpdate ()
	{
	}

	// Update once per turn
	public override void UpdateAI ()
	{

		switch (CurState) {
		case FSMState.Wander:
			UpdateWanderState ();
			break;
		case FSMState.Dead:
			UpdateDeadState ();
			break;
		}

		if (Health <= 0) {
			CurState = FSMState.Dead;
		}
	}

	protected void UpdateWanderState ()
	{  
		//check for LOS to player, if player, shoot in that direction,
		//else, move on to random behavior
		Direction? shootDir = CheckLOSToPlayer();

		if (shootDir != null) {
			Direction dir = (Direction) shootDir;	//convert from nullable Direction?
			StartCoroutine(_ec.Shooter.Shoot (dir));	
		} else {
			int moveCoin = UnityEngine.Random.Range (0, 4);
			//HACK default direction is north
			Direction randomDir = Direction.North;

			bool directionValid = false;
			List<Direction> potentialDirs = new List<Direction> { Direction.North, Direction.South, Direction.East, Direction.West };

			do {
				//TODO track attempted directions -- if no directions work, just end turn
				if (potentialDirs.Count == 0) {
					Debug.Log ("No valid directions, just staying put.");
					_ec.acting = false;
					_ec.EndPhase ();
					break;
				}
				randomDir = potentialDirs [Random.Range (0, potentialDirs.Count - 1)];
				potentialDirs.Remove (randomDir);
				directionValid = _ec.Mover.CheckForValidMovement (randomDir, Distance);
			} while (!directionValid);

			if (directionValid) {
				_ec.Mover.Move (randomDir, Distance);
			}
		}
	}

	protected void UpdateDeadState ()
	{
	}

	private void Explode ()
	{
		float rndX = UnityEngine.Random.Range (10.0f, 30.0f);
		float rndZ = UnityEngine.Random.Range (10.0f, 30.0f);
		for (int i = 0; i < 3; i++) {
			GetComponent<Rigidbody> ().AddExplosionForce (10000.0f,
transform.position - new Vector3 (rndX, 10.0f, rndZ), 40.0f, 10.0f);
			GetComponent<Rigidbody> ().velocity = transform.TransformDirection (new Vector3 (rndX, 20.0f, rndZ));
		}
		Destroy (gameObject, 1.5f);
	}

	void TakeDamage (int damage)
	{
		Health -= damage;
	}

	private Direction GetRandomDirection ()
	{
		Array dirArray = Enum.GetValues (typeof(Direction));
		Direction dir = (Direction)dirArray.GetValue (UnityEngine.Random.Range (0, dirArray.Length));
		return dir;
	}

    private Direction? CheckLOSToPlayer() {
        Vector3 rayOrigin = new Vector3(transform.position.x, 1.0f, transform.position.z);
        Ray ray = new Ray(rayOrigin, Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            Debug.Log(gameObject + " ray hit " + hit.transform.name);
            if (hit.transform.tag == "Player") {
                return Direction.North;
            }
        }

        ray = new Ray(rayOrigin, Vector3.back);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.transform.tag == "Player") {
                return Direction.South;
            }
        }

        ray = new Ray(rayOrigin, Vector3.left);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.transform.tag == "Player") {
                return Direction.West;
            }
        }

        ray = new Ray(rayOrigin, Vector3.right);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.transform.tag == "Player") {
                return Direction.East;
            }
        }

        return null;
    }

}