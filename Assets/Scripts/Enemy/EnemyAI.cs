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

    public FSMState curState;
    private float curSpeed;
    private float curRotSpeed;
    private Transform target;
    public GameObject bullet;
    private bool bDead;
    public int health;
    public float attackDistance;
    public float aggroDistance;

    // Use this for initialization
    protected override void Initialize() {
    }

    // Update is called once per frame
    protected override void FSMUpdate() {
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
    }

    protected void UpdateWanderState() {  //Choose random new position
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

}