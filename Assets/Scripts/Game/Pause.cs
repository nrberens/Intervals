using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
	public Turn CurrentTurn;

	public bool paused;

	void Update() {
		if(Input.GetButtonDown("Pause")) {
			if(!paused)	{
				paused = true;
				UIController.ui.DisplayPauseUI();
				Time.timeScale = 0;
			} else {
				paused = false;
				UIController.ui.DisableAllUI();
				Time.timeScale = 1;
			}
		}
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