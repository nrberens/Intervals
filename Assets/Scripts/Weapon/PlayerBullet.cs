using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

    public PlayerController pc;

    public Transform spawnPoint;
    public MoveNode currentNode { get; set; }
    public MoveNode targetNode { get; set; }
    public float MoveTime;
    
    public void Start() {
    }

    public void TranslateBullet(MoveNode targetNode) {
        StartCoroutine(MoveToTarget(targetNode));
    }

    public IEnumerator MoveToTarget(MoveNode targetNode) {
        Vector3 startPos = new Vector3(currentNode.transform.position.x, 0.05f, currentNode.transform.position.z);
        Vector3 endPos = new Vector3(targetNode.transform.position.x, 0.05f, targetNode.transform.position.z);
        float startTime = Time.time;

        while (Time.time < MoveTime + startTime) {
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / MoveTime);

            transform.position = currentPos;

            yield return null;
        }

        EndPhase();
    }

    public void EndPhase() {
        Debug.Log("Bullet reached target");
        pc.acting = false;
		Destroy(gameObject);
    }

}
