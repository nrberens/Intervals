using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController ui;
	public Canvas canvas;
	public RectTransform startMenuPanel;
	public RectTransform pausePanel;
	public RectTransform creditsPanel;
    
	public RectTransform gameOverPanel;
	public Text gameOverScoreText;

	public RectTransform ingameUI;
	public Text scoreText;
	public Text phoneText;

    public RectTransform tutorialPanel;
    public Text header_text;
    public Text p1_text;
    public Text p2_text;
    public Text p3_text;

    public RectTransform highscorePanel;
    public Text highscore_score_text;
    public Text letter1_text;
    public Text letter2_text;
    public Text letter3_text;

    public RectTransform leaderboardPanel;
    public Text score1_text;
    public Text score2_text;
    public Text score3_text;
    public Text score4_text;
    public Text score5_text;
    public Text score6_text;
    public Text score7_text;
    public Text score8_text;
    public Text score9_text;
    public Text score10_text;
    public Text[] scoreTexts;

    public int tutorialPage;
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
        scoreTexts = new Text[] {score1_text, score2_text, score3_text, score4_text, score5_text, score6_text, score7_text, score8_text, score9_text, score10_text};
	}

	// Use this for initialization
	void Start ()
	{
		//DEBUG allow starting from any scene -- start menu only appears if scene = 0
		DisableAllUI();
		if (Application.loadedLevel == 0) {
			startMenuPanel.gameObject.SetActive (true);
		}
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnGUI() {
		if(ingameUI.gameObject.activeInHierarchy == true) {
			scoreText.text = "score: " + GameControl.gc.currentScore; 
			if(PhoneController.pc.currentPhone != null) {
				phoneText.text = "turns to reach phone: " + PhoneController.pc.turnsUntilGameOver;
			} else {
				phoneText.text = "turns until new phone: " + PhoneController.pc.turnsUntilPhoneSpawn;
			}
		}

	    if (tutorialPanel.gameObject.activeInHierarchy) {
	        switch (tutorialPage) {
                case 1:
	                header_text.text = "how to play. (1/3)";
                    p1_text.gameObject.SetActive(true);
                    p2_text.gameObject.SetActive(false);
                    p3_text.gameObject.SetActive(false);
	                break;
                case 2: 
	                header_text.text = "how to play. (2/3)";
                    p1_text.gameObject.SetActive(false);
                    p2_text.gameObject.SetActive(true);
                    p3_text.gameObject.SetActive(false);
	                break;
                case 3: 
	                header_text.text = "how to play. (3/3)";
                    p1_text.gameObject.SetActive(false);
                    p2_text.gameObject.SetActive(false);
                    p3_text.gameObject.SetActive(true);
	                break;
	        }
	    }

	}

	public void DisableAllUI ()
	{
		foreach (RectTransform rect in canvas.transform) {
			rect.gameObject.SetActive (false);
		}
	}

	public void EnableAllUI() {
		foreach(RectTransform rect in canvas.transform) {
			rect.gameObject.SetActive (true);
		}
	}

	public void StartGame ()
	{
		if (!initialPlay) {
			Debug.Log ("Restart level " + Application.loadedLevel);
			DisableAllUI ();
			DisplayInGameUI();
			GameControl.gc.RestartLevel ();
		} else {
			Debug.Log ("Initial play - Current Level" + Application.loadedLevel + " loading level 3");
			initialPlay = false;
			DisableAllUI ();
			DisplayInGameUI();
			Application.LoadLevel (3);
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
		gameOverScoreText.text = "score: " + GameControl.gc.currentScore;
		gameOverPanel.gameObject.SetActive (true);
        //HACK
        DisplayHighscoreUI();
	}

	public void DisplayCredits ()
	{
		DisableAllUI ();
		creditsPanel.gameObject.SetActive (true);
	}

	public void DisplayInGameUI() {
		DisableAllUI ();
		ingameUI.gameObject.SetActive(true);
	}

    public void DisplayTutorialUI() {
        DisableAllUI();
        tutorialPanel.gameObject.SetActive(true);
        tutorialPage = 1;
    }

    public void AdvanceTutorialPage() {
        if (tutorialPage < 3) {
            tutorialPage++;
        } else if (tutorialPage == 3) {
            tutorialPage = 1;
        }
    }

    public void DisplayHighscoreUI() {
        DisableAllUI();
        highscorePanel.gameObject.SetActive(true);
    }

    public void AdvanceLetter(Text letter) {
        if (letter.text.Equals("Z")) letter.text = 'A'.ToString();
        else if (letter.text.Equals("_")) letter.text = 'A'.ToString();
        else {
            char c = letter.text.ToCharArray()[0];
            c++;
            letter.text = c.ToString();
        }
    }

    public void DisplayLeaderboardUI() {
        for (int i = 0; i < 10; i++) {
            LeaderboardEntry entry = GameControl.gc.leaderboard.leaderboard[i];
            scoreTexts[i].text = i + 1 + ". " + entry.name + " " + entry.score;
        }

        DisableAllUI();
        leaderboardPanel.gameObject.SetActive(true);
    }

}
