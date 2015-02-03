using UnityEngine;
using System.Collections;

//Mover is an object that can move from node to node

public interface IMover {

    MoveNode currentNode {get; set;}

    void MoveForward(int distance);
    void MoveBackward(int distance);
    void MoveLeft(int distance);
    void MoveRight(int distance);
    void GetCurrentNode();

}