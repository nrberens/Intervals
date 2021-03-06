﻿using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
	public Turn CurrentTurn;
	public bool paused;

	void Update ()
	{
		if (Application.loadedLevel != 0) {
			if (Input.GetButtonDown ("Pause") || Input.GetKeyDown(KeyCode.Escape)) {
				if (!paused) {
					PauseGame ();
				} else {
					ResumeGame ();
				}
			}
		}
	}

	public void PauseGame ()
	{
		paused = true;
        UIController.ui.DisableAllUI();
		UIController.ui.DisplayPauseUI ();
		Time.timeScale = 0;
	}

	public void ResumeGame ()
	{
		paused = false;
		UIController.ui.DisableAllUI ();
        UIController.ui.DisplayInGameUI();
		Time.timeScale = 1;
	}

	public void UnPauseAndQuitToMenu() {
		ResumeGame ();
		UIController.ui.QuitToMenu();
	}

//	void OnApplicationPause(bool pauseStatus) {
//		paused = pauseStatus;
//		if(paused) {
//			UIController.ui.DisplayPauseUI();
//			Time.timeScale = 0;
//		}
//		else {
//			UIController.ui.DisableAllUI();
//			Time.timeScale = 1;
//		}
//	}
}