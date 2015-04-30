using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

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
public class MeleeAI : FSM {

    public enum FSMState {
        None,
        SeekLOS,
        Attack,
        Dead,
    }

    private EnemyController _ec;
    private Map map;

    public FSMState CurState;
    public GameObject Bullet;
    public int Health;
    public float AttackDistance;
    public float AggroDistance;
    public int Distance;
    public int cqbDistance;

    public MoveNode targetNode;

    // Use this for initialization
    protected override void Initialize() {
        _ec = GetComponentInParent<EnemyController>();
        map = GameObject.Find("World").GetComponent<Map>();
    }

    // FSMUpdate is called once per frame
    protected override void FSMUpdate() { }

    void LateUpdate() {
        //Debug.DrawLine(transform.position, targetNode.transform.position, Color.red);
    }

    // Update once per turn
    public override void UpdateAI() {
        UpdateSeekLOSState();
    }

    protected void UpdateSeekLOSState() {
        //check for LOS to player, if player, shoot in that direction,
        Direction? shootDir = CheckLOSToPlayer();

        if (shootDir != null) {
            int playerX = PlayerController.pc.Mover.currentNode.x;
            int playerZ = PlayerController.pc.Mover.currentNode.z;
            int enemyX = _ec.Mover.currentNode.x;
            int enemyZ = _ec.Mover.currentNode.z;

            int distance = 0;

            if (playerX == enemyX)
                distance = Mathf.Abs(playerZ - enemyZ);
            else if (playerZ == enemyZ)
                distance = Mathf.Abs(playerX - enemyX);

                Direction dir = (Direction) shootDir; //convert from nullable Direction?
            if (distance > 0 && distance <= cqbDistance) {
                Debug.Log("Melee AI Attacking from distance of " + distance);
                StartCoroutine(_ec.Shooter.Shoot(dir));
            }
            else {
                //TODO check for valid movement
                if (_ec.Mover.CheckForValidMovement(dir, Distance)) {
                    _ec.Mover.Move(dir, Distance);
                    return;
                }
            }
        }

        //check for LOS to LOS-flagged node
        //if yes, move toward that node
        //if no, move randomly
        MoveNode closestLOSNode = GetClosestLOSNode();

        if (closestLOSNode != null) {
            Debug.Log(transform + ": Closest LOS Node is (" + closestLOSNode.x + "," + closestLOSNode.z + ")");
            //TODO get direction toward LOS Node and move
            Direction? moveDir = GetDirectionToNode(_ec.Mover.currentNode, closestLOSNode);

            if (moveDir != null) {
                //TODO check for valid movement
                Direction dir = (Direction)moveDir;
                if (_ec.Mover.CheckForValidMovement(dir, Distance)) {
                    _ec.Mover.Move(dir, Distance);
                    return;
                }
            } else {
                Debug.Log("Direction returned null.");
            }
        }

        //No LOS, move randomly
        int moveCoin = UnityEngine.Random.Range(0, 4);
        //HACK default direction is north
        Direction randomDir = Direction.North;

        bool directionValid = false;
        List<Direction> potentialDirs = new List<Direction> { Direction.North, Direction.South, Direction.East, Direction.West };

        do {
            if (potentialDirs.Count == 0) {
                Debug.Log("No valid directions, just staying put.");
                _ec.EndPhase();
                break;
            }
            randomDir = potentialDirs[UnityEngine.Random.Range(0, potentialDirs.Count - 1)];
            potentialDirs.Remove(randomDir);
            directionValid = _ec.Mover.CheckForValidMovement(randomDir, Distance);
        } while (!directionValid);

        if (directionValid) {
            _ec.Mover.Move(randomDir, Distance);
        }
    }

    private Direction GetRandomDirection() {
        Array dirArray = Enum.GetValues(typeof(Direction));
        Direction dir = (Direction)dirArray.GetValue(UnityEngine.Random.Range(0, dirArray.Length));
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

    private MoveNode GetClosestLOSNode() {
        List<MoveNode> losNodes = new List<MoveNode>();
        MoveNode currentNode = _ec.Mover.currentNode;
        //if node blocks movement or LOS, cancel that direction
        //if LOS node found, add to a list

        //search north
        for (int z = currentNode.z + 1; z < map.mapLength; z++) {
            int x = currentNode.x;
            MoveNode thisNode = map.Nodes[x, z];
            if (thisNode.blocksLOS || thisNode.blocksMovement) {
                break;
            }

            if (thisNode.LOSToPlayer) {
                losNodes.Add(thisNode);
                break;
            }
        }


        //search south
        for (int z = currentNode.z - 1; z > 0; z--) {
            int x = currentNode.x;
            MoveNode thisNode = map.Nodes[x, z];
            if (thisNode.blocksLOS || thisNode.blocksMovement) {
                break;
            }

            if (thisNode.LOSToPlayer) {
                losNodes.Add(thisNode);
                break;
            }
        }


        //search left 
        for (int x = currentNode.x - 1; x > 0; x--) {
            int z = currentNode.z;
            MoveNode thisNode = map.Nodes[x, z];
            if (thisNode.blocksLOS || thisNode.blocksMovement) {
                break;
            }

            if (thisNode.LOSToPlayer) {
                losNodes.Add(thisNode);
                break;
            }
        }


        //search right 
        for (int x = currentNode.x + 1; x > map.mapWidth; x++) {
            int z = currentNode.z;
            MoveNode thisNode = map.Nodes[x, z];
            if (thisNode.blocksLOS || thisNode.blocksMovement) {
                break;
            }

            if (thisNode.LOSToPlayer) {
                losNodes.Add(thisNode);
                break;
            }
        }

        if (losNodes.Count == 0) return null;
        if (losNodes.Count == 1) return losNodes[0];
        if (losNodes.Count > 1) {
            //TODO find closest node and return
            return losNodes[0];
        } else return null;
    }

    private Direction? GetDirectionToNode(MoveNode from, MoveNode to) {
        int deltaX = from.x - to.x;
        int deltaZ = from.z - to.z;

        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaZ)) {
            if (deltaX > 0)  //enemy is east of node, must move west
                return Direction.West;
            if (deltaX < 0)
                return Direction.East;
        } else if (Mathf.Abs(deltaZ) > Mathf.Abs(deltaX)) {
            if (deltaZ > 0) //enemy is north of node, must move south
                return Direction.South;
            if (deltaZ < 0)
                return Direction.North;
        }

        return null;
    }

}