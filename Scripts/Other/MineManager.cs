using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineManager : MonoBehaviour
{
    public static MineManager instance;

    public GameObject Mine;
    public Transform minePosition;

    public int numOfMines = 5;
    public Text txt_numOfMines;

    public Button MineButton;
    public Image minePlantVisualTimer;
    public int mineTimeLimit = 4;

    public AudioClip BOOM_SOUND;
    public AudioSource BOOM_SOURCE;

    void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("numOfMines"))
        {
            // we had a previous session
            numOfMines = PlayerPrefs.GetInt("numOfMines", 5);
        }
        else
        {
            SavePrefs();
        }
    }
    void Start()
    {
        minePlantVisualTimer.fillAmount = 1f;
        updateMineAmount();
    }

    void FixedUpdate()
    {
        HandleMineButton();
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("numOfMines", numOfMines);
    }

    public void PLAY_BOOM_SOUND()
    {
        BOOM_SOURCE.PlayOneShot(BOOM_SOUND);
    }

    public void updateMineAmount()
    {
        txt_numOfMines.text = numOfMines.ToString();
    }

    private void HandleMineButton()
    {
        if(numOfMines > 0 && Controller.bCanPlantMine && MineIsReady())
        {
            MineButton.interactable = true;
        }else { MineButton.interactable = false; }
    }

    public void PLANT_MINE()
    {
        StartCoroutine(USE_MINE_EFFECT());
    }

    IEnumerator USE_MINE_EFFECT()
    {
        numOfMines--;
        updateMineAmount();
        SavePrefs();

        Instantiate(Mine, minePosition.position, Quaternion.identity);

        minePlantVisualTimer.fillAmount = 0f;

        float timer = 0f;

        while(timer <= mineTimeLimit)
        {
            timer += Time.deltaTime;
            minePlantVisualTimer.fillAmount = timer / mineTimeLimit;
            yield return null;
        }
    }

    bool MineIsReady()
    {
        return minePlantVisualTimer.fillAmount >= 1;
    }
    

}
