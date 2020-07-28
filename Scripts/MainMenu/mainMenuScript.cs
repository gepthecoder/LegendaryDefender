using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenuScript : MonoBehaviour
{
    public Button PlayButton;
    public Image fillAmountImg;

    float waitTime = 4f;

    void Start()
    {
        fillAmountImg.fillAmount = 0f;
    }

    public void PLAY_GAME()
    {
        // START LOADING BAR
        PlayButton.interactable = false;
        StartCoroutine(StartLoading());
    }

    IEnumerator StartLoading()
    {
        fillAmountImg.fillAmount = 0f;

        float timer = 0;

        while (timer <= waitTime)
        {
            timer += Time.deltaTime;
            fillAmountImg.fillAmount = timer / waitTime;
            yield return null;
        }

        if(timer >= waitTime)
        {
            LoadGame();
        }
    }

    void LoadGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
