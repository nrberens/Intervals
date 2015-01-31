using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private float moveX, moveZ;
    private float moveSpeed = 4.0f;

    public Transform weapon;    //currently equipped weapon

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 crosshairPos = Crosshair.GetCrosshairInWorld();

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime);

        Transform bulletSpawnPoint = weapon.Find("BulletPoint");
        transform.LookAt(crosshairPos);
        if (Input.GetButton("Fire1")) {
            weapon.SendMessage("Shoot", crosshairPos);
        } 
	}
}
