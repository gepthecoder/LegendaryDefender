using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public WaveSpawner spawner;

    public Animator SettingsGuiAnime;
    public Animator gameSavedAnime;

    public Button saveBtn;
    public Button exitBtn;


    public void ShowSettings()
    {
        SettingsGuiAnime.SetBool("showSettings", true);
    }

    public void HideSettings()
    {
        SettingsGuiAnime.SetBool("showSettings", false);
    }

 
    public void SAVE_GAME()
    {
        if (spawner != null)
        {
            //save
            spawner.SavePrefs();
            gameSavedAnime.SetTrigger("gameSaved");
            saveBtn.interactable = false;
            exitBtn.interactable = false;
            StartCoroutine(saveEffect());
        }
    }

    IEnumerator saveEffect()
    {
        yield return new WaitForSeconds(2f);
        saveBtn.interactable = true;
        exitBtn.interactable = true;
    }

    public void EXIT_GAME() {
        SceneManager.LoadScene(0);
    }



}
