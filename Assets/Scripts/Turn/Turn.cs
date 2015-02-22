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

    private PlayerController _pc;
    private EnemiesController _ec;

    public void Start() {
        _pc = FindObjectOfType<PlayerController>();
        _ec = FindObjectOfType<EnemiesController>(); 
        CurrentPhase = Phase.Player;
        TurnNumber = 1;
        //HACK
        _pc.BeginPhase();
    }

    public void AdvancePhase() {
        Debug.Log("End of " + TurnNumber + " " + CurrentPhase);
        TurnNumber++;
        switch (CurrentPhase) {
            case Phase.Enemy:
                CurrentPhase = Phase.Player;
                _pc.BeginPhase();
                break;
            case Phase.Player:
                CurrentPhase = Phase.Enemy;
                _ec.BeginPhase();
                break;
        }


    }
}
