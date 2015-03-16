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
    private Map map;

    public void Start() {
        _pc = FindObjectOfType<PlayerController>();
        _ec = FindObjectOfType<EnemiesController>(); 
		_bc = FindObjectOfType<BulletsController>();
        map = FindObjectOfType<Map>();
        CurrentPhase = Phase.Player;
        TurnNumber = 0;
        //HACK player starts automatically
        _pc.BeginPhase();
    }

    public void AdvancePhase() {
		//Turn order: Bullet, Enemy, Player
        switch (CurrentPhase) {
            case Phase.Bullet:
                CurrentPhase = Phase.Enemy;
                _ec.BeginPhase();
                break;
			case Phase.Enemy:
				CurrentPhase = Phase.Player;
		        Debug.Log("End of " + TurnNumber + " " + CurrentPhase);
                map.turnsUntilNextSpawn--;
                TurnNumber++;
				_pc.BeginPhase();
				break;
            case Phase.Player:
                CurrentPhase = Phase.Bullet;
                _bc.BeginPhase();
                break;
        }


    }
}
