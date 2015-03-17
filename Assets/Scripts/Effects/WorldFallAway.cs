using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class WorldFallAway : MonoBehaviour {

    public Transform world;
    public Transform player;
    public float finalY;
    public float fallTime;
    public bool fallingComplete;

	// Use this for initialization
	void Start () {
	    world = GameObject.Find("World").transform;
	    player = GameObject.FindGameObjectWithTag("Player").transform;
	    fallingComplete = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator ManageFallAwayTiming() {
        Debug.Log("World falling away");
        //world falls away
        foreach (Transform t in world) {
            StartCoroutine(ObjectFallAway(t));
        }
        yield return null;
        //bullets fall away
        foreach (EnemyBullet b in BulletsController.bc.Bullets) {
            Transform t = b.transform;
            StartCoroutine(ObjectFallAway(t));
        }
        //enemies fall away
        foreach (EnemyController e in EnemiesController.ec.Enemies) {
            Transform t = e.transform;
            StartCoroutine(ObjectFallAway(t));
        }
        //player falls away?
        StartCoroutine(ObjectFallAway(player));
        yield return new WaitForSeconds(1.5f);
    }

    public IEnumerator ObjectFallAway(Transform t) {
        float startTime = Time.time;
        Vector3 startPos = t.position;
        Vector3 finalPos = new Vector3(t.position.x, finalY, t.position.z);
        transform.position = startPos;
        float fallOffset = Random.Range(0, 20) * 0.05f;

        while (Time.time < startTime + fallTime || t.position.y > finalPos.y) {
            Vector3 currentPosition = Vector3.Lerp(startPos, finalPos, (Time.time - startTime) / fallTime + fallOffset);
            t.position = currentPosition;

            yield return null;
        }

    }
}
