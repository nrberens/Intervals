using UnityEngine;
using System.Collections;

public class EnemyMover : MonoBehaviour, IMover {

    public float moveTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public MoveNode[,] nodes { get; private set; }

    public MoveNode currentNode { get; set; }

    public bool moving { get; set; }

    public void MoveUp(int distance) {
    }

    public void MoveDown(int distance) {
    }

    public void MoveLeft(int distance) {
    }

    public void MoveRight(int distance) {
    }

    public void DetectCurrentNode() {
    }

    public IEnumerator MoveToNode(MoveNode targetNode) {
        Vector3 startPos = currentNode.transform.position;
        Vector3 endPos = targetNode.transform.position;
        Vector3 bending = Vector3.up;
        float startTime = Time.time;

        while (Time.time < moveTime + startTime)
        {
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, (Time.time - startTime)/moveTime);

            currentPos.x += bending.x * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / moveTime) * Mathf.PI);
            currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / moveTime) * Mathf.PI);
            currentPos.z += bending.z * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / moveTime) * Mathf.PI);

            transform.position = currentPos;

            yield return null;
        }

        moving = false;
    }
}
