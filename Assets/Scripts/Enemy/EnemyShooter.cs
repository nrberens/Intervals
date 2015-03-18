using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour {

    private EnemyController _ec;
    public Transform bulletSpawnPoint;
    public Transform BulletTransform;
    public Transform muzzleFlash;
    public Light muzzleFlashLight;

    private int mapLength, mapWidth;
    public float rotateTime;

    // Use this for initialization
    void Start() {
        _ec = GetComponentInParent<EnemyController>();
        bulletSpawnPoint = transform.FindChild("BulletSpawnPoint");
        muzzleFlashLight = muzzleFlash.GetComponent<Light>();
        muzzleFlashLight.enabled = false;

        Map map = FindObjectOfType<Map>();
        mapLength = map.mapLength;
        mapWidth = map.mapWidth;
    }

    // Update is called once per frame
    void Update() {

    }

    public IEnumerator Shoot(Direction direction) {
        //Check for node in that direction
        if (CheckForValidShot(direction)) {
			if(CheckForMeleeRange()) {
	            // Look in direction
	            switch (direction) {
	                case Direction.South:
	                    Debug.Log("Current rotation: " + transform.rotation.eulerAngles + " Target Rotation: " + Vector3.back);
	                    yield return StartCoroutine(RotateToTarget(Vector3.back));
	                    break;
	                case Direction.North:
	                    Debug.Log("Current rotation: " + transform.rotation.eulerAngles + " Target Rotation: " + Vector3.forward);
	                    yield return StartCoroutine(RotateToTarget(Vector3.forward));
	                    break;
	                case Direction.West:
	                    Debug.Log("Current rotation: " + transform.rotation.eulerAngles + " Target Rotation: " + Vector3.left);
	                    yield return StartCoroutine(RotateToTarget(Vector3.left));
	                    break;
	                case Direction.East:
	                    Debug.Log("Current rotation: " + transform.rotation.eulerAngles + " Target Rotation: " + Vector3.right);
	                    yield return StartCoroutine(RotateToTarget(Vector3.right));
	                    break;
	            }
				_ec.lowReadyMesh.SetActive(false);
				_ec.meleeMesh.SetActive(true);
				yield return new WaitForSeconds(0.2f);
				PlayerController.pc.GameOver(transform);
				_ec.meleeMesh.SetActive(false);
				_ec.lowReadyMesh.SetActive(true);
			} else {
	            _ec.lowReadyMesh.SetActive(false);
	            _ec.firingMesh.SetActive(true);
	            // Look in direction
	            switch (direction) {
	                case Direction.South:
	                    Debug.Log("Current rotation: " + transform.rotation.eulerAngles + " Target Rotation: " + Vector3.back);
	                    yield return StartCoroutine(RotateToTarget(Vector3.back));
	                    break;
	                case Direction.North:
	                    Debug.Log("Current rotation: " + transform.rotation.eulerAngles + " Target Rotation: " + Vector3.forward);
	                    yield return StartCoroutine(RotateToTarget(Vector3.forward));
	                    break;
	                case Direction.West:
	                    Debug.Log("Current rotation: " + transform.rotation.eulerAngles + " Target Rotation: " + Vector3.left);
	                    yield return StartCoroutine(RotateToTarget(Vector3.left));
	                    break;
	                case Direction.East:
	                    Debug.Log("Current rotation: " + transform.rotation.eulerAngles + " Target Rotation: " + Vector3.right);
	                    yield return StartCoroutine(RotateToTarget(Vector3.right));
	                    break;
	            }

	            StartCoroutine(_ec.Mover.ObjectWiggle(transform.position));

	            // Instantiate EnemyBullet prefab and set direction
	            Transform bullet = (Transform)Instantiate(BulletTransform);
	            EnemyBullet enemyBulletScript = bullet.GetComponent<EnemyBullet>();
	            EnemyBullet.totalBullets++;
	            BulletsController.bc.Bullets.Add(enemyBulletScript);
	            bullet.name = "EnemyBullet " + EnemyBullet.totalBullets;
	            enemyBulletScript.currentNode = _ec.Mover.currentNode;
	            enemyBulletScript.currentNode.AddToNode(bullet.gameObject);
	            enemyBulletScript.Dir = direction;
	            bullet.position = bulletSpawnPoint.transform.position;
	            bullet.rotation = transform.rotation;
	            enemyBulletScript.UpdateBullet();
	            yield return StartCoroutine(MuzzleFlash());
	            yield return new WaitForSeconds(0.5f);
	            _ec.firingMesh.SetActive(false);
	            _ec.lowReadyMesh.SetActive(true);
	        }
		}
		_ec.EndPhase();
    }

    public bool CheckForValidShot(Direction direction) {
        MoveNode node = _ec.Mover.currentNode;
        bool valid = false;

        switch (direction) {
            case Direction.South:
                if (node.z > 0) valid = true;
                break;
            case Direction.North:
                if (node.z < mapLength) valid = true;
                break;
            case Direction.West:
                if (node.x > 0) valid = true;
                break;
            case Direction.East:
                if (node.x < mapWidth) valid = true;
                break;
            default:
                valid = false;
                Debug.Log(gameObject + ": Invalid direction!");
                break;
        }

        return valid;
    }

	public bool CheckForMeleeRange() {
		int xDistance = Mathf.Abs (PlayerController.pc.Mover.currentNode.x - _ec.Mover.currentNode.x);
		int zDistance = Mathf.Abs (PlayerController.pc.Mover.currentNode.z - _ec.Mover.currentNode.z);
		
		if(xDistance == 1 && zDistance == 0) return true;
		else if(xDistance == 0 && zDistance == 1) return true;
		else return false;
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

    public IEnumerator RotateToTarget(Vector3 directionVector) {
        float startTime = Time.time;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(directionVector);
        Debug.DrawRay(transform.position, directionVector, Color.red, 3.0f);

        while (Time.time < rotateTime + startTime) {
            transform.rotation = Quaternion.Slerp(startRot, endRot, (Time.time - startTime) / rotateTime);
            yield return null;
        }
		transform.rotation = endRot;
    }
}
