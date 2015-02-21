using UnityEngine;
using System.Collections;

// TODO Enemy takes multiple turns? Why?

public class EnemyMover : MonoBehaviour, IMover {

    private EnemyController ec;
    public float moveTime;

    // Use this for initialization
    void Start() {
        nodes = GameObject.Find("World").GetComponent<World>().nodes;
        ec = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update() {

    }

    public MoveNode[,] nodes { get; private set; }

    public MoveNode currentNode { get; set; }

    public bool moving { get; set; }

    public void MoveUp(int distance) {
        //throw new System.NotImplementedException();
        int node_id = currentNode.x;
        int block_id = currentNode.z;

        if (block_id >= nodes.GetUpperBound(1)) //bust out early if you're at the top of the map
        {
            Debug.Log("You hit the top! node_id= " + node_id + " z = " + block_id);
            ec.EndPhase();
            return;
        }

        MoveNode targetNode = nodes[node_id, block_id + distance];
        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveDown(int distance) {
        //throw new System.NotImplementedException();
        int node_id = currentNode.x;
        int block_id = currentNode.z;

        if (block_id <= 0) {
            Debug.Log("You hit the bottom! node_id= " + node_id + " z = " + block_id);
            ec.EndPhase();
            return;
        }

        MoveNode targetNode = nodes[node_id, block_id - distance];
        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveLeft(int distance) {
        //throw new System.NotImplementedException();
        int node_id = currentNode.x;
        int block_id = currentNode.z;

        if (node_id <= 0) {
            Debug.Log("You hit the left! node_id= " + node_id + " z = " + block_id);
            ec.EndPhase();
            return;
        }

        MoveNode targetNode = nodes[node_id - distance, block_id];
        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveRight(int distance) {
        //throw new System.NotImplementedException();
        int node_id = currentNode.x;
        int block_id = currentNode.z;

        if (node_id >= nodes.GetUpperBound(0)) {
            Debug.Log("You hit the right! node_id= " + node_id + " z = " + block_id);
            ec.EndPhase();
            return;
        }

        MoveNode targetNode = nodes[node_id + distance, block_id];
        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public IEnumerator MoveToNode(MoveNode targetNode) {
        Vector3 startPos = currentNode.transform.position;
        Vector3 endPos = targetNode.transform.position;
        Vector3 bending = Vector3.up;
        float startTime = Time.time;

        while (Time.time < moveTime + startTime) {
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / moveTime);

            currentPos.x += bending.x * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / moveTime) * Mathf.PI);
            currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / moveTime) * Mathf.PI);
            currentPos.z += bending.z * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / moveTime) * Mathf.PI);

            transform.position = currentPos;

            yield return null;
        }

        ec.EndPhase();
    }

    public void DetectCurrentNode() {
    }

}
