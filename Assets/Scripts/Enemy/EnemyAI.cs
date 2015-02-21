using UnityEngine;
using System.Collections;
using System;

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
public class EnemyAI : FSM {

    public enum FSMState {
        None,
        Attack,
        Wander,
        Dead,
    }

	public enum Direction {
		Up,
		Down,
		Left,
		Right,
	}


    private EnemyController ec;

    public FSMState curState;
    private Transform target;
    public GameObject bullet;
    private bool bDead;
    public int health;
    public float attackDistance;
    public float aggroDistance;
	public int distance;

    // Use this for initialization
    protected override void Initialize() {
        ec = GetComponentInParent<EnemyController>();
    }

    // FSMUpdate is called once per frame
    protected override void FSMUpdate() {
        if (ec.CurrentTurn.CurrentPhase == Turn.Phase.Enemy) {
            switch (curState) {
                case FSMState.Wander:
                    UpdateWanderState();
                    break;
                case FSMState.Attack:
                    UpdateAttackState();
                    break;
                case FSMState.Dead:
                    UpdateDeadState();
                    break;
            }

            elapsedTime += Time.deltaTime;

            if (health <= 0) {
                curState = FSMState.Dead;
            }

            // TODO Move this out of single enemy script - must trigger after all enemies have taken turn
            //CurrentTurn.AdvancePhase();
        }
    }

    protected void UpdateWanderState() {  //Choose random new position
		Direction randomDir = GetRandomDirection();

		switch(randomDir) {
		case Direction.Up:
			ec.mover.MoveUp(distance);
			break;
		case Direction.Down:
			ec.mover.MoveDown(distance);
			break;
		case Direction.Left:
			ec.mover.MoveLeft(distance);
			break;
		case Direction.Right:
			ec.mover.MoveRight(distance);
			break;
		}
    }

    protected void UpdateAttackState() {
    }

    protected void UpdateDeadState() {
    }

    private void Explode() {
        float rndX = UnityEngine.Random.Range(10.0f, 30.0f);
        float rndZ = UnityEngine.Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++) {
            rigidbody.AddExplosionForce(10000.0f,
transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            rigidbody.velocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
        }
        Destroy(gameObject, 1.5f);
    }

    private void ShootBullet() {
        if (elapsedTime >= fireRate) {
            Transform spawnedBullet = (Transform)GameObject.Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
            Bullet bulletController = spawnedBullet.GetComponent<Bullet>();
            //bulletController.drone = transform;
            bulletController.ShootAtPoint(player.transform.position);
            elapsedTime = 0.0f;
        }
    }

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Bullet") {
            TakeDamage(coll.gameObject.GetComponent<Bullet>().damage);
        }
    }

    void TakeDamage(int damage) {
        health -= damage;
    }

	private Direction GetRandomDirection ()
	{
		Array dirArray = Enum.GetValues (typeof(Direction));
		Direction dir = (Direction)dirArray.GetValue(UnityEngine.Random.Range (0, dirArray.Length));
		return dir;
	}

}