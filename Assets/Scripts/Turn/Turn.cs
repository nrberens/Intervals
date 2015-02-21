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

    public void Awake() {
        CurrentPhase = Phase.Player;
        TurnNumber = 1;
    }

    public void AdvancePhase() {
        switch (CurrentPhase) {
            case Phase.Enemy:
                CurrentPhase = Phase.Player;
                break;
            case Phase.Player:
                CurrentPhase = Phase.Enemy;
                break;
        }

        Debug.Log(CurrentPhase);
    }
}
