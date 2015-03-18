using System;
using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour, IMover {

    public Direction Dir;

    public int distance;
    public float MoveTime;
    public static int totalBullets; //TODO reset at game over
	public bool finished;

    public MoveNode[,] nodes { get; private set; }

    public MoveNode currentNode { get; set; }

    public void Awake() {
        nodes = GameObject.Find("World").GetComponent<Map>().Nodes;
    }

    public void Update() {
        //rotate bullet
        transform.Rotate(0, 0, 45*Time.deltaTime);
    }

    public void BeginPhase() {
		finished = false;
        UpdateBullet();
    }

    public void EndPhase() {
		finished = true;
		Debug.Log("Done updating " + gameObject.name);
    }

    public void UpdateBullet() {
		Debug.Log ("Updating "+ gameObject.name);
        Move(Dir, distance);

        if (gameObject != null) {
            if (currentNode.transform.parent.tag == "Obstacle") {
                DestroyBullet();
            }

        }

    }

    public void DestroyBullet() {
        BulletsController.bc.Bullets.Remove(this);
        currentNode.RemoveFromNode(gameObject);
        Destroy(gameObject);
    }

    public void Move(Direction direction, int distance) {
        switch (direction) {
            case Direction.North:
                MoveUp(distance);
                break;
            case Direction.South:
                MoveDown(distance);
                break;
            case Direction.West:
                MoveLeft(distance);
                break;
            case Direction.East:
                MoveRight(distance);
                break;
        }
    }

    public void MoveUp(int distance) {
        try {
            Debug.Log(gameObject.name + ": Moving North");
            //throw new System.NotImplementedException();
            int x = currentNode.x;
            int z = currentNode.z;

            if (z >= nodes.GetUpperBound(1)) //bust out early if you're at the top of the map
            {
                Debug.Log("You hit the top! node_id= " + x + " z = " + z);
                DestroyBullet();
                EndPhase();
                return;
            }

            MoveNode targetNode = nodes[x, z + distance];
            //remove from currentNode
            //add to targetNode
            currentNode.RemoveFromNode(gameObject);
            targetNode.AddToNode(gameObject);

            StartCoroutine(MoveToNode(targetNode));
            currentNode = targetNode;
        } catch (NullReferenceException e) {
            Console.WriteLine(e);
        }
    }

    public void MoveDown(int distance) {
        try {
            Debug.Log(gameObject.name + ": Moving South");
            //throw new System.NotImplementedException();
            int x = currentNode.x;
            int z = currentNode.z;

            if (z <= 0) {
                Debug.Log("You hit the bottom! node_id= " + x + " z = " + z);
                DestroyBullet();
                EndPhase();
                return;
            }

            MoveNode targetNode = nodes[x, z - distance];
            //remove from currentNode
            //add to targetNode
            currentNode.RemoveFromNode(gameObject);
            targetNode.AddToNode(gameObject);

            StartCoroutine(MoveToNode(targetNode));
            currentNode = targetNode;
        } catch (NullReferenceException e) {
            Console.WriteLine(e);
        }
    }

    public void MoveLeft(int distance) {
        try {
            Debug.Log(gameObject.name + ": Moving West");
            //throw new System.NotImplementedException();
            int x = currentNode.x;
            int z = currentNode.z;

            if (x <= 0) {
                Debug.Log("You hit the left! node_id= " + x + " z = " + z);
                DestroyBullet();
                EndPhase();
                return;
            }

            MoveNode targetNode = nodes[x - distance, z];
            //remove from currentNode
            //add to targetNode
            currentNode.RemoveFromNode(gameObject);
            targetNode.AddToNode(gameObject);

            StartCoroutine(MoveToNode(targetNode));
            currentNode = targetNode;
        } catch (NullReferenceException e) {
            Console.WriteLine(e);
        }
    }

    public void MoveRight(int distance) {
        try {
            Debug.Log(gameObject.name + ": Moving East");
            //throw new System.NotImplementedException();
            int x = currentNode.x;
            int z = currentNode.z;

            if (x >= nodes.GetUpperBound(0)) {
                Debug.Log("You hit the right! node_id= " + x + " z = " + z);
                DestroyBullet();
                EndPhase();
                return;
            }

            MoveNode targetNode = nodes[x + distance, z];
            //remove from currentNode
            //add to targetNode
            currentNode.RemoveFromNode(gameObject);
            targetNode.AddToNode(gameObject);

            StartCoroutine(MoveToNode(targetNode));
            currentNode = targetNode;
        } catch (NullReferenceException e) {
            Console.WriteLine(e);
        }
    }

    public IEnumerator MoveToNode(MoveNode targetNode) {
        Vector3 startPos = new Vector3(currentNode.transform.position.x, 1.085f, currentNode.transform.position.z);
        Vector3 endPos = new Vector3(targetNode.transform.position.x, 1.085f, targetNode.transform.position.z);
        float startTime = Time.time;

        while (Time.time < MoveTime + startTime) {
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / MoveTime);

            transform.position = currentPos;

            yield return null;
        }

        EndPhase();
    }


    public void DetectCurrentNode() {
    }

}
