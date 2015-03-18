using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShooter : MonoBehaviour {

    public Transform bulletPrefab;
    public Transform weapon;
    public Transform bulletSpawnPoint;
    public Transform muzzleFlash;
    public Light muzzleFlashLight;
    public float rotateTime;

    public List<EnemyController> shootableEnemies { get; set; }

    // Use this for initialization
    void Start() {
        bulletSpawnPoint = transform.FindChild("BulletSpawnPoint");
        muzzleFlashLight = muzzleFlash.GetComponent<Light>();
        muzzleFlashLight.enabled = false;
		shootableEnemies = new List<EnemyController>();
    }

    // Update is called once per frame
    void Update() {

    }

    void LateUpdate() {
        //HACK I don't like this but i'm not sure where else to put it
        for (int i = 0; i < shootableEnemies.Count; i++) {
            EnemyController enemy = shootableEnemies[i];
            MovableNode movableNode = enemy.Mover.currentNode.transform.parent.GetComponentInChildren<MovableNode>();
			ShootableNode shootableNode = enemy.Mover.currentNode.transform.parent.GetComponentInChildren<ShootableNode>();
            if (movableNode.currentState == MovableNode.NodeState.MovableSelected) {
                movableNode.currentState = MovableNode.NodeState.MeleeableSelected;
            } else if (movableNode.currentState == MovableNode.NodeState.MovableUnselected) {
                movableNode.currentState = MovableNode.NodeState.MeleeableUnselected;
            }
        }
    }

    //On click, get target of mouse click
    public Transform GetTargetOfClick() {
        //target layers EXCLUDING layer 9 - obstacles
        int layerMask = 1 << 9;
        layerMask = ~layerMask;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 5.0f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            return hit.transform;
        } else return null;
    }

    public bool CheckValidTarget(Transform target) {
        if (target.tag == "Enemy") {
            MoveNode targetNode = target.GetComponent<EnemyMover>().currentNode;
            bool validPosition = CheckValidPosition(targetNode);
            bool validLOS = CheckValidLOS(target);

            if (validPosition && validLOS) return true;
        }
        return false;
    }

    public bool CheckValidPosition(MoveNode targetNode) {
        MoveNode playerNode = PlayerController.pc.Mover.currentNode;

        //if player is in same row or column
        if (playerNode.x == targetNode.x || playerNode.z == targetNode.z) return true;
        else return false;
    }

    public bool CheckValidLOS(Transform target) {
        Vector3 rayOrigin = new Vector3(transform.position.x, 1.0f, transform.position.z);
        Vector3 rayTarget = new Vector3(target.position.x, 1.0f, target.position.z);
        Ray ray = new Ray(rayOrigin, (rayTarget - rayOrigin));
        RaycastHit hit;

        //Check against colliders EXCEPT player layer (layer 8)
        //int layerMask = 1 << 8;
        //layerMask = ~layerMask;
        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) { 
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            Debug.Log("Raycast hit " + hit.transform);
            if (hit.transform.tag == "Obstacle") {
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5.0f);
                return false;
            }

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue, 5.0f);
        }
        return true;
    }

    public bool CheckMeleeRange(Transform target) {
        //TODO only accept EnemyControllers
        EnemyController enemy = target.GetComponent<EnemyController>();
        int xDistance = Mathf.Abs(PlayerController.pc.Mover.currentNode.x - enemy.Mover.currentNode.x);
        int zDistance = Mathf.Abs(PlayerController.pc.Mover.currentNode.z - enemy.Mover.currentNode.z);

        if (xDistance == 1 && zDistance == 0) return true;
        else if (xDistance == 0 && zDistance == 1) return true;
        else return false;

    }

    public void BeginShot(Transform target) {
        //TODO change method to only except EnemyControllers
        if (target != null && PlayerController.pc.Shooter.CheckValidTarget(target)) {
            PlayerController.pc.Input.allowInput = false;
            if (CheckMeleeRange(target)) {
                //switch to melee mesh
                StartCoroutine(Melee(target));
            } else {
                //switch to firing mesh
                PlayerController.pc.lowReadyMesh.SetActive(false);
                PlayerController.pc.firingMesh.SetActive(true);
                StartCoroutine(Shoot(target));
            }
        }
    }

    public IEnumerator Shoot(Transform target) {
        //TODO make sure that the enemy struck with the bullet dies and is removed, not the enemy clicked on, if both are in LOS
        // Look in direction
        yield return StartCoroutine(RotateToTarget(target));

        //spawn bullet
        StartCoroutine(PlayerController.pc.Mover.ObjectWiggle(transform.position));
        Transform newBullet = (Transform)Instantiate(bulletPrefab);
        newBullet.position = bulletSpawnPoint.position;
        newBullet.rotation = transform.rotation;
        PlayerBullet newBulletScript = newBullet.GetComponent<PlayerBullet>();
        newBulletScript.spawnPoint = bulletSpawnPoint;
        newBulletScript.currentNode = PlayerController.pc.Mover.currentNode;
        newBulletScript.targetNode = target.GetComponent<EnemyMover>().currentNode;
        //bullet travels to enemy
        newBulletScript.TranslateBullet(newBulletScript.targetNode);
        //enemy dies
        Debug.Log(target + " killed!");
        //target.GetComponent<EnemyController>().TakeDamage(newBullet);
        yield return StartCoroutine(MuzzleFlash());
        yield return new WaitForSeconds(0.5f);
        PlayerController.pc.firingMesh.SetActive(false);
        PlayerController.pc.lowReadyMesh.SetActive(true);
        PlayerController.pc.acting = false;
    }

    public IEnumerator Melee(Transform target) {
        EnemyController enemy = target.GetComponent<EnemyController>();
        MoveNode targetNode = enemy.Mover.currentNode;
        yield return StartCoroutine(RotateToTarget(target));
        PlayerController.pc.lowReadyMesh.SetActive(false);
        PlayerController.pc.meleeMesh.SetActive(true);
        Debug.Log(target + " killed!");
        enemy.TakeDamage(transform);
        //TODO move player to target node
        PlayerController.pc.Mover.currentNode.RemoveFromNode(gameObject);
        targetNode.AddToNode(gameObject);

        StartCoroutine(PlayerController.pc.Mover.MoveToNode(targetNode));
        PlayerController.pc.Mover.currentNode = targetNode;
        PlayerController.pc.meleeMesh.SetActive(false);
        PlayerController.pc.lowReadyMesh.SetActive(true);
    }

    public IEnumerator MuzzleFlash() {
        float startTime = Time.time;
        const float flashTime = 0.1f;

        while (Time.time < flashTime + startTime) {
            muzzleFlashLight.enabled = true;

            yield return null;
        }
        muzzleFlashLight.enabled = false;
    }


    public IEnumerator RotateToTarget(Transform target) {
        float startTime = Time.time;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(target.position - transform.position);
        Debug.DrawRay(transform.position, target.position - transform.position, Color.red, 3.0f);

        while (Time.time < rotateTime + startTime) {
            transform.rotation = Quaternion.Slerp(startRot, endRot, (Time.time - startTime) / rotateTime);
            yield return null;
        }
    }

    public void TagShootableEnemies() {
        shootableEnemies = new List<EnemyController>();

        Vector3 rayOrigin = new Vector3(transform.position.x, 1.0f, transform.position.z);
        Ray ray = new Ray(rayOrigin, Vector3.forward);
        RaycastHit hit;
        //Debug.DrawRay(rayOrigin, Vector3.forward*10, Color.blue, 5.0f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            Debug.Log("North Ray hit " + hit.transform);
            if (hit.transform.tag == "Enemy") {
                EnemyController ec = hit.transform.GetComponent<EnemyController>();
                ec.shootable = true;
                shootableEnemies.Add(ec);
            }
        }

        ray = new Ray(rayOrigin, Vector3.back);
        //Debug.DrawRay(rayOrigin, Vector3.back*10, Color.blue, 5.0f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            Debug.Log("South Ray hit " + hit.transform);
            if (hit.transform.tag == "Enemy") {
                EnemyController ec = hit.transform.GetComponent<EnemyController>();
                ec.shootable = true;
                shootableEnemies.Add(ec);
            }
        }

        ray = new Ray(rayOrigin, Vector3.left);
        //Debug.DrawRay(rayOrigin, Vector3.left*10, Color.blue, 5.0f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            Debug.Log("East Ray hit " + hit.transform);
            if (hit.transform.tag == "Enemy") {
                EnemyController ec = hit.transform.GetComponent<EnemyController>();
                ec.shootable = true;
                shootableEnemies.Add(ec);
            }
        }

        ray = new Ray(rayOrigin, Vector3.right);
        //Debug.DrawRay(rayOrigin, Vector3.right*10, Color.blue, 5.0f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.transform.tag == "Enemy") {
                Debug.Log("West Ray hit " + hit.transform);
                EnemyController ec = hit.transform.GetComponent<EnemyController>();
                ec.shootable = true;
                shootableEnemies.Add(ec);
            }
        }
    }

    public void UnTagShootableEnemies() {
        for (int i = 0; i < shootableEnemies.Count; i++) {
            EnemyController ec = shootableEnemies[i];
            MoveNode node = ec.Mover.currentNode;
            ec.shootable = false;
            ShootableNode nodeController = node.transform.parent.GetComponentInChildren<ShootableNode>();
            nodeController.currentState = ShootableNode.NodeState.Off;
        }
		shootableEnemies.Clear ();
    }
}
