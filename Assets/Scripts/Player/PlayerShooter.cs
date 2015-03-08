using UnityEngine;
using System.Collections;

public class PlayerShooter : MonoBehaviour {

    private PlayerController pc;

    public Transform bulletPrefab;
    public Transform weapon;
    public Transform bulletSpawnPoint;
    public Transform muzzleFlash;
    public Light muzzleFlashLight;

    // Use this for initialization
    void Start() {
        pc = GetComponentInParent<PlayerController>();
        bulletSpawnPoint = transform.FindChild("BulletSpawnPoint");
        muzzleFlashLight = muzzleFlash.GetComponent<Light>();
        muzzleFlashLight.enabled = false;
    }

    // Update is called once per frame
    void Update() {

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
        Ray ray = new Ray(transform.position, (target.position - transform.position));
        RaycastHit hit;

        //Check against colliders EXCEPT player layer (layer 8)
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) { 
            Debug.Log("Raycast hit " + hit.transform);
            if (hit.transform.tag == "Obstacle") {
                Debug.DrawRay(ray.origin, ray.direction*100, Color.red, 5.0f);
                return false;
            }
            
            Debug.DrawRay(ray.origin, ray.direction*100, Color.blue, 5.0f);
        }
        return true;
        //HACK for now just return true
        //return true;
    }

    public void BeginShot() {
        Transform target = GetTargetOfClick();
        // TODO player can only shoot target with line of sight
        if (target != null && pc.Shooter.CheckValidTarget(target)) {
            pc.Input.allowInput = false;
            //shot is valid, shoot target
            transform.LookAt(target);
            pc.acting = false;
            pc.Shooter.Shoot(target);
        }
    }

    public void Shoot(Transform target) {
        //spawn bullet
        StartCoroutine(pc.Mover.ObjectWiggle(transform.position));
        Transform newBullet = (Transform) Instantiate(bulletPrefab);
        newBullet.position = bulletSpawnPoint.position;
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
		target.GetComponent<EnemyController>().TakeDamage(newBullet);
        StartCoroutine(MuzzleFlash());
    }

    public IEnumerator MuzzleFlash() {
        float startTime = Time.time;
        float flashTime = 0.1f;

        while (Time.time < flashTime + startTime) {
            muzzleFlashLight.enabled = true;

            yield return null;
        }
        muzzleFlashLight.enabled = false;
    }

}
