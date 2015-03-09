using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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
    private Map map;

    public FSMState CurState;
    private Transform _target;
    public GameObject Bullet;
    private bool _bDead;
    public int Health;
    public float AttackDistance;
    public float AggroDistance;
    public int Distance;

    public MoveNode targetNode;

    // Use this for initialization
    protected override void Initialize() {
        _ec = GetComponentInParent<EnemyController>();
        map = GameObject.Find("World").GetComponent<Map>();
    }

    // FSMUpdate is called once per frame
    protected override void FSMUpdate() { }

    void LateUpdate() {
        Debug.DrawLine(transform.position, targetNode.transform.position, Color.red);
    }

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
        //Check for LOS to player
        Direction? shootDir = CheckLOSToPlayer();

        if (shootDir != null) {
            _ec.Shooter.Shoot((Direction) shootDir);
            _ec.acting = false;
            _ec.EndPhase();
            return;
        }

        //Check for LOS to an LOS flagged node  
        MoveNode closestLOSNode = map.FindClosestLOSNode(transform);
        if (closestLOSNode != null) {
            Debug.Log("Closest LOS node for " + transform.name + " is: (" + closestLOSNode.x + "," + closestLOSNode.z +
                      ")");
            //TODO move towards closestNode
            targetNode = closestLOSNode;
            Direction moveDir = MoveNode.DirectionToNode(_ec.Mover.currentNode, closestLOSNode);
            _ec.Mover.Move(moveDir, Distance);
        } else {
            //TODO No LOS so move randomly
            Direction moveDir = GetRandomDirection();
            _ec.Mover.Move(moveDir, Distance);
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

    private Direction? CheckLOSToPlayer() {
        Ray ray = new Ray(transform.position, Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.transform.tag == "Player") {
                return Direction.North;
            }
        }        

        ray = new Ray(transform.position, Vector3.back);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.transform.tag == "Player") {
                return Direction.South;
            }
        }        

        ray = new Ray(transform.position, Vector3.left);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.transform.tag == "Player") {
                return Direction.West;
            }
        }        

        ray = new Ray(transform.position, Vector3.right);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.transform.tag == "Player") {
                return Direction.East;
            }
        }

        return null;
    }

}