using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class EnemyMover : MonoBehaviour, IMover
{

	private EnemyController _ec;
	public float MoveTime;
	public float wobbleAmount;
	public float wobbleRotateAmount;
	public float wiggleTime;
	public float rotateTime;

	// Use this for initialization
	void Start ()
	{
		nodes = GameObject.Find ("World").GetComponent<Map> ().Nodes;
		_ec = GetComponentInParent<EnemyController> ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public MoveNode[,] nodes { get; private set; }

	public MoveNode currentNode { get; set; }

	public void Move (Direction direction, int distance)
	{
		switch (direction) {
		case Direction.North:
			_ec.Mover.MoveUp (distance);
			break;
		case Direction.South:
			_ec.Mover.MoveDown (distance);
			break;
		case Direction.West:
			_ec.Mover.MoveLeft (distance);
			break;
		case Direction.East:
			_ec.Mover.MoveRight (distance);
			break;
		}
	}

	public void MoveUp (int distance)
	{
		try {
			Debug.Log (gameObject.name + ": Moving North");
			//throw new System.NotImplementedException();
			int x = currentNode.x;
			int z = currentNode.z;

			if (z >= nodes.GetUpperBound (1)) { //bust out early if you're at the top of the map
				Debug.Log ("You hit the top! node_id= " + x + " z = " + z);
				_ec.acting = false;
				_ec.EndPhase ();
				return;
			}

			MoveNode targetNode = nodes [x, z + distance];
			//remove from currentNode
			//add to targetNode
			currentNode.RemoveFromNode (gameObject);
			targetNode.AddToNode (gameObject);

			StartCoroutine (MoveToNode (targetNode));
			currentNode = targetNode;
		} catch (NullReferenceException e) {
			Console.WriteLine (e);
		}
	}

	public void MoveDown (int distance)
	{
		try {
			Debug.Log (gameObject.name + ": Moving South");
			//throw new System.NotImplementedException();
			int x = currentNode.x;
			int z = currentNode.z;

			if (z <= 0) {
				Debug.Log ("You hit the bottom! node_id= " + x + " z = " + z);
				_ec.acting = false;
				_ec.EndPhase ();
				return;
			}

			MoveNode targetNode = nodes [x, z - distance];
			//remove from currentNode
			//add to targetNode
			currentNode.RemoveFromNode (gameObject);
			targetNode.AddToNode (gameObject);

			StartCoroutine (MoveToNode (targetNode));
			currentNode = targetNode;
		} catch (NullReferenceException e) {
			Console.WriteLine (e);
		}
	}

	public void MoveLeft (int distance)
	{
		try {
			Debug.Log (gameObject.name + ": Moving West");
			//throw new System.NotImplementedException();
			int x = currentNode.x;
			int z = currentNode.z;

			if (x <= 0) {
				Debug.Log ("You hit the left! node_id= " + x + " z = " + z);
				_ec.acting = false;
				_ec.EndPhase ();
				return;
			}

			MoveNode targetNode = nodes [x - distance, z];
			//remove from currentNode
			//add to targetNode
			currentNode.RemoveFromNode (gameObject);
			targetNode.AddToNode (gameObject);

			StartCoroutine (MoveToNode (targetNode));
			currentNode = targetNode;
		} catch (NullReferenceException e) {
			Console.WriteLine (e);
		}
	}

	public void MoveRight (int distance)
	{
		try {
			Debug.Log (gameObject.name + ": Moving East");
			//throw new System.NotImplementedException();
			int x = currentNode.x;
			int z = currentNode.z;

			if (x >= nodes.GetUpperBound (0)) {
				Debug.Log ("You hit the right! node_id= " + x + " z = " + z);
				_ec.acting = false;
				_ec.EndPhase ();
				return;
			}

			MoveNode targetNode = nodes [x + distance, z];
			//remove from currentNode
			//add to targetNode
			currentNode.RemoveFromNode (gameObject);
			targetNode.AddToNode (gameObject);

			StartCoroutine (MoveToNode (targetNode));
			currentNode = targetNode;
		} catch (NullReferenceException e) {
			Console.WriteLine (e);
		}
	}

	public IEnumerator MoveToNode (MoveNode targetNode)
	{
		Vector3 startPos = currentNode.transform.position;
		Vector3 endPos = targetNode.transform.position;
		Quaternion startRot = transform.rotation;
		Quaternion endRot = Quaternion.LookRotation (targetNode.transform.position - transform.position);
		Debug.DrawRay (transform.position, targetNode.transform.position - transform.position, Color.black, 3.0f);
		Vector3 bending = Vector3.up;
		float startTime = Time.time;

		//Rotate Object before moving
		while (Time.time < rotateTime + startTime) {
			//TODO object immediately snaps to final position?
			transform.rotation = Quaternion.Slerp (startRot, endRot, (Time.time - startTime) / rotateTime);
			yield return null;
		}

		startTime = Time.time;

		//move
		while (Time.time < MoveTime + startTime) {
			Vector3 currentPos = Vector3.Lerp (startPos, endPos, (Time.time - startTime) / MoveTime);

			//currentPos.x += bending.x * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);
			//currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);
			//currentPos.z += bending.z * Mathf.Sin(Mathf.Clamp01((Time.time - startTime) / MoveTime) * Mathf.PI);

			transform.position = currentPos;

			yield return null;
		}

		Vector3 centerPos = endPos;
		startTime = Time.time;

		//wiggle

		StartCoroutine (ObjectWiggle (endPos));

		transform.position = endPos;
		//transform.rotation = startRot;

		_ec.acting = false;
		_ec.EndPhase ();
	}

	public IEnumerator ObjectWiggle (Vector3 centerPos)
	{
		float startTime = Time.time;

		//wiggle
		while (Time.time < wiggleTime + startTime) {
			float xWiggleOffset = (Mathf.Sin (Time.time * wobbleAmount) * 0.05f) / 2;// + (Random.Range(0,3)*.02f);
			float zWiggleOffset = (Mathf.Cos (Time.time * wobbleAmount) * 0.05f) / 2;// + (Random.Range(0, 3) * .02f); 
			Vector3 currentPos = new Vector3 (centerPos.x + xWiggleOffset, centerPos.y, centerPos.z + zWiggleOffset);
			transform.position = currentPos;
			transform.Rotate (xWiggleOffset * wobbleRotateAmount, 0, zWiggleOffset * wobbleRotateAmount);

			yield return null;
		}

	}

	public void DetectCurrentNode ()
	{
	}

	public bool CheckForValidMovement (Direction dir, int distance)
	{
		//TODO fix crash from in here
		try {
			MoveNode node;
			switch (dir) {
			case Direction.North:
				node = nodes [currentNode.x, currentNode.z + 1];
				break;
			case Direction.South:
				node = nodes [currentNode.x, currentNode.z - 1];
				break;
			case Direction.West:
				node = nodes [currentNode.x - 1, currentNode.z];
				break;
			case Direction.East:
				node = nodes [currentNode.x + 1, currentNode.z];
				break;
			default:
				node = null;
				break;
			}
			if (node != null) {
				if (!node.blocksMovement) {
					foreach (GameObject obj in node.objectsOnNode) {
						if (obj.tag == "Player" || obj.tag == "Enemy" || obj.tag == "Obstacle") {
							Debug.Log (gameObject + " can't move. " + obj.name + " is on the node.");
							return false;
						}
					}
					return true;
				}
			}
		} catch (Exception e) {	} 

		return false;
	}

}
