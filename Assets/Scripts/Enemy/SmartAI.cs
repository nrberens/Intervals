using UnityEngine;
using System.Collections;
using System;

/**
 * 1. Enemy Spawns
 * 2. Enemy checks for LOS to an LOS flagged node (if yes, move toward that node)
 * 3. If on LOS node, attack
 * 4. If no, check whether playerx or playery is closer
 * 5. Move to match playerx or playery
 * 6. Check for line of sight
 * 7. If no LOS but x or y matches, then there's an obstacle in the way - move randomly?
 **/

//[RequireComponent(typeof(Rigidbody))]
public class SmartAI : FSM {

    public enum FSMState {
        None,
        SeekLOS,
        Attack,
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
            case FSMState.SeekLOS:
                UpdateSeekLOSState();
                break;
            case FSMState.Attack:
                UpdateAttackState();
                break;
            case FSMState.Dead:
                UpdateDeadState();
                break;
        }

        if (Health <= 0) {
            CurState = FSMState.Dead;
        }
    }

    protected void UpdateSeekLOSState() {
        //Check for LOS to an LOS flagged node

    }

    protected void UpdateAttackState() {
        
    }

    protected void UpdateDeadState() {
    }

    private void Explode() {
        float rndX = UnityEngine.Random.Range(10.0f, 30.0f);
        float rndZ = UnityEngine.Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++) {
            GetComponent<Rigidbody>().AddExplosionForce(10000.0f,
transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
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