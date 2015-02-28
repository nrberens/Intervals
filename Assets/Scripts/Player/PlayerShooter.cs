using UnityEngine;
using System.Collections;

public class PlayerShooter : MonoBehaviour {

    private PlayerController pc;

    public Transform bulletPrefab;
    public Transform weapon;
    public Transform bulletSpawnPoint; 

	// Use this for initialization
	void Start () {
	    pc = GetComponentInParent<PlayerController>();
	    weapon = transform.FindChild("Weapon");
	    bulletSpawnPoint = weapon.FindChild("BulletSpawnPoint");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//On click, get target of mouse click
	public Transform GetTargetOfClick() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast (ray, out hit)) {
			return hit.transform;
		} else return null;
	}

	public bool CheckValidTarget (Transform target) {
		if(target.tag == "Enemy") {
			//Check for correct positioning

			//Check for line of sight
			return true;
		} else return false;

	}

	public void Shoot (Transform target) {
		//spawn bullet
	    Transform newBullet = (Transform) Instantiate(bulletPrefab);
	    newBullet.position = bulletSpawnPoint.position;
	    PlayerBullet newBulletScript = newBullet.GetComponent<PlayerBullet>();
	    newBulletScript.pc = pc;
	    newBulletScript.currentNode = pc.Mover.currentNode;
	    newBulletScript.targetNode = target.GetComponent<EnemyMover>().currentNode;
	    //bullet travels to enemy
	    newBulletScript.TranslateBullet(newBulletScript.targetNode);
	    //enemy takes damage
	    //if enemy health hits zero, enemy dies
	}

}
