using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollectableItem : MonoBehaviour
{
    public string Name;
    // AUDIO CLIPS
    public AudioClip pickUpSound;
    //
    private AudioSource aSource;

    private float destroyTime;

    bool rewardGiven;

    void Awake()
    {
        aSource = GetComponent<AudioSource>();
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.gameObject.tag == "Player")
        {
            if (!rewardGiven)
            {
                PLAY(pickUpSound);
                COLLECT_REWARD(Name);
            }
            Destroy(gameObject, destroyTime);
        }
    }

    void PLAY(AudioClip clip) {
        destroyTime = clip.length;
        aSource.PlayOneShot(clip); }

    void COLLECT_REWARD(string rewardName)
    {
        switch (rewardName)
        {
            case "Axe":
                PlacementController.instance.numOfPlacableObjects++;
                PlacementController.instance.updateTextAmount();
                PlacementController.instance.SavePrefs();
                break;
            case "Bomb":
                MineManager.instance.numOfMines++;
                MineManager.instance.updateMineAmount();
                MineManager.instance.SavePrefs();
                break;
            case "Coin":
                CurrencyManager.instance.numOfCoins++;
                CurrencyManager.instance.updateText_Coins();
                CurrencyManager.instance.SAVE_PREFS();
                break;
            case "Diamond":
                CurrencyManager.instance.numOfDiamonds++;
                CurrencyManager.instance.updateText_Diamonds();
                CurrencyManager.instance.SAVE_PREFS();
                break;
            case "Heart":
                PlayerHealth.instance.INCREASE_HEALTH(30);
                PlayerHealth.instance.SavePrefs();
                break;
            case "Key":
                Debug.Log("YOU GOT A KEY!!");
                break;
            case "Shield":
                SkillManager.instance.numOfShield++;
                SkillManager.instance.updateShieldAmountText();
                SkillManager.instance.SavePrefs();
                break;
            case "greenMamba":
                JetPackManager.Instance.START_SET_JETPACK((int)JetPackManager.JETPACKS.greenMamba);
                PlayerPrefs.SetInt("jetpack", (int)JetPackManager.JETPACKS.greenMamba);
                break;
            case "blueThunder":
                JetPackManager.Instance.START_SET_JETPACK((int)JetPackManager.JETPACKS.blueThunder);
                PlayerPrefs.SetInt("jetpack", (int)JetPackManager.JETPACKS.blueThunder);
                break;
            case "goldenJuice":
                JetPackManager.Instance.START_SET_JETPACK((int)JetPackManager.JETPACKS.goldenJuice);
                PlayerPrefs.SetInt("jetpack", (int)JetPackManager.JETPACKS.goldenJuice);
                break;
            case "conservative":
                JetPackManager.Instance.START_SET_JETPACK((int)JetPackManager.JETPACKS.conservative);
                PlayerPrefs.SetInt("jetpack", (int)JetPackManager.JETPACKS.conservative);
                break;
            case "doubleFurious":
                JetPackManager.Instance.START_SET_JETPACK((int)JetPackManager.JETPACKS.doubleFurious);
                PlayerPrefs.SetInt("jetpack", (int)JetPackManager.JETPACKS.doubleFurious);
                break;
            case "superTurbo":
                JetPackManager.Instance.START_SET_JETPACK((int)JetPackManager.JETPACKS.superTurbo);
                PlayerPrefs.SetInt("jetpack", (int)JetPackManager.JETPACKS.superTurbo);
                break;
        }

        rewardGiven = true;
    }
}
