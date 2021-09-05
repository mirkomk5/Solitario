using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_DialogWinController : MonoBehaviour
{
    public GameObject content;
    public Text moves;
    public Text time;
    public Text scores;

    SolitarioManager solitario;

    private void OnEnable()
    {
        solitario = FindObjectOfType<SolitarioManager>();

        Solitario_Events.OnPlayerWin += PlayerWinHandle;
    }

    private void OnDisable()
    {
        Solitario_Events.OnPlayerWin -= PlayerWinHandle;
    }

    // ******

    void PlayerWinHandle()
    {
        int min = 0;
        int sec = 0;

        // Restituisce il tempo totale in min e sec
        solitario.GetPlayerTime(out min, out sec);

        ShowDialog(solitario.playerMoves.ToString(), $"{min}:{sec}", solitario.playerScore.ToString());
    }


    // *******

    public void ShowDialog(string moves, string time, string scores)
    {
        this.moves.text = moves;
        this.time.text = time;
        this.scores.text = $"punti <b><size=80>{scores}</size></b>";

        content.SetActive(true);
    }
}
