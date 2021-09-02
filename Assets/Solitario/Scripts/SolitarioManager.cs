using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Solitario.Params;

public class SolitarioManager : MonoBehaviour
{
    // Game status ***

    public int playerScore = 0;
    public int playerMoves = 0;

    // ****************
    // Public var ****

    public Sprite[] cardFaces;
    public GameObject cardPrefab;

    public GameObject deckButton;
    public GameObject[] bottomPos;
    public GameObject[] topPos;

    public static string[] semi = new string[] { "C", "Q", "F", "P" };
    public static string[] values = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

    public List<string> currentDeck;
    public List<string> discardPile = new List<string>();

    public List<string>[] bottoms;
    public List<string>[] tops;
    public List<string> tripsOnDisplay = new List<string>();
    public List<List<string>> deckTrips = new List<List<string>>();

    // ****************
    // Internal var ***

    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();

    private int trips;
    private int tripsRemainder;
    private int deckLocation;


    // Events ********

    public System.Action OnScoreUpdate = delegate { };
    public System.Action OnMovesUpdate = delegate { };

    // Classic void **************************************************************************



    void Start()
    {
        bottoms = new List<string>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };

        Play();
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        Solitario_Events.OnStackOnTop += StackOnTopHandle;
        Solitario_Events.OnStackOnBottom += StackOnBottomHandle;
        Solitario_Events.OnMoveDetected += MovesHandle;
    }

    private void OnDisable()
    {
        Solitario_Events.OnStackOnTop -= StackOnTopHandle;
        Solitario_Events.OnStackOnBottom -= StackOnBottomHandle;
        Solitario_Events.OnMoveDetected -= MovesHandle;
    }



    // Delegates *******************************************************************************


    void StackOnTopHandle()
    {
        playerScore += Solitario_Params.SCORE_TOPCARD;
        OnScoreUpdate();
    }

    void StackOnBottomHandle()
    {
        playerScore += Solitario_Params.SCORE_BOTTOMCARD;
        OnScoreUpdate();
    }

    void MovesHandle()
    {
        playerMoves += 1;
        OnMovesUpdate();
    }


    // Public void ************************************************************************************************************************************





    public void Play()
    {
        // Registra l'evento
        Solitario_Events.OnGameStart();

        // Crea il mazzo di carte
        currentDeck = CreateCards();
        // Mischia il mazzo di carte appena creato
        Shuffle(currentDeck);

        // Crea gli slot appositi dove le carte verranno piazzate "a scala" nel riquadro inferiore
        SortCards();
        // Instanziamento carte
        StartCoroutine(InstantiateCards());
        // Crea il mazzo di carte in alto
        SortMainDeck();
    }


    public void SortMainDeck()
    {
        trips = currentDeck.Count / 3;
        tripsRemainder = currentDeck.Count % 3;

        deckTrips.Clear();

        // Creazione del tris di carte
        int modifier = 0;
        for(int i=0; i<trips; i++)
        {
            List<string> myTrips = new List<string>();
            for(int j=0; j<3; j++)
            {
                myTrips.Add(currentDeck[j + modifier]);
            }
            deckTrips.Add(myTrips);
            modifier += 3;
        }

        // Gestione carte rimanenti dal tris
        if(tripsRemainder != 0)
        {
            List<string> myRemainders = new List<string>();
            modifier = 0;

            for(int x=0; x<tripsRemainder; x++)
            {
                myRemainders.Add(currentDeck[currentDeck.Count - tripsRemainder + modifier]);
                modifier++;
            }
            deckTrips.Add(myRemainders);
            trips++;
        }

        deckLocation = 0;
    }


    public void PickFromDeck()
    {
        // Aggiunge le carte rimanenti a quelle scartate ed elimina il tris di carte quando arriva alla fine
        foreach(Transform child in deckButton.transform)
        {
            if(child.tag == "Card")
            {
                currentDeck.Remove(child.name);
                discardPile.Add(child.name);
                Destroy(child.gameObject);
            }
        }


        // *******



        if(deckLocation < trips)
        {
            // mostra 3 carte dal mazzo
            tripsOnDisplay.Clear();

            float xOffset = Solitario_Params.CardOffset.xOffsetTrisCard;
            float zOffset = Solitario_Params.CardOffset.zOffset;

            foreach(string card in deckTrips[deckLocation])
            {
                GameObject topInstance = Instantiate(cardPrefab, new Vector3(deckButton.transform.position.x + xOffset, deckButton.transform.position.y, deckButton.transform.position.z + zOffset), Quaternion.identity, deckButton.transform);
                xOffset -= Solitario_Params.CardOffset.xOffset;
                zOffset -= 0.2f;

                topInstance.name = card;

                tripsOnDisplay.Add(card);

                topInstance.GetComponent<Selectable>().faceUp = true;
            }

            deckLocation++;
        }
        else
        {
            RestackTopDeck();
        }
    }



    /// <summary>
    /// Crea un nuovo mazzo di carte
    /// </summary>
    /// <returns>Ritorna ad una lista stringhe contente tutte le carte del mazzo francese</returns>
    public static List<string> CreateCards()
    {
        List<string> newdeck = new List<string>();

        foreach(string s in semi)
        {
            foreach(string v in values)
            {
                // Per ogni carta viene dato un nome specifico dato da "seme_valore"
                newdeck.Add($"{s}_{v}");
            }
        }

        return newdeck;
    }




    // Internal void ****************************************************************************************************************************************





    // Sistema per mischiare le carte non utilizzando il semplice random di unity
    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        while(n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    


    // Funzione per riporre le 52 carte nei vari slot inferiori
    void SortCards()
    {
        for(int i=0; i<7; i++)
        {
            for(int j=i; j<7; j++)
            {
                bottoms[j].Add(currentDeck.Last<string>());
                currentDeck.RemoveAt(currentDeck.Count - 1);
            }
        }
    }


    void RestackTopDeck()
    {
        currentDeck.Clear();

        foreach(string card in discardPile)
        {
            currentDeck.Add(card);
        }

        discardPile.Clear();
        SortMainDeck();

    }



    // Coroutines ***********************************************************************************************************************


    IEnumerator InstantiateCards()
    {
        for (int i = 0; i < 7; i++)
        {
            // Stabilisco un offset per non sovrapporre le carte
            float yOffset = 0.0f;
            float zOffset = 0.3f;

            foreach (string card in bottoms[i])
            {
                // Simulo un delay per non farle comparire tutte insieme
                yield return new WaitForSeconds(0.05f);

                // Instance 
                GameObject instance = Instantiate(cardPrefab,
                    new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity,
                    bottomPos[i].transform);

                // Stabilisco un nome preciso per la carta
                instance.name = card;

                // Setta la riga dove è posizionata la carta
                instance.GetComponent<Selectable>().row = i;

                // Se la carta è l'ultima della fila, la mostrerò scoperta. Le altre rimarranno coperte
                if (card == bottoms[i][bottoms[i].Count - 1])
                    instance.GetComponent<Selectable>().faceUp = true;

                // Incremento l'offset per la carta successiva
                yOffset += 0.1f;
                zOffset += 0.03f;

                discardPile.Add(card);
            }
        }


        foreach (string card in discardPile)
        {
            if (currentDeck.Contains(card))
                currentDeck.Remove(card);
        }
        discardPile.Clear();
    }


}
