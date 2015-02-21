using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public interface ITurnBased {

    Turn CurrentTurn { get; set; }
    bool acting { get; set; }
}
