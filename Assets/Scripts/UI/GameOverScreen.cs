using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {
    public Canvas gameOverCanvas;
    public Transform gameOverPanel;
    public Text gameOverText;
    public Score score;

	// Use this for initialization
	void Start () {
	    gameOverCanvas.enabled = false;
	    score = FindObjectOfType<Score>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayGameOverUI() {
        gameOverCanvas.enabled = true;
        gameOverText.text = "GAME OVER\n SCORE " + score.currentScore + "\nHIGH SCORE " + score.highScore;
    }
}
