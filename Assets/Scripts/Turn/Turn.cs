using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class Turn : MonoBehaviour {

    public enum Phase {
        Player,
		Bullet,
        Enemy
    }

    public Phase CurrentPhase { get; set; }
    public static int TurnNumber { get; set; }

    private PlayerController _pc;
    private EnemiesController _ec;
	private BulletsController _bc;

    public void Start() {
        _pc = FindObjectOfType<PlayerController>();
        _ec = FindObjectOfType<EnemiesController>(); 
		_bc = FindObjectOfType<BulletsController>();
        CurrentPhase = Phase.Player;
        TurnNumber = 1;
        //HACK plyaer starts automatically
        _pc.BeginPhase();
    }

    public void AdvancePhase() {
        Debug.Log("End of " + TurnNumber + " " + CurrentPhase);
        TurnNumber++;
        switch (CurrentPhase) {
            case Phase.Player:
                CurrentPhase = Phase.Bullet;
                _bc.BeginPhase();
                break;
			case Phase.Bullet:
				CurrentPhase = Phase.Enemy;
				_ec.BeginPhase();
				break;
            case Phase.Enemy:
                CurrentPhase = Phase.Player;
                _pc.BeginPhase();
                break;
        }


    }
}
