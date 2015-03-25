using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

    public static GameControl gc;

	public float currentScore; 
	public float highScore;
    //public AudioSource music;

    void Awake() {
        if (gc == null) {
            DontDestroyOnLoad(gameObject);
            gc = this;
        } else if (gc != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        currentScore = 0;
        //TODO load high score from file
        //Load();
	    //music = FindObjectOfType<AudioSource>();
        //DontDestroyOnLoad(music);
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.highScore = highScore;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData) bf.Deserialize(file);

            highScore = data.highScore;
        }
    }

	public void RestartLevel() {
		if (PlayerController.pc != null) {
			PlayerController.pc.RestartLevel();
		} 
	}

    public void CheckForHighScore() {
        if (currentScore > highScore) {
            highScore = currentScore;
            //TODO display new high score
            //TODO save high score to file?
            Debug.Log("New High Score!");
        }
    }

	public void ResetGameController() {
		//TODO
	}

	public static void ClearGameObjectsBeforeRestart ()
	{
		GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);
		
		for (int i = 0; i < GameObjects.Length; i++)
		{
			GameObject obj = GameObjects[i];
			//Exclude certain types here
			if(obj.name == "GameController") continue;
			else if(obj.layer == 5) continue; //layer 5 is UI
			else if(obj.name == "UIController") continue;
			else if (obj.name == "AudioController" || obj.name == "Audio Source") continue;

			Destroy(obj);
		}
	}

	public static void ResetStaticVariables() {
		EnemyController.totalEnemies = 0;
		EnemyBullet.totalBullets = 0;
		//Turn.TurnNumber = 0;
	}
}

[Serializable]
class PlayerData {
    public float highScore { get; set; }
}
