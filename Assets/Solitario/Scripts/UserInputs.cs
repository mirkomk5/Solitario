using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputs : MonoBehaviour
{
    private SolitarioManager solitario;

    void Start()
    {
        solitario = FindObjectOfType<SolitarioManager>();
    }


    void Update()
    {
        GetMouseClick();
    }

    void GetMouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(hit)
            {
                switch(hit.collider.tag)
                {
                    case "Deck":
                        DeckHandle();
                        break;

                    case "Card":
                        CardHandle();
                        break;

                    case "Top":
                        TopHandle();
                        break;

                    case "Bottom":
                        BottomHandle();
                        break;
                }
            }

        }
    }


    void DeckHandle()
    {
        print("deck");
        solitario.PickFromDeal();
    }

    void CardHandle()
    {
        print("Card");
    }

    void TopHandle()
    {
        print("Top");
    }

    void BottomHandle()
    {
        print("bottom");
    }


}
