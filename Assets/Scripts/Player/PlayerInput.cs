using System;
using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    private float moveX, moveZ;

    private PlayerController pc;

    public bool allowInput, playerSelected;

    //public Vector3 crosshairPos;


    // Use this for initialization
    void Start() {
        pc = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {

        //crosshairPos = Crosshair.GetCrosshairInWorld();

        if (pc.CurrentTurn.CurrentPhase == Turn.Phase.Player) {
            //INPUT PHASE 
            //TODO track time between inputs, only allow input every half second or so
            if (allowInput && !pc.acting) {
                if (!playerSelected) {
                    if (Input.GetMouseButtonDown(0)) {
                        Transform target = GetTargetOfClick();
                        //If target is player
                        if (target.tag == "Player") {
                            playerSelected = true;
                            pc.Mover.TagMovableNodes();
                            pc.Shooter.TagShootableEnemies();
                        }
                    }
                } else if (playerSelected) {
                    if (Input.GetMouseButton(0)) {
                        //show moveable nodes
                        //show shootable enemies
                    } else if (Input.GetMouseButtonUp(0)) {
                        playerSelected = false;
                        pc.Mover.UnTagMovableNodes();
                        pc.Shooter.UnTagShootableEnemies();
                    }
                }
            }
            //END OF TURN
            else if (!allowInput && !pc.acting) {
                pc.EndPhase();
            }
        }
    }

    //On click, get target of mouse click
    public Transform GetTargetOfClick() {
        //target layers EXCLUDING layer 9 - obstacles
        int layerMask = 1 << 9;
        layerMask = ~layerMask;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 5.0f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            return hit.transform;
        } else return null;
    }

    private Direction GetAdjustedDirection(float moveX, float moveZ, Quaternion rotation) {
        float yRot = rotation.eulerAngles.y;
        int value = 0; //0 = North, 1 = East, 2 = South, 3 = West
        int offset = 0; //number of 90 deg turns clockwise

        if (moveX > 0) {
            value = 1;
        } else if (moveX < 0) {
            value = 3;
        } else if (moveZ > 0) {
            value = 0;
        } else if (moveZ < 0) {
            value = 2;
        }

        if ((yRot >= 0 && yRot < 45) || yRot >= 315 && yRot < 360) {
            value += 0;
        } else if (yRot >= 45 && yRot < 135) {
            value += 1;
        } else if (yRot >= 135 && yRot < 225) {
            value += 2;
        } else if (yRot >= 225 && yRot < 315) {
            value += 3;
        }

        value %= 4; //modulus clamps value between 0 and 3

        Direction direction = (Direction)value;

        return direction;
    }

}
