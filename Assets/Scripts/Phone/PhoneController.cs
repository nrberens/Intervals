using UnityEngine;
using System.Collections;

public class PhoneController : MonoBehaviour, ITurnBased {
    public static PhoneController pc;

    public Phone currentPhone;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }


    public int turnsBetweenPhones;
    public int turnsUntilPhoneSpawn;
    public int turnsToReachPhone;
    public int turnsUntilGameOver;


    void Awake() {
        if (pc == null) {
            DontDestroyOnLoad(gameObject);
            pc = this;
        } else if (pc != this) {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
	    CurrentTurn = FindObjectOfType<Turn>();
        turnsUntilPhoneSpawn = 0;
	    turnsUntilGameOver = turnsToReachPhone;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GameOverViaPhone() {
        CurrentTurn.CurrentPhase = Turn.Phase.GameOver;
        Debug.Log("You didn't get to the phone in time!");
        Debug.Log("Died with a score of " + GameControl.gc.currentScore);
        GameControl.gc.CheckForHighScore();
        //world falls away? show score, restart button
        WorldFallAway wfa = FindObjectOfType<WorldFallAway>();
        StartCoroutine(wfa.ManageFallAwayTiming());
        UIController.ui.DisplayGameOverUI();
        //GameControl.gc.Save();PlayerController.pc.GameOver();
    }
}
