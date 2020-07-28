using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject playerCam;
    public GameObject shopCam;
    public Animator shopAnime;

    public Controller playerControls;
    public GameObject GUI_Controls;

    public bool _isAvailable;

    public GameObject shopTriggerEffect;
    public GameObject triggerShopCollider;

    //  SHOP SETTINGS
    public Transform[] spawnItemPositions;

    //  ITEMS
    public GameObject Item_HEALTH;
    public GameObject Item_SHIELD;
    public GameObject Item_BOMB;
    public GameObject Item_KEY;
    public GameObject Item_AXE;
    //  JETPACKS..
    public GameObject Item_BLUE_THUNDER_JETPACK;
    public GameObject Item_GREEN_MAMBA_JETPACK;
    public GameObject Item_GOLDEN_JUICE_JETPACK;
    public GameObject Item_CONSERVATIVE_JETPACK;
    public GameObject Item_DOUBLE_FURIOUS_JETPACK;
    public GameObject Item_SUPER_TURBO_JETPACK;
    //  MONEY
    public Text numOfCoins;
    public Text numOfDiamonds;
    //  BUTTONS
    public Button USE_BLUE_THUNDER_BTN;
    public Button USE_BUY_GREEN_MAMBA_BTN;
    public Button USE_BUY_GOLDEN_JUICE_BTN;
    public Button USE_BUY_CONSERVATIVE_BTN;
    public Button USE_BUY_DOUBLE_FURIOUS_BTN;
    public Button USE_BUY_SUPER_TURBO_BTN;
    // BOUGHT ITEMS -> 1 bought | 0 not bought
    protected int greenMambaBought;
    protected int goldenJuiceBought;
    protected int conservativeBought;
    protected int doubleFuriousBought;
    protected int superTurboBought;
    // BUTTON INTERACTABILLITY
    public GameObject GM_priceImg;
    public GameObject GM_useTextImg;

    public GameObject GJ_priceImg;
    public GameObject GJ_useTextImg;

    public GameObject C_priceImg;
    public GameObject C_useTextImg;

    public GameObject DF_priceImg;
    public GameObject DF_useTextImg;

    public GameObject ST_priceImg;
    public GameObject ST_useTextImg;

    void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("greenMambaBought"))
        {
            // we had a previous session
            greenMambaBought    = PlayerPrefs.GetInt("greenMambaBought", 0);
            goldenJuiceBought   = PlayerPrefs.GetInt("goldenJuiceBought", 0);
            conservativeBought  = PlayerPrefs.GetInt("conservativeBought", 0);
            doubleFuriousBought = PlayerPrefs.GetInt("doubleFuriousBought", 0);
            superTurboBought    = PlayerPrefs.GetInt("superTurboBought", 0);

        }
        else
        {
            SavePrefs();
        }
    }


    void Update()
    {
        if (_isAvailable)
        {
            if(!shopTriggerEffect.activeSelf)
                shopTriggerEffect.SetActive(true);

            triggerShopCollider.SetActive(true);
        }
        else
        {
            if (shopTriggerEffect.activeSelf)
                shopTriggerEffect.SetActive(false);

            triggerShopCollider.SetActive(false);
        }

        if (playerControls._inShop)
        {
            set_text_coins();
            set_text_diamonds();

            HandleButtons();
        }
    }

    private void SavePrefs()
    {
        PlayerPrefs.SetInt("greenMambaBought", greenMambaBought);
        PlayerPrefs.SetInt("goldenJuiceBought", goldenJuiceBought);
        PlayerPrefs.SetInt("conservativeBought", conservativeBought);
        PlayerPrefs.SetInt("doubleFuriousBought", doubleFuriousBought);
        PlayerPrefs.SetInt("superTurboBought", superTurboBought);
    }

    public void HandleButtons()
    {      

        if (greenMambaBought == 1)
        {
            USE_BLUE_THUNDER_BTN.interactable = true;

            if (PlayerPrefs.GetInt("jetpack", 0) != (int)JetPackManager.JETPACKS.greenMamba)
            {
                // use -> active
                USE_BUY_GREEN_MAMBA_BTN.interactable = true;
                GM_priceImg.SetActive(false);
                GM_useTextImg.SetActive(true);
            }
            else
            {
                USE_BUY_GREEN_MAMBA_BTN.interactable = false;
                GM_priceImg.SetActive(false);
                GM_useTextImg.SetActive(true);
            }
        }

        if (goldenJuiceBought == 1)
        {
            USE_BLUE_THUNDER_BTN.interactable = true;

            if (PlayerPrefs.GetInt("jetpack", 0) != (int)JetPackManager.JETPACKS.goldenJuice)
            {
                // use -> active
                USE_BUY_GOLDEN_JUICE_BTN.interactable = true;
                GJ_priceImg.SetActive(false);
                GJ_useTextImg.SetActive(true);
            }
            else
            {
                USE_BUY_GOLDEN_JUICE_BTN.interactable = false;
                GJ_priceImg.SetActive(false);
                GJ_useTextImg.SetActive(true);
            }
        }

        if (conservativeBought == 1)
        {
            USE_BLUE_THUNDER_BTN.interactable = true;

            if (PlayerPrefs.GetInt("jetpack", 0) != (int)JetPackManager.JETPACKS.conservative)
            {
                // use -> active
                USE_BUY_CONSERVATIVE_BTN.interactable = true;
                C_priceImg.SetActive(false);
                C_useTextImg.SetActive(true);
            }
            else
            {
                USE_BUY_CONSERVATIVE_BTN.interactable = false;
                C_priceImg.SetActive(false);
                C_useTextImg.SetActive(true);
            }
        }

        if (doubleFuriousBought == 1)
        {
            USE_BLUE_THUNDER_BTN.interactable = true;

            if (PlayerPrefs.GetInt("jetpack", 0) != (int)JetPackManager.JETPACKS.doubleFurious)
            {
                // use -> active
                USE_BUY_DOUBLE_FURIOUS_BTN.interactable = true;
                DF_priceImg.SetActive(false);
                DF_useTextImg.SetActive(true);
            }
            else
            {
                USE_BUY_DOUBLE_FURIOUS_BTN.interactable = false;
                DF_priceImg.SetActive(false);
                DF_useTextImg.SetActive(true);
            }
        }

        if (superTurboBought == 1)
        {
            USE_BLUE_THUNDER_BTN.interactable = true;

            if (PlayerPrefs.GetInt("jetpack", 0) != (int)JetPackManager.JETPACKS.superTurbo)
            {
                // use -> active
                USE_BUY_SUPER_TURBO_BTN.interactable = true;
                ST_priceImg.SetActive(false);
                ST_useTextImg.SetActive(true);
            }
            else
            {
                USE_BUY_SUPER_TURBO_BTN.interactable = false;
                ST_priceImg.SetActive(false);
                ST_useTextImg.SetActive(true);
            }
        }
    }


    public void ShowShop()
    {
        Debug.Log("SHOW SHOP!!");
        shopAnime.SetTrigger("showShop");
    }

    public void HideShop()
    {
        Debug.Log("HIDE SHOP!");
        shopAnime.SetTrigger("hideShop");
    }

    // button on click
    public void CloseShopGUI()
    {
        HideShop();

        playerControls.Ljoystick.input.x = 0;
        playerControls.Ljoystick.input.y = 0;

        GUI_Controls.SetActive(true);
        shopCam.SetActive(false);
        playerCam.SetActive(true);
        playerControls._inShop = false;
        ShopTrigger.instance.timer = 5f;
        Debug.Log("CLOSE SHOP GUII COMPLETE!!");

    }

    public void BUY_ITEM_HEALTH()
    {
        int goldCoin_price = 10;

        if(CurrencyManager.instance.numOfCoins >= goldCoin_price)
        {
            //TO:DO --> sound effect -> bought

            // sell item
            CurrencyManager.instance.numOfCoins -= goldCoin_price;
            CurrencyManager.instance.SAVE_PREFS();
            CurrencyManager.instance.updateText_Coins();

            set_text_coins();

            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_HEALTH, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("Shortage of coins :(");
        }
    }

    public void BUY_ITEM_SHIELD()
    {
        int goldCoin_price = 12;

        if (CurrencyManager.instance.numOfCoins >= goldCoin_price)
        {
            //TO:DO --> sound effect -> bought

            // sell item
            CurrencyManager.instance.numOfCoins -= goldCoin_price;
            CurrencyManager.instance.SAVE_PREFS();
            CurrencyManager.instance.updateText_Coins();

            set_text_coins();

            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_SHIELD, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("Shortage of coins :(");
        }
    }

    public void BUY_ITEM_BOMB()
    {
        int goldCoin_price = 15;

        if (CurrencyManager.instance.numOfCoins >= goldCoin_price)
        {
            //TO:DO --> sound effect -> bought

            // sell item
            CurrencyManager.instance.numOfCoins -= goldCoin_price;
            CurrencyManager.instance.SAVE_PREFS();
            CurrencyManager.instance.updateText_Coins();

            set_text_coins();

            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_BOMB, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("Shortage of coins :(");
        }
    }


    public void BUY_ITEM_AXE()
    {
        int goldCoin_price = 15;

        if (CurrencyManager.instance.numOfCoins >= goldCoin_price)
        {
            //TO:DO --> sound effect -> bought

            // sell item
            CurrencyManager.instance.numOfCoins -= goldCoin_price;
            CurrencyManager.instance.SAVE_PREFS();
            CurrencyManager.instance.updateText_Coins();

            set_text_coins();

            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_AXE, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("Shortage of coins :(");
        }
    }

    public void BUY_ITEM_KEY()
    {
        int diamondPrice = 1;

        if (CurrencyManager.instance.numOfDiamonds >= diamondPrice)
        {
            //TO:DO --> sound effect -> bought

            // sell item
            CurrencyManager.instance.numOfDiamonds -= diamondPrice;
            CurrencyManager.instance.SAVE_PREFS();
            CurrencyManager.instance.updateText_Diamonds();

            set_text_diamonds();

            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_KEY, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("Shortage of coins :(");
        }
    }

    public void SPAWN_ITEM_BLUE_THUNDER_JETPACK()
    {
        if (JetPackManager.Instance.currentJetpack != (int)JetPackManager.JETPACKS.blueThunder)
        {
            //TO:DO --> sound effect -> bought
            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_BLUE_THUNDER_JETPACK, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("You got it already booi");
        }
    }

    public void BUY_ITEM_GREEN_MAMBA_JETPACK()
    {
        int diamondPrice = 10;

        if (CurrencyManager.instance.numOfDiamonds >= diamondPrice)
        {
            //TO:DO --> sound effect -> bought

            // sell item
            CurrencyManager.instance.numOfDiamonds -= diamondPrice;
            CurrencyManager.instance.SAVE_PREFS();
            CurrencyManager.instance.updateText_Diamonds();

            set_text_diamonds();

            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_GREEN_MAMBA_JETPACK, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("Shortage of coins :(");
        }
    }

    public void BUY_ITEM_GOLDEN_JUICE_JETPACK()
    {
        int diamondPrice = 20;

        if (CurrencyManager.instance.numOfDiamonds >= diamondPrice)
        {
            //TO:DO --> sound effect -> bought

            // sell item
            CurrencyManager.instance.numOfDiamonds -= diamondPrice;
            CurrencyManager.instance.SAVE_PREFS();
            CurrencyManager.instance.updateText_Diamonds();

            set_text_diamonds();

            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_GOLDEN_JUICE_JETPACK, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("Shortage of coins :(");
        }
    }


    public void BUY_ITEM_CONSERVATIVE_JETPACK()
    {
        int diamondPrice = 35;

        if (CurrencyManager.instance.numOfDiamonds >= diamondPrice)
        {
            //TO:DO --> sound effect -> bought

            // sell item
            CurrencyManager.instance.numOfDiamonds -= diamondPrice;
            CurrencyManager.instance.SAVE_PREFS();
            CurrencyManager.instance.updateText_Diamonds();

            set_text_diamonds();

            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_CONSERVATIVE_JETPACK, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("Shortage of coins :(");
        }
    }

    public void BUY_ITEM_DOUBLE_FURIOUS_JETPACK()
    {
        int diamondPrice = 50;

        if (CurrencyManager.instance.numOfDiamonds >= diamondPrice)
        {
            //TO:DO --> sound effect -> bought

            // sell item
            CurrencyManager.instance.numOfDiamonds -= diamondPrice;
            CurrencyManager.instance.SAVE_PREFS();
            CurrencyManager.instance.updateText_Diamonds();

            set_text_diamonds();

            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_DOUBLE_FURIOUS_JETPACK, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("Shortage of coins :(");
        }
    }

    public void BUY_ITEM_SUPER_TURBO_JETPACK()
    {
        int diamondPrice = 65;

        if (CurrencyManager.instance.numOfDiamonds >= diamondPrice)
        {
            //TO:DO --> sound effect -> bought

            // sell item
            CurrencyManager.instance.numOfDiamonds -= diamondPrice;
            CurrencyManager.instance.SAVE_PREFS();
            CurrencyManager.instance.updateText_Diamonds();

            set_text_diamonds();

            //spawn item
            int r = Random.Range(0, spawnItemPositions.Length);
            Instantiate(Item_SUPER_TURBO_JETPACK, spawnItemPositions[r], false);
        }
        else
        {
            //TO:DO --> sound effect -> not enough money
            Debug.Log("Shortage of coins :(");
        }
    }


    public void set_text_coins()
    {
        numOfCoins.text = CurrencyManager.instance.numOfCoins.ToString();
    }

    public void set_text_diamonds()
    {
        numOfDiamonds.text = CurrencyManager.instance.numOfDiamonds.ToString();
    }

}
