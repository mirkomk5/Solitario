using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitarioManager : MonoBehaviour
{
    public static string[] semi = new string[] { "Cuori", "Quadri", "Fiori", "Picche" };
    public static string[] values = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

    public List<string> currentDeck;

    // Start is called before the first frame update
    void Start()
    {
        Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Play()
    {
        currentDeck = CreateCards();
        Shuffle(currentDeck);
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
                newdeck.Add($"{s}_{v}");
            }
        }

        return newdeck;
    }

    // Internal void *****************************************************************

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
}
