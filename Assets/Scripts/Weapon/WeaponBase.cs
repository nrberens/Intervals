using UnityEngine;
using System.Collections;

public class WeaponBase : MonoBehaviour {

    protected Transform player;
    protected Transform weapon;
    protected Transform bulletSpawnPoint;
    public Transform bulletPrefab;
    protected float fireRate;
    protected float timeSinceShot;
    protected float lastShot;
    protected Vector3 shootPoint;

    protected virtual void Initialize() { }
    protected virtual void WeaponUpdate() { }
    protected virtual void WeaponFixedUpdate() { }
    public virtual void Shoot(Vector3 shootPoint) { }


    // Use this for initialization
    void Start() {
        Initialize();
    }

    // Update is called once per frame
    void Update() {
        WeaponUpdate();
    }

    void FixedUpdate() {
        WeaponFixedUpdate();
    }
}
