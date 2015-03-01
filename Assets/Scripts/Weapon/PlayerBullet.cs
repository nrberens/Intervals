using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

    public PlayerController pc;

    public MoveNode currentNode { get; set; }
    public MoveNode targetNode { get; set; }
    public float MoveTime;
    
    public void Start() {
    }

    public void TranslateBullet(MoveNode targetNode) {
        StartCoroutine(MoveToNode(targetNode));
    }

    public IEnumerator MoveToNode(MoveNode targetNode) {
        Vector3 startPos = currentNode.transform.position;
        Vector3 endPos = targetNode.transform.position;
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
