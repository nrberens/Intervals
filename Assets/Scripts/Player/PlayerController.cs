using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour {

	public static PlayerController pc;

    public Transform deathPrefab;
    public GameObject lowReadyMesh;
    public GameObject firingMesh;
    public GameObject meleeMesh;

    public PlayerMover Mover;
    public PlayerInventory Inventory;
    public PlayerInput Input;
	public PlayerShooter Shooter;

    public Turn CurrentTurn { get; set; }
	public bool acting; 
	//public bool turnFinished;

	void Awake() {
//		if (pc == null) {
//			DontDestroyOnLoad(gameObject);
//			pc = this;
//		} else if (pc != this) {
//			Destroy(gameObject);
//		}
		pc = this;
	}

	// Use this for initialization
	void Start () {
	    Mover = GetComponentInParent<PlayerMover>();
	    Inventory = GetComponentInParent<PlayerInventory>();
	    Input = GetComponentInParent<PlayerInput>();
		Shooter = GetComponentInParent<PlayerShooter>();
	    CurrentTurn = FindObjectOfType<Turn>();

        //meshes
        firingMesh.SetActive(false);
        meleeMesh.SetActive(false);

	    acting = false;
	}

    void Update() {

    }

    public void BeginPhase() {
            Input.allowInput = true;
    }

    public void EndPhase() {
        if(CurrentTurn.CurrentPhase == Turn.Phase.Player)
            CurrentTurn.AdvancePhase();   
        else Debug.Log("Calling AdvancePhase from the wrong object!");
    }

    public void GameOver(Transform bullet) {
		CurrentTurn.CurrentPhase = Turn.Phase.GameOver;
        Debug.Log("Game over, man, game over!");
        Vector3 deathPrefabPosition = transform.FindChild("DeathPrefab").position;
        Transform death = (Transform) Instantiate(deathPrefab, deathPrefabPosition, Quaternion.Inverse(bullet.rotation));
        Debug.Log("Died with a score of " + GameControl.gc.currentScore);
        GameControl.gc.CheckForHighScore();
        //world falls away? show score, restart button
        WorldFallAway wfa = FindObjectOfType<WorldFallAway>();
        StartCoroutine(wfa.ManageFallAwayTiming());
        UIController.ui.DisplayGameOverUI();
        //GameControl.gc.Save();
    }

    public void RestartLevel() {
		BulletsController.bc.ResetBulletsController();
		EnemiesController.ec.ResetEnemiesController();
		CurrentTurn.ResetTurn();
		Map map = FindObjectOfType<Map>();
		map.ResetMap();
		GameControl.ClearGameObjectsBeforeRestart();
		GameControl.ResetStaticVariables();
        GameControl.gc.currentScore = 0;
		//Turn.ResetTurn();
        Application.LoadLevel(Application.loadedLevel);
		CurrentTurn.RestartTurn ();
    }

	public static void ResetPlayerController() {
	}
}
