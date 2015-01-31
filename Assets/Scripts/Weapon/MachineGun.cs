using UnityEngine;
using System.Collections;

public class MachineGun : WeaponBase {

    /* Inherited from Weapon

    protected Transform player;
    protected Transform weapon;
    public Transform bulletPrefab;
    protected float fireRate;
    protected float timeSinceShot;
    protected float lastShot;
    protected Vector3 shootPoint;

     */

    protected override void Initialize() {
        player = transform.parent;
        weapon = transform;
        bulletSpawnPoint = transform.GetChild(0).transform;
        fireRate = 0.2f;
        lastShot = Time.time;
        timeSinceShot = 0.0f;
    }

    protected override void WeaponUpdate() {
        timeSinceShot = Time.time - lastShot;
    }

    protected override void WeaponFixedUpdate() { }

    public override void Shoot(Vector3 shootPoint) {
        //if enough time has passed since their last shot
        if (timeSinceShot >= fireRate) {
            Transform bullet = (Transform)GameObject.Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Bullet bulletController = bullet.GetComponent<Bullet>();
            //bulletController.drone = transform;
            lastShot = Time.time;
            bulletController.ShootAtPoint(shootPoint);
        }

    }

}
