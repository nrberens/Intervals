using UnityEngine;
using System.Collections;

public class EnemyMover : MonoBehaviour, IMover {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public MoveNode[,] nodes { get; private set; }

    public MoveNode currentNode { get; set; }

    public bool moving { get; set; }

    public void MoveForward(int distance) {
    }

    public void MoveBackward(int distance) {
    }

    public void MoveLeft(int distance) {
    }

    public void MoveRight(int distance) {
    }

    public void DetectCurrentNode() {
    }

    public IEnumerator MoveToNode(MoveNode targetNode) {
    }
}
