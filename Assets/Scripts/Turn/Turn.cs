using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class Turn : MonoBehaviour
{

	public enum Phase
	{
		Player,
		Bullet,
		Enemy,
		GameOver,
		Paused
	}

	public Phase CurrentPhase; //TODO reset in game over
	public Phase PausedPhase; //reset in game over? not necessary

	public int TurnNumber; //TODO reset in game over
	public int turnNumberPublic;

	public int turnsBetweenSpawns;
	public int turnsUntilNextSpawn;
	private Map map;

	public void Start ()
	{
		map = FindObjectOfType<Map> ();
		turnsUntilNextSpawn = turnsBetweenSpawns;
		CurrentPhase = Phase.Player;
		TurnNumber = 0;
		//HACK player starts automatically
		PlayerController.pc.BeginPhase ();
	}

	public void Update ()
	{
		turnNumberPublic = TurnNumber;
	}

	public void AdvancePhase ()
	{
		//Turn order: Bullet, Enemy, Player
		switch (CurrentPhase) {
		case Phase.Bullet:
			CurrentPhase = Phase.Enemy;
			EnemiesController.ec.BeginPhase ();
			break;
		case Phase.Enemy:
			CurrentPhase = Phase.Player;
			Debug.Log ("End of " + TurnNumber + " " + CurrentPhase);
			turnsUntilNextSpawn--;
			if (turnsUntilNextSpawn <= 0) {
				int index = Random.Range (0, map.SpawnPoints.Count - 1);
				MoveNode spawnPoint = map.SpawnPoints [index];
				map.SpawnEnemy (spawnPoint.x, spawnPoint.z);
				turnsUntilNextSpawn = turnsBetweenSpawns;
			}
			TurnNumber++;
			PlayerController.pc.BeginPhase ();
			break;
		case Phase.Player:
			CurrentPhase = Phase.Bullet;
			BulletsController.bc.BeginPhase ();
			break;
		}


	}

	public void ResetTurn() {
		turnsUntilNextSpawn = turnsBetweenSpawns;
		TurnNumber = 0;
	}

	public void RestartTurn() {
		map=FindObjectOfType<Map>();
		CurrentPhase = Turn.Phase.Player;
	}
}
