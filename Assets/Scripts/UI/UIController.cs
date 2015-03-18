using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public static UIController ui;

    public RectTransform startMenuPanel;
    public RectTransform gameOverPanel;
    public Text gameOverText;

	void Awake() {
		if (ui == null) {
            DontDestroyOnLoad(gameObject);
            ui = this;
        } else if (ui != this) {
            Destroy(gameObject);
        }
	}

	// Use this for initialization
	void Start () {
		startMenuPanel.gameObject.SetActive (true);
	    gameOverPanel.gameObject.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame() {
		startMenuPanel.gameObject.SetActive (false);
		Application.LoadLevel(1);
	}

	public void DisplayGameOverUI() {
		gameOverPanel.gameObject.SetActive(true);
		//gameOverText.text = "GAME OVER\n SCORE " + GameControl.gc.currentScore + "\nHIGH SCORE " + GameControl.gc.highScore;
	}

}
