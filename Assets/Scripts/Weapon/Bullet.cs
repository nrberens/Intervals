using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed = 20f;
    public Crosshair crosshair;
    //public Transform player;
    public bool shot;
    public float bulletLife = 8f;
    float shootAngle;
    float startTime;

    public int damage;

    void Start() {
        shot = true;
        startTime = Time.time;
        damage = 10;
    }

    // Update is called once per frame
    void Update() {
        //Only run this code after the bullet has been shot
        //Check how long bullet has existed: If bullet has lived longer than bulletLife (set above), destroy it.
        //Otherwise, keep moving it forward.
        if (shot) {
            if (Time.timeSinceLevelLoad - startTime >= bulletLife) Destroy(this.gameObject);
            //TODO Vector math isn't right -- bullets shoot toward unit vector points rather than in directions
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //transform.rigidbody.AddRelativeForce(transform.forward * speed);
        }
    }

    //Shoot() sets up the bullet's direction
    public void ShootAtPoint(Vector3 shootPoint) {
        //print(shootPoint);
        transform.LookAt(shootPoint);
        shot = true;
    }

    public void ShootInDirection(Vector3 direction) {
        transform.rotation = Quaternion.LookRotation(direction); ;
        shot = true;
    }

    //Gets called whenever the bullet hits another collider object
    void OnCollisionEnter(Collision collision) {
        Destroy(this.gameObject);
    }
}
