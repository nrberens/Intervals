using UnityEngine;
using System.Collections;

public class EnemyMover : MonoBehaviour, IMover {

    private EnemyController _ec;
    public float MoveTime;

    // Use this for initialization
    void Start() {
        nodes = GameObject.Find("World").GetComponent<Map>().Nodes;
        _ec = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update() {

    }

    public MoveNode[,] nodes { get; private set; }

    public MoveNode currentNode { get; set; }

    public void Move(Direction direction, int distance) {
        switch (direction) {
            case Direction.Up:
                _ec.Mover.MoveUp(distance);
                break;
            case Direction.Down:
                _ec.Mover.MoveDown(distance);
                break;
            case Direction.Left:
                _ec.Mover.MoveLeft(distance);
                break;
            case Direction.Right:
                _ec.Mover.MoveRight(distance);
                break;
        } 
    }

    public void MoveUp(int distance) {
        Debug.Log(gameObject.name + ": Moving Up");
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int z = currentNode.z;

        if (z >= nodes.GetUpperBound(1)) //bust out early if you're at the top of the map
        {
            Debug.Log("You hit the top! node_id= " + x + " z = " + z);
            _ec.acting = false;
            _ec.EndPhase();
            return;
        }

        transform.forward = Vector3.forward;
        MoveNode targetNode = nodes[x, z + distance];
        //remove from currentNode
        //add to targetNode
        currentNode.RemoveFromNode(gameObject);
        targetNode.AddToNode(gameObject);

        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;

    }

    public void MoveDown(int distance) {
        Debug.Log(gameObject.name + ": Moving Down");
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int z = currentNode.z;

        if (z <= 0) {
            Debug.Log("You hit the bottom! node_id= " + x + " z = " + z);
            _ec.acting = false;
            _ec.EndPhase();
            return;
        }

        transform.forward = Vector3.back;
        MoveNode targetNode = nodes[x, z - distance];
        //remove from currentNode
        //add to targetNode
        currentNode.RemoveFromNode(gameObject);
        targetNode.AddToNode(gameObject);

        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveLeft(int distance) {
        Debug.Log(gameObject.name + ": Moving Left");
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int z = currentNode.z;

        if (x <= 0) {
            Debug.Log("You hit the left! node_id= " + x + " z = " + z);
            _ec.acting = false;
            _ec.EndPhase();
            return;
        }

        transform.forward = Vector3.left;
        MoveNode targetNode = nodes[x - distance, z];
        //remove from currentNode
        //add to targetNode
        currentNode.RemoveFromNode(gameObject);
        targetNode.AddToNode(gameObject);

        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public void MoveRight(int distance) {
        Debug.Log(gameObject.name + ": Moving Right");
        //throw new System.NotImplementedException();
        int x = currentNode.x;
        int z = currentNode.z;

        if (x >= nodes.GetUpperBound(0)) {
            Debug.Log("You hit the right! node_id= " + x + " z = " + z);
            _ec.acting = false;
            _ec.EndPhase();
            return;
        }

        transform.forward = Vector3.right;
        MoveNode targetNode = nodes[x + distance, z];
        //remove from currentNode
        //add to targetNode
        currentNode.RemoveFromNode(gameObject);
        targetNode.AddToNode(gameObject);

        StartCoroutine(MoveToNode(targetNode));
        currentNode = targetNode;
    }

    public IEnumerator MoveToNode(MoveNode targetNode) {
        Vector3 startPos = currentNode.transform.position;
        Vector3 endPos = targetNode.transform.position;
        Vector3 bending = Vector3.up;
        float startTime = Time.time;

        while (Time.time < MoveTime + startTime) {
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / MoveTime);

            //currentPos.x += bending.x * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);
            //currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);
            //currentPos.z += bending.z * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);

            transform.position = currentPos;

            yield return null;
        }

        _ec.acting = false;
        _ec.EndPhase();
    }

    public void DetectCurrentNode() {
    }

}
