using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour {

    protected Transform player;
    protected Vector3 destPos;
    protected GameObject[] pointList;
    protected float fireRate;
    protected float elapsedTime;

    protected Transform bulletSpawnPoint { get; set; }

    protected virtual void Initialize() { }
    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }

    // Use this for initialization
    void Start() {
        Initialize();
    }

    // Update is called once per frame
    void Update() {
        FSMUpdate();
    }

    void FixedUpdate() {
        FSMFixedUpdate();
    }

    //Update once per turn
    public virtual void UpdateAI() {
    }
}