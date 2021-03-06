using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SolitarioManager : MonoBehaviour
{
    // ****************
    // Public var ****


    [Tooltip("Inserisci le sprite di tutte le carte del mazzo")]
    public Sprite[] cardFaces;

    [Tooltip("Inserisci il prefab della carta da gioco")]
    public GameObject cardPrefab;

    [Tooltip("Inserisci il gameObject della carta che consente il pescaggio dal mazzo")]
    public GameObject deckButton;

    [Tooltip("Inserisci tutti gli spazi vuoti della parte inferiore del tavolo di gioco")]
    public GameObject[] bottomPos;

    [Tooltip("Inserisci tutti gli spazi vuoti della parte superiore del tavolo di gioco")]
    public GameObject[] topPos;


    // ****

    [HideInInspector] public int playerScore = 0;
    [HideInInspector] public int playerMoves = 0;
    [HideInInspector] public float playerTime = 0f;


    public static string[] semi = new string[] { "C", "Q", "F", "P" };
    public static string[] values = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

    [HideInInspector] public List<string> currentDeck;
    [HideInInspector] public List<string> discardPile = new List<string>();


    public List<string>[] bottoms;
    public List<string>[] tops;
    [HideInInspector] public List<string> tripsOnDisplay = new List<string>();
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

    public List<GameObject> topStack = new List<GameObject> ();

    private int trips;
    private int tripsRemainder;
    private int deckLocation;

    private bool gameIsRunning = false;


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
        // Crono 
        if (gameIsRunning)
            playerTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        Solitario_Events.OnStackOnTop += StackOnTopHandle;
        Solitario_Events.OnStackOnBottom += StackOnBottomHandle;
        Solitario_Events.OnMoveDetected += MovesHandle;
        Solitario_Events.OnCardMovedOnFinalStack += GameWinHandle;
    }

    private void OnDisable()
    {
        Solitario_Events.OnStackOnTop -= StackOnTopHandle;
        Solitario_Events.OnStackOnBottom -= StackOnBottomHandle;
        Solitario_Events.OnMoveDetected -= MovesHandle;
        Solitario_Events.OnCardMovedOnFinalStack -= GameWinHandle;
    }



    // Delegates *******************************************************************************


    void StackOnTopHandle()  // Evento chiamato quando si procede allo stack con le carte dell'area superiore
    {
        playerScore += Solitario_Params.SCORE_TOPCARD;
        OnScoreUpdate();
    }

    void StackOnBottomHandle() // Evento chiamato quando si procede allo stack con le carte dell'area inferiore
    {
        playerScore += Solitario_Params.SCORE_BOTTOMCARD;
        OnScoreUpdate();
    }

    void MovesHandle()   // Evento chiamato ogni volta che il player faccia una mossa
    {
        playerMoves += 1;
        OnMovesUpdate();
    }

    void GameWinHandle()
    {
        if (topStack.Count == Solitario_Params.MAX_CARDS)
        {
            Solitario_Events.OnPlayerWin();
            SetGameStatus(false);
        }

        Debug.Log($"Carte stackate: {topStack.Count}");
    }


    // Public void ************************************************************************************************************************************




    /// <summary>
    /// Avvia il gioco
    /// </summary>
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

        gameIsRunning = true;
    }

    

    /// <summary>
    /// Setta lo stato attuale del gioco
    /// </summary>
    /// <param name="running">il gioco ? in esecuzione?</param>
    public void SetGameStatus(bool running)
    {
        gameIsRunning = running;
    }


    /// <summary>
    /// Procede alla mescola delle carte che verranno mostrate nella parte superiore
    /// </summary>
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



    /// <summary>
    /// Genera il tris di carte che viene mostrato in alto
    /// </summary>
    public void PickFromDeck()
    {
        // Rimuove le instanze dei tris prima di mostrare quelle successive
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


        // Se ci sono ancora carte nel mazzo, procede a mostrarne altre
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
                zOffset -= Solitario_Params.CardOffset.zOffset;

                topInstance.name = card;

                tripsOnDisplay.Add(card);

                topInstance.GetComponent<Selectable>().faceUp = true;
                
                // Marca la carta come inDeckPile per differenziarla dal resto delle carte presenti nella parte inferiore
                topInstance.GetComponent<Selectable>().inDeckPile = true;
            }

            deckLocation++;

        }
        else // Se le carte del mazzo sono finite, riprende dall'inizio
        {
            RestackTopDeck();

            // Ogni volta che il player ha terminato il giro di tris di carte, il punteggio viene riazzerato (Ps. non conoscevo questa regola :) )
            playerScore = 0;
            // Tutte le voci esterne collegate a questo evento vengono ad essere richiamate
            OnScoreUpdate();
        }
    }


    /// <summary>
    /// Metodo customizzato per la funzione Undo.
    /// </summary>
    /// <param name="deckId"></param>
    public void PickFromDeckAt(int deckId)
    {

        foreach (Transform child in deckButton.transform)
        {
            if (child.tag == "Card")
            {
                currentDeck.Remove(child.name);
                discardPile.Add(child.name);
                Destroy(child.gameObject);
            }
        }

        if (deckId < trips)
        {
            // mostra 3 carte dal mazzo
            tripsOnDisplay.Clear();

            float xOffset = Solitario_Params.CardOffset.xOffsetTrisCard;
            float zOffset = Solitario_Params.CardOffset.zOffset;

            foreach (string card in deckTrips[deckId])
            {
                GameObject topInstance = Instantiate(cardPrefab, new Vector3(deckButton.transform.position.x + xOffset, deckButton.transform.position.y, deckButton.transform.position.z + zOffset), Quaternion.identity, deckButton.transform);
                xOffset -= Solitario_Params.CardOffset.xOffset;
                zOffset -= Solitario_Params.CardOffset.zOffset;

                topInstance.name = card;

                tripsOnDisplay.Add(card);

                topInstance.GetComponent<Selectable>().faceUp = true;

                // Marca la carta come inDeckPile per differenziarla dal resto delle carte presenti nella parte inferiore
                topInstance.GetComponent<Selectable>().inDeckPile = true;
            }

            deckLocation--;
        }
        else
        {
            RestackTopDeck();
            playerScore = 0;
            OnScoreUpdate();
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


    /// <summary>
    /// Ritorna alla posizione del tris di carte
    /// </summary>
    /// <returns></returns>
    public int GetDeckLocation()
    {
        return deckLocation;
    }


    // Internal void ****************************************************************************************************************************************





    /// <summary>
    /// Metodo per mescolare le carte del mazzo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
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

    


    /// <summary>
    /// Ripone le carte del mazzo nei vari slot inferiori
    /// </summary>
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



    /// <summary>
    /// Rimescola le carte rimanenti nel mazzo
    /// </summary>
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



    public void GetPlayerTime(out int min, out int sec)
    {
        min = (int)playerTime / 60;
        sec = (int)playerTime - (min * 60);
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

                // Instance carta
                Vector3 target = new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset);
                GameObject instance = Instantiate(cardPrefab, deckButton.transform.position, Quaternion.identity);
                
                // Transizione carta
                StartCoroutine(MoveCard(instance, target));

                // Inserisce la carta all'interno della sua gerarchia corrispondente
                instance.transform.SetParent(bottomPos[i].transform);

                // Old instance (unused)
                /*GameObject instance = Instantiate(cardPrefab,
                    new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity,
                    bottomPos[i].transform);*/

                // Stabilisco un nome preciso per la carta
                instance.name = card;

                // Setta la riga dove ? posizionata la carta
                instance.GetComponent<Selectable>().row = i;

                // Se la carta ? l'ultima della fila, la mostrer? scoperta. Le altre rimarranno coperte
                if (card == bottoms[i][bottoms[i].Count - 1])
                    instance.GetComponent<Selectable>().faceUp = true;

                // Incremento l'offset per la carta successiva
                yOffset += 0.1f;
                zOffset += 0.03f;

                discardPile.Add(card);

                // Registra l'evento
                Solitario_Events.OnCardTransition();
            }
        }


        foreach (string card in discardPile)
        {
            if (currentDeck.Contains(card))
                currentDeck.Remove(card);
        }
        discardPile.Clear();
    }




    IEnumerator MoveCard(GameObject fromCard, Vector3 target)
    {
        float t = 0f;
        while (t < 1f)
        {
            fromCard.transform.position = Vector3.Lerp(fromCard.transform.position, target, t / 1f);
            t += Time.deltaTime;
            yield return null;
        }
    }


}
