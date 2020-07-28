using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    
    public int numOfDiamonds;
    public int numOfCoins;

    public Text textDiamonds;
    public Text textCoins;

    void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("DIAMONDS") || PlayerPrefs.HasKey("COINS"))
        {
            // WE HAD A PREVIOUS SESSION
            numOfCoins = PlayerPrefs.GetInt("COINS", 0);
            numOfDiamonds = PlayerPrefs.GetInt("DIAMONDS", 0);
        }
        else
        {
            SAVE_PREFS();
        }
    }

    void Start()
    {
        updateText_Diamonds();
        updateText_Coins();
    }

    public void updateText_Diamonds()
    {
        textDiamonds.text = numOfDiamonds.ToString();
    }

    public void updateText_Coins()
    {
        textCoins.text = numOfCoins.ToString();
    }

    public void SAVE_PREFS()
    {
        PlayerPrefs.SetInt("COINS", numOfCoins);
        PlayerPrefs.SetInt("DIAMONDS", numOfDiamonds);
    }

}
