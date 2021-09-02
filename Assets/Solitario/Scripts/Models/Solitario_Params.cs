using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solitario.Params
{
    public class Solitario_Params
    {
        public static int SCORE_TOPCARD = 15;
        public static int SCORE_BOTTOMCARD = 5;

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
}
