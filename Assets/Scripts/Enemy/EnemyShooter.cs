using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour {

    private EnemyController _ec;
    private Transform bulletSpawnPoint;
    public Transform BulletTransform;

	// Use this for initialization
	void Start () {
	    _ec = GetComponentInParent<EnemyController>();
	    bulletSpawnPoint = transform.FindChild("Weapon/BulletSpawnPoint");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shoot(EnemyBullet.Direction direction = EnemyBullet.Direction.Down) {
        // Look in direction
        switch(direction) {
            case EnemyBullet.Direction.Down:
                transform.forward = Vector3.back;
                break;
            case EnemyBullet.Direction.Up:
                transform.forward = Vector3.forward;
                break;
            case EnemyBullet.Direction.Left:
                transform.forward = Vector3.left;
                break;
            case EnemyBullet.Direction.Right:
                transform.forward = Vector3.right;
                break;
        }

        // Instantiate EnemyBullet prefab and set direction
        Transform bullet = (Transform) Instantiate(BulletTransform);
        bullet.position = bulletSpawnPoint.transform.position;
        bullet.rotation = transform.rotation;
		EnemyBullet enemyBulletScript = bullet.GetComponent<EnemyBullet>();
        enemyBulletScript.currentNode = _ec.Mover.currentNode;
        enemyBulletScript.MoveDown(1);
        enemyBulletScript.Dir = direction;
		enemyBulletScript.bc.Bullets.Add(enemyBulletScript);
    }
}
