using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_TopBarController : MonoBehaviour
{
    public Text score;
    public Text moves;

    SolitarioManager solitario;

    private void OnEnable()
    {
        solitario = FindObjectOfType<SolitarioManager>();

        solitario.OnScoreUpdate += ScoreHandle;
        solitario.OnMovesUpdate += MovesHandle;
    }

    private void OnDisable()
    {

    }


    // Delegates *******************************

    void ScoreHandle()
    {
        score.text = solitario.playerScore.ToString();
    }

    void MovesHandle()
    {
        moves.text = solitario.playerMoves.ToString();
    }

}
