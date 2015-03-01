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
public class RandomAI : FSM {

    public enum FSMState {
        None,
        Wander,
        Dead,
    }

    private EnemyController _ec;

    public FSMState CurState;
    private Transform _target;
    public GameObject Bullet;
    private bool _bDead;
    public int Health;
    public float AttackDistance;
    public float AggroDistance;
    public int Distance;

    // Use this for initialization
    protected override void Initialize() {
        _ec = GetComponentInParent<EnemyController>();
    }

    // FSMUpdate is called once per frame
    protected override void FSMUpdate() { }

    // Update once per turn
    public override void UpdateAI() {

        switch (CurState) {
            case FSMState.Wander:
                UpdateWanderState();
                break;
            case FSMState.Dead:
                UpdateDeadState();
                break;
        }

        if (Health <= 0) {
            CurState = FSMState.Dead;
        }
    }

    protected void UpdateWanderState() {  //Choose random new position

        //random chance of shooting, otherwise get randomDir and move
        int shootCoin = UnityEngine.Random.Range(0, 5);
        Direction randomDir = GetRandomDirection();

        if (shootCoin == 0) {
            _ec.Shooter.Shoot(randomDir);
        }
        else {

            switch (randomDir) {
                case Direction.Up:
                    _ec.Mover.MoveUp(Distance);
                    break;
                case Direction.Down:
                    _ec.Mover.MoveDown(Distance);
                    break;
                case Direction.Left:
                    _ec.Mover.MoveLeft(Distance);
                    break;
                case Direction.Right:
                    _ec.Mover.MoveRight(Distance);
                    break;
            }
        }
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

    void TakeDamage(int damage) {
        Health -= damage;
    }

    private Direction GetRandomDirection() {
        Array dirArray = Enum.GetValues(typeof(Direction));
        Direction dir = (Direction)dirArray.GetValue(UnityEngine.Random.Range(0, dirArray.Length));
        return dir;
    }

}