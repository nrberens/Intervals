using UnityEngine;
using System.Collections;

public class PulsateGUI : MonoBehaviour {

    public float pulseAmount, rotateAmount;
    public float xOffset, yOffset;

	// Use this for initialization
	void Start () {
        xOffset = 0;
        yOffset = 0;
		iTween.MoveBy(gameObject, iTween.Hash("x", pulseAmount + xOffset, "y", pulseAmount + yOffset, "easeType", "easeInOutQuad", "loopType", "pingPong"));
        iTween.RotateBy(gameObject, iTween.Hash("z", rotateAmount, "easeType", "easeInOutQuad", "loopType", "pingPong"));
	}
	
	// Update is called once per frame
	void Update () {
        xOffset = Random.Range(-40, 40);
        yOffset = Random.Range(-40, 40);
	
	}
}
