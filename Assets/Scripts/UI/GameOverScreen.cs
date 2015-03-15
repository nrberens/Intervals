using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {
    public Canvas gameOverCanvas;
    public Transform gameOverPanel;
    public Text gameOverText;

	// Use this for initialization
	void Start () {
	    gameOverCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayGameOverUI() {
        gameOverCanvas.enabled = true;
        gameOverText.text = "GAME OVER\n SCORE " + GameControl.gc.currentScore + "\nHIGH SCORE " + GameControl.gc.highScore;
    }
}
