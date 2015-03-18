using UnityEngine;
using System.Collections;

public class ShootableNode : MonoBehaviour {
    public enum NodeState {
        ShootableSelected,
        ShootableUnselected,
        Off
    }

	public Renderer selectedRenderer;
	public Renderer unselectedRenderer;

    public NodeState currentState;

	// Use this for initialization
	void Start () {
		currentState = NodeState.Off;
		selectedRenderer.enabled = false;
		unselectedRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    switch (currentState) {
	        case NodeState.ShootableSelected:
	            unselectedRenderer.enabled = false;
	            selectedRenderer.enabled = true;
	            break;
	        case NodeState.ShootableUnselected:
	            selectedRenderer.enabled = false;
	            unselectedRenderer.enabled = true;
	            break;
            case NodeState.Off:
	            selectedRenderer.enabled = false;
	            unselectedRenderer.enabled = false;
	            break;
	    }
	}
}
