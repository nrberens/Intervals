using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class Turn : MonoBehaviour {

    public enum Phase {
        Player,
        Enemy
    }

    public Phase CurrentPhase { get; set; }
    public static int TurnNumber { get; set; }

    public PlayerController pc;
    public EnemyController ec;

    public void Start() {
        pc = FindObjectOfType<PlayerController>();
        ec = FindObjectOfType<EnemyController>(); //TODO Update to EnemiesController when > 1 enemies
        CurrentPhase = Phase.Player;
        TurnNumber = 1;
        //HACK
        pc.BeginPhase();
    }

    public void AdvancePhase() {
        Debug.Log("End of " + TurnNumber + " " + CurrentPhase);
        TurnNumber++;
        switch (CurrentPhase) {
            case Phase.Enemy:
                CurrentPhase = Phase.Player;
                pc.BeginPhase();
                break;
            case Phase.Player:
                CurrentPhase = Phase.Enemy;
                ec.BeginPhase();
                break;
        }


    }
}
