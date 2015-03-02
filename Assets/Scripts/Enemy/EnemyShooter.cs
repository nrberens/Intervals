using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour {

    private EnemyController _ec;
    private Transform bulletSpawnPoint;
    public Transform BulletTransform;

    private int mapLength, mapWidth;

	// Use this for initialization
	void Start () {
	    _ec = GetComponentInParent<EnemyController>();
	    bulletSpawnPoint = transform.FindChild("Weapon/BulletSpawnPoint");

	    Map map = FindObjectOfType<Map>();
	    mapLength = map.mapLength;
	    mapWidth = map.mapWidth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shoot(Direction direction) {
        //TODO Check for valid shot
        //Check for node in that direction
        if (CheckForValidShot(direction)) {
            // Look in direction
            switch (direction) {
                case Direction.Down:
                    transform.forward = Vector3.back;
                    break;
                case Direction.Up:
                    transform.forward = Vector3.forward;
                    break;
                case Direction.Left:
                    transform.forward = Vector3.left;
                    break;
                case Direction.Right:
                    transform.forward = Vector3.right;
                    break;
            }

            // Instantiate EnemyBullet prefab and set direction
            Transform bullet = (Transform) Instantiate(BulletTransform);
            EnemyBullet enemyBulletScript = bullet.GetComponent<EnemyBullet>();
            EnemyBullet.totalBullets++;
            bullet.name = "EnemyBullet " + EnemyBullet.totalBullets;
            enemyBulletScript.currentNode = _ec.Mover.currentNode;
            //TODO move bullet one square in direction
            enemyBulletScript.Dir = direction;
            bullet.position = bulletSpawnPoint.transform.position;
            bullet.rotation = transform.rotation;
            enemyBulletScript.bc.Bullets.Add(enemyBulletScript);
            enemyBulletScript.currentNode.AddToNode(bullet.gameObject);
            enemyBulletScript.UpdateBullet();
        }
    }

    public bool CheckForValidShot(Direction direction) {
        MoveNode node = _ec.Mover.currentNode;
        bool valid = false;

        switch (direction) {
            case Direction.Down:
                if (node.z > 0) valid = true;
                break;
            case Direction.Up:
                if (node.z < mapLength) valid = true;
                break;
            case Direction.Left:
                if (node.x > 0) valid = true;
                break;
            case Direction.Right:
                if (node.x < mapWidth) valid = true;
                break;
            default:
                valid = false;
                Debug.Log(gameObject + ": Invalid direction!");
                break;
        }

        return valid;
    }
}
