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

    public void Shoot(Bullet.Direction direction = Bullet.Direction.Down) {
        // Look in direction
        switch(direction) {
            case Bullet.Direction.Down:
                transform.forward = Vector3.back;
                break;
            case Bullet.Direction.Up:
                transform.forward = Vector3.forward;
                break;
            case Bullet.Direction.Left:
                transform.forward = Vector3.left;
                break;
            case Bullet.Direction.Right:
                transform.forward = Vector3.right;
                break;
        }

        // Instantiate bullet prefab and set direction
        Transform bullet = (Transform) Instantiate(BulletTransform);
        bullet.position = bulletSpawnPoint.transform.position;
		Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.currentNode = _ec.Mover.currentNode;
        bulletScript.MoveDown(1);
        bulletScript.Dir = direction;
		bulletScript.bc.Bullets.Add(bulletScript);
    }
}
