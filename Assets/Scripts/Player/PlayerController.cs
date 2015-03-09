using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour, ITurnBased {

    public Transform deathPrefab;
    public GameObject lowReadyMesh;
    public GameObject firingMesh;
    public GameObject meleeMesh;

    public PlayerMover Mover;
    public PlayerInventory Inventory;
    public PlayerInput Input;
	public PlayerShooter Shooter;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }

	// Use this for initialization
	void Start () {
	    Mover = GetComponentInParent<PlayerMover>();
	    Inventory = GetComponentInParent<PlayerInventory>();
	    Input = GetComponentInParent<PlayerInput>();
		Shooter = GetComponentInParent<PlayerShooter>();
	    CurrentTurn = FindObjectOfType<Turn>();

        //meshes
        //lowReadyMesh = transform.FindChild("shooter_geo_completeMesh/shooter_geo_lowReadyMesh").GetComponent<Renderer>();
        //firingMesh = transform.FindChild("shooter_geo_completeMesh/shooter_geo_firingMesh").GetComponent<Renderer>();
        //meleeMesh = transform.FindChild("shooter_geo_completeMesh/shooter_geo_meleeMesh").GetComponent<Renderer>();
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
        Debug.Log("Game over, man, game over!");
        Vector3 deathPrefabPosition = transform.FindChild("DeathPrefab").position;
        Transform death = (Transform) Instantiate(deathPrefab, deathPrefabPosition, Quaternion.Inverse(bullet.rotation));
        //TODO set rotation opposite to the bullet impact
    }
}
