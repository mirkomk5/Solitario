using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Solitario_Params
{
    public static readonly int MAX_CARDS = 52;
    public static readonly int SCORE_TOPCARD = 15;            // Punti assegnati per l'accoppiamento delle carte con la parte superiore
    public static readonly int SCORE_BOTTOMCARD = 5;          // Punti assegnati per l'accoppiamento delle carte con la parte inferiore

    public static Color COLOR_SELECTED = new Color(0.89f, 0.91f, 0.66f);

    /// <summary>
    /// Settaggi per gli offset da applicare alle carte
    /// </summary>
    public static class CardOffset
    {
        public static float xOffsetTrisCard = -0.7f;
        public static float xOffset = 0.2f;
        public static float yOffset = 0.25f;
        public static float zOffset = -0.2f;
    }
        
}

