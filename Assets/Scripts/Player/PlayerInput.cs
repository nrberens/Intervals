using System;
using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    private float moveX, moveZ;

    private PlayerController pc;

    public bool allowInput;

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

                //SHOOTING
                if (Input.GetMouseButtonDown(0)) {
                    Debug.Log("Registered click");
                    pc.Shooter.BeginShot();
                }

                //IF PLAYER DOESN'T SHOOT, HANDLE MOVEMENT
                // TODO reinterpret input based on camera rotation
                //When camera rotation between 0-45 and 315-360, normal inputs
                //45.1-135 a = up, w = right, d = down, s = left
                //135.1-225, a = right, w = down, d = left, s = up 
                //225.1 - 315, a = back, w = left, d = up, s = right
                //need to change inputs, not directions

                //Get direction, rotate direction

                moveX = Input.GetAxis("Horizontal");
                moveZ = Input.GetAxis("Vertical");
                Direction moveDir;

                if (moveX != 0 || moveZ != 0) {
                    pc.acting = true;
                    allowInput = false;
                    moveDir = GetAdjustedDirection(moveX, moveZ, Camera.main.transform.rotation);
                    pc.Mover.Move(moveDir, 1);
                }

            }
                //ACTION PHASE 
            else if (!allowInput && pc.acting) {
                //wait for signal that movement or firing is done
            }
                //END OF TURN
            else if (!allowInput && !pc.acting) {
                pc.EndPhase();
            }
        }
    }

    private Direction GetAdjustedDirection(float moveX, float moveZ, Quaternion rotation) {
        float yRot = rotation.eulerAngles.y;
        int value = 0; //0 = Up, 1 = Right, 2 = Down, 3 = Left
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

        Direction direction = (Direction) value;

        return direction;
    }

}
