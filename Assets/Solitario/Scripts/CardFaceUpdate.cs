using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFaceUpdate : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;

    private SpriteRenderer sRenderer;
    private Selectable selectable;
    private SolitarioManager sManager;
    private UserInputs userInputs;

    void Start()
    {
        List<string> deck = SolitarioManager.CreateCards();
        sManager = FindObjectOfType<SolitarioManager>();
        userInputs = FindObjectOfType<UserInputs>();

        int i = 0;
        foreach(string card in deck)
        {
            if(this.name == card)
            {
                cardFace = sManager.cardFaces[i];
                break;
            }

            i++;
        }

        sRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
    }


    void Update()
    {
        if(selectable.faceUp == true)
        {
            sRenderer.sprite = cardFace;
        }
        else
        {
            sRenderer.sprite = cardBack;
        }

        if (userInputs.slot1)
        {
            if (name == userInputs.slot1.name)
            
                sRenderer.color = Color.yellow;         
            else
                sRenderer.color = Color.white;
        }
    }
}
