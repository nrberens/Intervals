using UnityEngine;
using System.Collections;

public class MovableNode : MonoBehaviour {

	public enum NodeState {
		MovableSelected,
		MovableUnselected,
		MeleeableSelected,
		MeleeableUnselected,
        Off
	}

	public Renderer selectedRenderer;
	public Renderer unselectedRenderer;

	public Material blueMat;
	public Material redMat;

	public NodeState currentState;

	// Use this for initialization
	void Start () {
		currentState = NodeState.Off;
		selectedRenderer.enabled = false;
		unselectedRenderer.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		switch(currentState) {
		case NodeState.MovableSelected:
			unselectedRenderer.enabled = false;
			selectedRenderer.enabled = true;
			if(selectedRenderer.material != blueMat) selectedRenderer.material = blueMat;
			break;
		case NodeState.MovableUnselected:
			selectedRenderer.enabled = false;
			unselectedRenderer.enabled = true;
			if(unselectedRenderer.material != blueMat) unselectedRenderer.material = blueMat;
			break;
		case NodeState.MeleeableSelected:
			unselectedRenderer.enabled = false;
			selectedRenderer.enabled = true;
			if(selectedRenderer.material != redMat) selectedRenderer.material = redMat;
			break;
		case NodeState.MeleeableUnselected:
			selectedRenderer.enabled = false;
			unselectedRenderer.enabled = true;
			if(unselectedRenderer.material != redMat) unselectedRenderer.material = redMat;
			break;
		case NodeState.Off:
			selectedRenderer.enabled = false;
			unselectedRenderer.enabled = false;
			break;
		}
	}
}
