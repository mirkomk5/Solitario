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
    private Animator anim;
    private bool flipped = false;

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
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if(selectable.faceUp == true)
        {
            //sRenderer.sprite = cardFace;
            if(!flipped)
                StartCoroutine(CardFlipHandle(cardFace));
        }
        else
        {
            //sRenderer.sprite = cardBack;
            if(flipped)
                StartCoroutine(CardFlipHandle(cardBack));
        }

        if (userInputs.slot1)
        {
            if (name == userInputs.slot1.name)

                sRenderer.color = Solitario_Params.COLOR_SELECTED;
            else
                sRenderer.color = Color.white;
        }
    }

    IEnumerator CardFlipHandle(Sprite cardSprite)
    {
        anim.Play("card_flip");
        yield return new WaitForSeconds(0.2f);
        sRenderer.sprite = cardSprite;

        flipped = !flipped;
    }
}
