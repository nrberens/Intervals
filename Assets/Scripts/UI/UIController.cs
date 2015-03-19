using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public static UIController ui;

	public Canvas canvas;

    public RectTransform startMenuPanel;
	public RectTransform pausePanel;
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
		//DEBUG allow starting from any scene -- start menu only appears if scene = 0
		    gameOverPanel.gameObject.SetActive(false);
		if(Application.loadedLevel == 0) {
			startMenuPanel.gameObject.SetActive (true);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisableAllUI() {
		foreach (RectTransform rect in canvas.transform) {
			rect.gameObject.SetActive(false);
		}
	}

	public void StartGame() {
		startMenuPanel.gameObject.SetActive (false);
		Application.LoadLevel(1);
	}

	public void DisplayPauseUI() {
		pausePanel.gameObject.SetActive(true);
	}

	public void DisplayGameOverUI() {
		gameOverPanel.gameObject.SetActive(true);
		//gameOverText.text = "GAME OVER\n SCORE " + GameControl.gc.currentScore + "\nHIGH SCORE " + GameControl.gc.highScore;
	}

}
