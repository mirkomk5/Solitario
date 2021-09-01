using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
                        TopHandle(hit.collider.gameObject);
                        break;

                    case "Bottom":
                        BottomHandle(hit.collider.gameObject);
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
        // Se la carta selezionata è ancora nascosta
        if (!selected.GetComponent<Selectable>().faceUp)
        {
            if (!Blocked(selected))
            {
                selected.GetComponent<Selectable>().faceUp = true;
                slot1 = this.gameObject;
            }
        }// Se la carta invece si trova nel mazzo
        else if(selected.GetComponent<Selectable>().inDeckPile)
        {
            if(!Blocked(selected))
            {
                slot1 = selected;
            }
        }

        if (slot1 == this.gameObject)
        {
            slot1 = selected;
        }
        else if (slot1 != selected)
        {
            if (Stackable(selected))
            {
                Stack(selected);
            }
            else
            {
                slot1 = selected;
            }
        }
    }

    void TopHandle(GameObject selected)
    {
        // Se la slot in alto è vuota e la carta selezionata è un asso, allora la carta può essere mossa
        if(slot1.CompareTag("Card"))
        {
            if(slot1.GetComponent<Selectable>().value == 1)
            {
                Stack(selected);
            }
        }
    }

    void BottomHandle(GameObject selected)
    {
        //Se la slot in basso è vuota e la prima carta è un K, allora la carta può essere mossa
        if(slot1.CompareTag("Card"))
        {
            if(slot1.GetComponent<Selectable>().value == 13)
            {
                Stack(selected);
            }
        }
    }


    // ************************

    bool Stackable(GameObject selected)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();

        if (!s2.inDeckPile)
        {
            if (s2.top)
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
                if (s1.value == s2.value - 1)
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
        }

        return false;
    }

    bool Blocked(GameObject selected)
    {
        Selectable s2 = selected.GetComponent<Selectable>();

        if (s2.inDeckPile)
        {
            if (s2.name == solitario.tripsOnDisplay.Last())
            {
                return false;
            }
            else
                return true;
        } else
        {
            if (s2.name == solitario.bottoms[s2.row].Last())
            {
                return false;
            }
            else
                return true;
        }
    }


    void Stack(GameObject selected)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();

        float yOffset = 0.2f;

        if(s2.top || !s2.top && s1.value == 13)
        {
            yOffset = 0f;
        }

        Vector3 target = new Vector3(selected.transform.position.x,
                                                selected.transform.position.y - yOffset,
                                                selected.transform.position.z - 0.1f);
        StartCoroutine(MoveCard(slot1, target));

        /*slot1.transform.position = new Vector3(selected.transform.position.x,
                                                selected.transform.position.y - yOffset,
                                                selected.transform.position.z - 0.1f);*/
        slot1.transform.parent = selected.transform;

        // Rimuove la carta dal mazzo
        if(s1.inDeckPile)
        {
            solitario.tripsOnDisplay.Remove(slot1.name);
        }
        else if(s1.top && s2.top && s1.value == 1) // Consente il movimento delle carte che si trovano in cima
        {
            solitario.topPos[s1.row].GetComponent<Selectable>().value = 0;
            solitario.topPos[s1.row].GetComponent<Selectable>().seme = null;
        }
        else if(s1.top)
        {
            solitario.topPos[s1.row].GetComponent<Selectable>().value = s1.value - 1;
        }
        else
        {
            solitario.bottoms[s1.row].Remove(slot1.name);
        }

        // non da la possibiità che le carte vengano aggiunte al tris di carte in alto
        s1.inDeckPile = false;
        s1.row = s2.row;


        if(s2.top)
        {
            solitario.topPos[s1.row].GetComponent<Selectable>().value = s1.value;
            solitario.topPos[s1.row].GetComponent<Selectable>().seme = s2.seme;
            s1.top = true;
        }
        else
        {
            s1.top = false;
        }

        slot1 = this.gameObject;

    }

    IEnumerator MoveCard(GameObject fromCard, Vector3 target)
    {
        float t = 0f;
        while(t < 2f)
        {
            fromCard.transform.position = Vector3.Lerp(fromCard.transform.position, target, t / 2f);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
