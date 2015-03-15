using UnityEngine;
using System.Collections;

public class FallingSpawn : MonoBehaviour {
    public Vector3 startPos;
    public float finalY;
    public Vector3 finalPos;
    public float fallTime;
    public float wiggleTime = 0.25f;
    public float wobbleAmount = 50.0f;
    public float wobbleRotateAmount = 40.0f;

    public IEnumerator FallIntoPlace() {
        float startTime = Time.time;
        startPos = new Vector3(transform.position.x, 8.0f, transform.position.z);
        finalPos = new Vector3(transform.position.x, finalY, transform.position.z);
        transform.position = startPos;
        float fallOffset = Random.Range(0, 20) * 0.05f;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) {
            renderer.enabled = true;
        }

        foreach (Transform t in transform) {
            Renderer[] renderers = t.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers) {
                r.enabled = true;
            }
        }

        while (Time.time < startTime + fallTime || transform.position.y > finalPos.y) {
            Vector3 currentPosition = Vector3.Lerp(startPos, finalPos, (Time.time - startTime) / fallTime - ((transform.position.x+transform.position.z)*.2f));
            //Vector3 currentPosition = Vector3.Lerp(startPos, finalPos, (Time.time - startTime) / fallTime + fallOffset);
            transform.position = currentPosition;

            yield return null;
        }

        startTime = Time.time;
        Vector3 centerPos = finalPos;

        yield return StartCoroutine(ObjectWiggle(centerPos));
    }

    public IEnumerator ObjectWiggle(Vector3 centerPos) {
        float startTime = Time.time;

        //wiggle
        while (Time.time < wiggleTime + startTime) {
            float xWiggleOffset = (Mathf.Sin(Time.time * wobbleAmount) * 0.05f) / 2;// + (Random.Range(0,3)*.02f);
            float zWiggleOffset = (Mathf.Cos(Time.time * wobbleAmount) * 0.05f) / 2;// + (Random.Range(0, 3) * .02f); 
            Vector3 currentPos = new Vector3(centerPos.x + xWiggleOffset, centerPos.y, centerPos.z + zWiggleOffset);
            transform.position = currentPos;
            transform.Rotate(xWiggleOffset * wobbleRotateAmount, 0, zWiggleOffset * wobbleRotateAmount);

            yield return null;
        }

    }
}
