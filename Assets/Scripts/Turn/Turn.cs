using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class Turn : MonoBehaviour {

    public enum Phase {
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

    public void Start() {
        map = FindObjectOfType<Map>();
        turnsUntilNextSpawn = turnsBetweenSpawns;
        CurrentPhase = Phase.Player;
        TurnNumber = 0;
        //HACK player starts automatically
        PlayerController.pc.BeginPhase();
    }

    public void Update() {
        turnNumberPublic = TurnNumber;
    }

    public void AdvancePhase() {
        //Turn order: Bullet, Enemy, Player
        switch (CurrentPhase) {
            case Phase.Bullet:
                CurrentPhase = Phase.Enemy;
                EnemiesController.ec.BeginPhase();
                break;
            case Phase.Enemy:
                CurrentPhase = Phase.Player;
                Debug.Log("End of " + TurnNumber + " " + CurrentPhase);

                //End of turn maintenance
                turnsUntilNextSpawn--;
                PhoneController.pc.turnsUntilPhoneSpawn--;

                if (PhoneController.pc.currentPhone != null) {
                    PhoneController.pc.turnsUntilGameOver--;

                    if (PhoneController.pc.turnsUntilGameOver <= 0) {
                        PhoneController.pc.GameOverViaPhone();
                    }
                }

                //Spawn enemies
                if (turnsUntilNextSpawn <= 0) {
                    int index = Random.Range(0, map.SpawnPoints.Count - 1);
                    MoveNode spawnPoint = map.SpawnPoints[index];
                    map.SpawnEnemy(spawnPoint.x, spawnPoint.z);
                    turnsUntilNextSpawn = turnsBetweenSpawns;
                }
                //Spawn phones
                if (PhoneController.pc.turnsUntilPhoneSpawn <= 0) {
                    int phoneX, phoneZ;
                    //HACK potential infinite loop
                    do {
                        phoneX = Random.Range(0, map.mapWidth);
                        phoneZ = Random.Range(0, map.mapLength);
                    } while (map.Nodes[phoneX, phoneZ].objectsOnNode.Count > 0 || map.Nodes[phoneX, phoneZ].blocksMovement);

                    map.SpawnPhone(phoneX, phoneZ);
                    PhoneController.pc.turnsUntilPhoneSpawn = PhoneController.pc.turnsBetweenPhones;
                    PhoneController.pc.turnsUntilGameOver = PhoneController.pc.turnsToReachPhone;
                }

                TurnNumber++;
                PlayerController.pc.BeginPhase();
                break;
            case Phase.Player:
                CurrentPhase = Phase.Bullet;
                BulletsController.bc.BeginPhase();
                break;
        }


    }

    public void ResetTurn() {
        turnsUntilNextSpawn = turnsBetweenSpawns;
        TurnNumber = 0;
    }

    public void RestartTurn() {
        map = FindObjectOfType<Map>();
        CurrentPhase = Turn.Phase.Player;
    }
}
