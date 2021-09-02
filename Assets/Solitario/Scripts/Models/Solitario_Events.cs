using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Solitario_Events
{
    /// <summary>
    /// Viene chiamato ogni volta che una sessione di gioco viene iniziata
    /// </summary>
    public static Action OnGameStart = delegate { };

    /// <summary>
    /// Viene chiamato ogni qualvolta venga fatta una mossa in gioco
    /// </summary>
    public static Action OnMoveDetected = delegate { };

    /// <summary>
    /// Viene chiamato ogni volta che la mossa è corretta
    /// </summary>
    public static Action OnStackSuccesfully = delegate { };

    /// <summary>
    /// Viene chiamato ogni volta che la mossa è sbagliata
    /// </summary>
    public static Action OnStackFailed = delegate { };

    /// <summary>
    /// Viene chiamato ogni volta che viene fatta una mossa sulle carte in alto
    /// </summary>
    public static Action OnStackOnTop = delegate { };

    /// <summary>
    /// Viene chiamato ogni volta che viene fatta una mossa sulle carte in basso
    /// </summary>
    public static Action OnStackOnBottom = delegate { };
}
