using UnityEngine;
using System.Collections;

public class Phone : MonoBehaviour {

    public Transform phoneMesh;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }
    public float phoneDelay;

    public MoveNode currentNode;

	// Use this for initialization
	void Start () {
	    StartCoroutine(StartPhoneRinging());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Player") {
            //TODO destroy phone, play sound effect, reset timers
            AudioController.ac.PlayPhonePickUp();
			GameControl.gc.currentScore += GameControl.gc.scorePerPhone;
            PhoneController.pc.currentPhone = null;
			currentNode.RemoveFromNode (gameObject);
            Destroy(gameObject);
        }
    }

    public IEnumerator StartPhoneRinging() {
        while (true) {
            yield return new WaitForSeconds(phoneDelay);
            AudioController.ac.PlayPhoneRing();
        }
    }
}
