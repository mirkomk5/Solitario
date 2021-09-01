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

    void Start()
    {
        List<string> deck = SolitarioManager.CreateCards();
        sManager = FindObjectOfType<SolitarioManager>();

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
    }
}
