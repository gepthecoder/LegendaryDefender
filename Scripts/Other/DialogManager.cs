 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private Queue<string> sentences;

    public Text nameText;
    public Text dialogueText;

    public Text shop_nameText;
    public Text shop_dialogText;

    public Animator animePrincessTalk;
    public Animator animeShopTalk;

    public GameObject GUInterface;
    public GameObject PLAYER_CAM;
    public GameObject SHOWCASE_CAM;

    private bool bSkipIntro;

    public GameObject skipButton;
    protected bool playerControlsEnabled;

    public ShopManager shopManager;

    void Start()
    {
        sentences = new Queue<string>();
        skipButton.SetActive(true);
        playerControlsEnabled = false;
    }

    void Update()
    {
        if (!playerControlsEnabled)
        {
            if (bSkipIntro) { Time.timeScale = 4f; } else { if (Time.timeScale != 1) { Time.timeScale = 1; } }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        bSkipIntro = false;
        skipButton.SetActive(false);


        animePrincessTalk.SetBool("showDialogue", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void StartShopDialogue(Dialogue dialogue)
    {
        animeShopTalk.SetBool("showShopDialogue", true);
        shop_nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextShopSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void DisplayNextShopSentence()
    {
        if (sentences.Count == 0)
        {
            EndShopDialog();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeShopSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    IEnumerator TypeShopSentence(string sentence)
    {
        shop_dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            shop_dialogText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        playerControlsEnabled = true;
        animePrincessTalk.SetBool("showDialogue", false);
        GUInterface.SetActive(true);
        PLAYER_CAM.SetActive(true);
        SHOWCASE_CAM.SetActive(false);
        PlayerManager.instance.canChase = true;
        WaveSpawner.instance.animateWaveNameText = true;
        QueenLogic.instance.canGoToDestination = true;
        Debug.Log("Dialogue ended!!");
    }

    void EndShopDialog()
    {
        PLAYER_CAM.SetActive(false);
        animeShopTalk.SetBool("showShopDialogue", false);
        shopManager.ShowShop();
    }

    public void SKIP_INTRO()
    {
        skipButton.GetComponent<Button>().interactable = false;
        bSkipIntro = true;
    }

}
