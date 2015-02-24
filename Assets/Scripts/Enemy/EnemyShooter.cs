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

    public void Shoot() {
        //TODO instantiate bullet prefab and set direction
        Transform bullet = (Transform) Instantiate(BulletTransform);
        bullet.position = bulletSpawnPoint.transform.position;
		Bullet bulletScript = bullet.GetComponent<Bullet>();
		//HACK all bullets shoot down
        bulletScript.currentNode = _ec.Mover.currentNode;
		bulletScript.Dir = Bullet.Direction.Down;
		bulletScript.bc.Bullets.Add(bulletScript);
    }
}
