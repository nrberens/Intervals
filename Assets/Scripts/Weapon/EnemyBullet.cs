using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour, IMover{

	public BulletsController bc;

	public Direction Dir { get; set; }
	public int speed;
    public float MoveTime;

	public MoveNode[,] nodes { get; private set; }

    public MoveNode currentNode { get; set; }

	public void Awake() {
        nodes = GameObject.Find("World").GetComponent<Map>().Nodes;
		bc = FindObjectOfType<BulletsController>();
	}

   	public void UpdateBullet() {
   		switch(Dir) {
   			case Direction.Up:
   			MoveUp(speed);
   			break;
   			case Direction.Down:
   			MoveDown(speed);
   			break;
   			case Direction.Left:
   			MoveLeft(speed);
   			break;
   			case Direction.Right:
   			MoveRight(speed);
   			break;
   		}

		if(gameObject != null) {
		    foreach (GameObject obj in currentNode.objectsOnNode) {
		        if (obj.tag == "Player") {
		            obj.GetComponent<PlayerController>().GameOver();
		        }
		    }
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
    }

    public void MoveDown(int distance) {
        Debug.Log(gameObject.name + ": Moving Down");
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
    }

    public void MoveLeft(int distance) {
        Debug.Log(gameObject.name + ": Moving Left");
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
    }

    public void MoveRight(int distance) {
        Debug.Log(gameObject.name + ": Moving Right");
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
    }

    public IEnumerator MoveToNode(MoveNode targetNode) {
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

	public void BeginPhase() {
		UpdateBullet ();
	}

	public void EndPhase() {
	}

    public void DestroyBullet() {
        bc.Bullets.Remove(this);
        currentNode.RemoveFromNode(gameObject);
        Destroy(gameObject);
    }

    public void DetectCurrentNode() {
    }

}
