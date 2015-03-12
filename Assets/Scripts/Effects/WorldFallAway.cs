using UnityEngine;
using System.Collections;

public class WorldFallAway : MonoBehaviour {

    public Transform world;
    public EnemiesController ec;
    public BulletsController bc;
    public Transform player;
    public float finalY;
    public float fallTime;

	// Use this for initialization
	void Start () {
	    world = GameObject.Find("World").transform;
	    ec = FindObjectOfType<EnemiesController>();
	    bc = FindObjectOfType<BulletsController>();
	    player = GameObject.FindGameObjectWithTag("Player").transform;
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
        foreach (EnemyBullet b in bc.Bullets) {
            Transform t = b.transform;
            StartCoroutine(ObjectFallAway(t));
        }
        //enemies fall away
        foreach (EnemyController e in ec.Enemies) {
            Transform t = e.transform;
            StartCoroutine(ObjectFallAway(t));
        }
        //player falls away?
        StartCoroutine(ObjectFallAway(player));
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
