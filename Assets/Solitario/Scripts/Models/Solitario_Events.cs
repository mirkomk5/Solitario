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

    /// <summary>
    /// Viene chiamato ogni volta che vi è una transizione di carta
    /// </summary>
    public static Action OnCardTransition = delegate { };

    /// <summary>
    /// Viene chiamata ogni volta che una carta viene posizionata in cima alla lista delle carte finali
    /// </summary>
    public static Action OnCardMovedOnFinalStack = delegate { };

    /// <summary>
    /// Metodo chiamato quando il giocatore vince la partita
    /// </summary>
    public static Action OnPlayerWin = delegate { };
}
