using UnityEngine;
using System.Collections;

public class IsoFollowCamera : MonoBehaviour {

	public Transform player;
	public Transform worldPivot;
    public float sensitivity;
	float x, z;

    void Update()
    {
        //right click + drag to rotate camera
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");

            //Vector3 currentAngle = transform.rotation.eulerAngles;
            //transform.rotation = Quaternion.Euler(new Vector3(currentAngle.x, currentAngle.y+(mouseX * sensitivity), currentAngle.z));
			transform.RotateAround (worldPivot.position, Vector3.up, mouseX*sensitivity);
        }
    }

	void LateUpdate() {
		//x = player.transform.position.x;
	    //z = player.transform.position.z;

		//transform.position = new Vector3(x, 0, z);
	}
}
