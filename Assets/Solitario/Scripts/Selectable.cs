using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool faceUp = false;

    public bool top = false;
    public string seme;
    public int value;
    public int row;
    public bool inDeckPile = false;

    private string valueString;

    void Start()
    {
        if(CompareTag("Card"))
        {
            seme = transform.name[0].ToString();

            for(int i=2; i<transform.name.Length; i++)
            {
                char c = transform.name[i];
                valueString = valueString + c.ToString();
            }

            switch(valueString)
            {
                case "A":
                    value = 1;
                    break;

                case "2":
                    value = 2;
                    break;

                case "3":
                    value = 3;
                    break;

                case "4":
                    value = 4;
                    break;

                case "5":
                    value = 5;
                    break;

                case "6":
                    value = 6;
                    break;

                case "7":
                    value = 7;
                    break;

                case "8":
                    value = 8;
                    break;

                case "9":
                    value = 9;
                    break;

                case "10":
                    value = 10;
                    break;

                case "J":
                    value = 11;
                    break;

                case "Q":
                    value = 12;
                    break;

                case "K":
                    value = 13;
                    break;
            }
        }
    }


    void Update()
    {
        
    }
}
