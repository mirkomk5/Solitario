using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Solitario_History
{
    public GameObject card;
    public Vector3 cardPosition;
    public Transform cardParent;
    public int deckLocation;
    public enum ActionType { FaceUp, Moves, Deck };
    public ActionType actionType;

    public Solitario_History(GameObject card, ActionType actionType)
    {
        this.card = card;
        this.cardPosition = card.transform.position;
        this.cardParent = card.transform.parent;
        this.actionType = actionType;
    }

    public Solitario_History(GameObject card, ActionType actionType, int deckLocation)
    {
        this.card = card;
        this.cardPosition = card.transform.position;
        this.cardParent = card.transform.parent;
        this.actionType = actionType;
        this.deckLocation = deckLocation;
    }
}
