using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HistoryController : MonoBehaviour
{
    public List<Solitario_History> cardHistory;

    private SolitarioManager solitario;

    private void Start()
    {
        cardHistory = new List<Solitario_History>();
        solitario = GetComponent<SolitarioManager>();
    }

    /// <summary>
    /// Registra l'azione del player
    /// </summary>
    /// <param name="card">la carta selezionata</param>
    /// <param name="actionType">Tipologia di azione</param>
    public void Register(GameObject card, Solitario_History.ActionType actionType)
    {
        cardHistory.Add(new Solitario_History(card, actionType));
    }

    public void Register(GameObject card, Solitario_History.ActionType action, int deckLocation)
    {
        cardHistory.Add(new Solitario_History(card, action, deckLocation));
    }


    /// <summary>
    /// Procede all'azione annulla ultima mossa
    /// </summary>
    public void Undo()
    {
        // Se non ci sono azioni registrate, ferma la funzione
        if (cardHistory.Count == 0)
            return;

        int index = cardHistory.Count-1;

        //Se l'azione è stata una mossa, ripristina la sua ultima posizione
        if (cardHistory[index].actionType == Solitario_History.ActionType.Moves)
        {
            cardHistory[index].card.transform.position = cardHistory[index].cardPosition;
            cardHistory[index].card.transform.SetParent(cardHistory[index].cardParent);
        }

        // Se l'azione è stata di scoprire una carta, la rende nascosta di nuovo
        if( cardHistory[index].actionType == Solitario_History.ActionType.FaceUp)
        {
            cardHistory[index].card.GetComponent<Selectable>().faceUp = false;
        }

        // Se l'azione è stata di scoprire carte dal mazzo, ritorna alle carte visualizzate in precedenza
        if(cardHistory[index].actionType == Solitario_History.ActionType.Deck)
        {
            if(solitario.GetDeckLocation() -2 >= 0)
                solitario.PickFromDeckAt(solitario.GetDeckLocation() - 2);
        }

        // Rimuove l'ultima azione
        cardHistory.RemoveAt(index);
    }
}
