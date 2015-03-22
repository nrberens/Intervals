using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
	public Turn CurrentTurn;
	public bool paused;

	void Update ()
	{
		if (Application.loadedLevel != 0) {
			if (Input.GetButtonDown ("Pause")) {
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
		UIController.ui.DisplayPauseUI ();
		Time.timeScale = 0;
	}

	public void ResumeGame ()
	{
		paused = false;
		UIController.ui.DisableAllUI ();
		Time.timeScale = 1;
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