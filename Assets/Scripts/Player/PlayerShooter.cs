using UnityEngine;
using System.Collections;

public class PlayerShooter : MonoBehaviour {

    private PlayerController pc;

    public Transform bulletPrefab;
    public Transform weapon;
    public Transform bulletSpawnPoint;

    // Use this for initialization
    void Start() {
        pc = GetComponentInParent<PlayerController>();
        bulletSpawnPoint = transform.FindChild("Weapon/BulletSpawnPoint");
    }

    // Update is called once per frame
    void Update() {

    }

    //On click, get target of mouse click
    public Transform GetTargetOfClick() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            return hit.transform;
        } else return null;
    }

    public bool CheckValidTarget(Transform target) {
        if (target.tag == "Enemy") {
            MoveNode targetNode = target.GetComponent<EnemyMover>().currentNode;
            //Check for correct positioning
            bool validPosition = CheckValidPosition(targetNode);
            //Check for line of sight
            //TODO implement LOS -- use raycast?
            bool validLOS = CheckValidLOS(target);

            if (validPosition && validLOS) return true; 
        }
        return false;
    }

    public bool CheckValidPosition(MoveNode targetNode) {
        MoveNode playerNode = pc.Mover.currentNode;

        //if player is in same row or column
        if (playerNode.x == targetNode.x || playerNode.z == targetNode.z) return true;
        else return false;
    }

    public bool CheckValidLOS(Transform target) {
        //TODO LOS checking is spotty -- need layer mask?
        Ray ray = new Ray(transform.position, target.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform == target) return true;
        }
        //return false;
        //HACK for now just return true
        return true;
    }

    public void BeginShot() {
        Transform target = GetTargetOfClick();
        // TODO player can only shoot target where playerx == enemyx or playery == enemyy
        // TODO player can only shoot target with line of sight
        if (target != null && pc.Shooter.CheckValidTarget(target)) {
            pc.Input.allowInput = false;
            //shot is valid, shoot target
            Debug.Log("Shooting " + target);
            pc.acting = false;
            pc.Shooter.Shoot(target);
            return;
        }
    }

    public void Shoot(Transform target) {
        //spawn bullet
        Transform newBullet = (Transform) Instantiate(bulletPrefab);
        newBullet.position = new Vector3(bulletSpawnPoint.position.x, 0, bulletSpawnPoint.position.z);
        newBullet.rotation = transform.rotation;
        PlayerBullet newBulletScript = newBullet.GetComponent<PlayerBullet>();
        newBulletScript.pc = pc;
        newBulletScript.spawnPoint = bulletSpawnPoint;
        newBulletScript.currentNode = pc.Mover.currentNode;
        newBulletScript.targetNode = target.GetComponent<EnemyMover>().currentNode;
        //bullet travels to enemy
        newBulletScript.TranslateBullet(newBulletScript.targetNode);
		//enemy dies
		Debug.Log (target + " killed!");
		target.GetComponent<EnemyController>().TakeDamage();
    }

}
