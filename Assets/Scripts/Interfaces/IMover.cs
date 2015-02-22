using UnityEngine;
using System.Collections;

//Mover is an object that can move from node to node

public interface IMover
{

    MoveNode[,] nodes { get; }

    MoveNode currentNode {get; set;}

    void MoveUp(int distance);
    void MoveDown(int distance);
    void MoveLeft(int distance);
    void MoveRight(int distance);
    void DetectCurrentNode();
    IEnumerator MoveToNode(MoveNode targetNode);

}