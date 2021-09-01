using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputs : MonoBehaviour
{
    public GameObject slot1;
    private SolitarioManager solitario;

    void Start()
    {
        solitario = FindObjectOfType<SolitarioManager>();

        slot1 = this.gameObject;
    }


    void Update()
    {
        GetMouseClick();
    }

    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                switch (hit.collider.tag)
                {
                    case "Deck":
                        DeckHandle();
                        break;

                    case "Card":
                        CardHandle(hit.collider.gameObject);
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
        solitario.PickFromDeck();
    }

    void CardHandle(GameObject selected)
    {
        if (slot1 == this.gameObject)
        {
            slot1 = selected;
        }
        else if (slot1 != selected)
        {
            if (Stackable(selected))
            {
            }
            else
            {
                slot1 = selected;
            }
        }
    }

    void TopHandle()
    {
    }

    void BottomHandle()
    {
    }


    // ************************

    bool Stackable(GameObject selected)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();

        if(s2.top)
        {
            if (s1.seme == s2.seme || (s1.value == 1 && s2.seme == null))
            {
                if (s1.value == s2.value + 1)
                    return true;
            }
            else
                return false;
        }
        else
        {
            if(s1.value == s2.value -1)
            {
                // Determina il colore della carta dal suo seme. C = cuore => rosso. P = Picche => rosso
                bool card1Red = false;
                bool card2Red = false;

                if (s1.seme == "C" || s1.seme == "P")
                    card1Red = true;

                if (s2.seme == "C" || s2.seme == "P")
                    card2Red = true;

                // Se le due carte hanno lo stesso colore non sono componibili. Diversamente invece ritornerà vero
                if (card1Red == card2Red)
                {
                    print("Cards not stackable");
                    return false;
                }
                else
                {
                    print("Cards stackable");
                    return true;
                }
            }
        }

        return false;
    }

}
