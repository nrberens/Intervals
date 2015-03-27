using UnityEngine;
using System.Collections;

public class Phone : MonoBehaviour {

    public Transform phoneMesh;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }

    public MoveNode currentNode;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Player") {
            //TODO destroy phone, play sound effect, reset timers
			GameControl.gc.currentScore += GameControl.gc.scorePerPhone;
            PhoneController.pc.currentPhone = null;
            Destroy(gameObject);
        }
    }
}
