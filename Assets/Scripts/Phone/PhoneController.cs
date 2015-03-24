using UnityEngine;
using System.Collections;

public class PhoneController : MonoBehaviour, ITurnBased {
    public static PhoneController pc;

    public Turn CurrentTurn { get; set; }
    public bool acting { get; set; }

    public Transform phoneMesh;

    public int turnsBetweenSpawn;
    public int turnsUntilSpawn;

    void Awake() {
        if (pc == null) {
            DontDestroyOnLoad(gameObject);
            pc = this;
        } else if (pc != this) {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {

        turnsUntilSpawn = turnsBetweenSpawn;
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
