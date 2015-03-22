using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController ui;
	public Canvas canvas;
	public RectTransform startMenuPanel;
	public RectTransform pausePanel;
	public RectTransform creditsPanel;
	public RectTransform gameOverPanel;
	public Text gameOverText;
	public bool initialPlay;

	void Awake ()
	{
		if (ui == null) {
			DontDestroyOnLoad (gameObject);
			ui = this;
		} else if (ui != this) {
			Destroy (gameObject);
		}

		initialPlay = true;
	}

	// Use this for initialization
	void Start ()
	{
		//DEBUG allow starting from any scene -- start menu only appears if scene = 0
		gameOverPanel.gameObject.SetActive (false);
		if (Application.loadedLevel == 0) {
			startMenuPanel.gameObject.SetActive (true);
		}
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void DisableAllUI ()
	{
		foreach (RectTransform rect in canvas.transform) {
			rect.gameObject.SetActive (false);
		}
	}

	public void StartGame ()
	{
		if (!initialPlay) {
			Debug.Log ("Restart level " + Application.loadedLevel);
			DisableAllUI();
			GameControl.gc.RestartLevel ();
		} else {
			Debug.Log ("Initial play - Current Level" + Application.loadedLevel + " loading level 2");
			initialPlay = false;
			DisableAllUI ();
			Application.LoadLevel (2);
		}
	}

	public void QuitToDesktop ()
	{
		Application.Quit ();
	}

	public void QuitToMenu ()
	{
		DisableAllUI ();
		DisplayMainMenu ();
	}

	public void DisplayMainMenu ()
	{
		DisableAllUI ();
		startMenuPanel.gameObject.SetActive (true);
	}

	public void DisplayPauseUI ()
	{
		DisableAllUI ();
		pausePanel.gameObject.SetActive (true);
	}

	public void DisplayGameOverUI ()
	{
		DisableAllUI ();
		gameOverPanel.gameObject.SetActive (true);
		//gameOverText.text = "GAME OVER\n SCORE " + GameControl.gc.currentScore + "\nHIGH SCORE " + GameControl.gc.highScore;
	}

	public void DisplayCredits ()
	{
		DisableAllUI ();
		creditsPanel.gameObject.SetActive (true);
	}

}
