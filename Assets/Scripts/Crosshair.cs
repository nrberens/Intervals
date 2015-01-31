using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

    public Vector3 mousePos;
    void Start() {
    }

    void Update() {
    }

    public static Vector3 GetCrosshairInWorld() {
        //Get the mouse position on the screen (in 2D space)
        //Cast a ray from the screen into the 3D worldspace
        //Where the ray hits the ground, set a point for the bullet to shoot towards
        //Set the bullet to "look at" the shootPoint
        //So that when we translate it forward in Update, it moves in that direction constantly
        Vector3 inputPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        //RaycastHit hit;
        Vector3 shootPoint;
        Plane floor = new Plane(Vector3.up, new Vector3(0, 1, 0));
        float dist;
        Vector3 hit;
        if (floor.Raycast(ray, out dist)) {
            hit = ray.GetPoint(dist);
            shootPoint = hit;// +new Vector3(0, 1f, 0);
            Debug.DrawLine(ray.origin, shootPoint, Color.yellow, 1f);
        }
        else {
            shootPoint = ray.GetPoint(10f);
        }
        return shootPoint;
    }
}
