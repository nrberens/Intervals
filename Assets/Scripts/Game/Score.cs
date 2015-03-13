using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    public int highScore;
    public int currentScore;

	// Use this for initialization
	void Start () {
	    currentScore = 0;
	}
	
	// Update is called once per frame
	void Update () {
        currentScore = Turn.TurnNumber;
	}

    public void CheckForHighScore() {
        if (currentScore > highScore) {
            highScore = currentScore;
            //TODO display new high score
            //TODO save high score to file?
            Debug.Log("New High Score!");
        }
    }
}
