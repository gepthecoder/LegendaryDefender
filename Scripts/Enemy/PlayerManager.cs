using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
        canChase = false;
    }

    #endregion

    public GameObject player;
    public GameObject princess;

    public bool canChase;

    [Header("COMBO ANIMATORS")]
    [Space(10)]
    [SerializeField ]private Animator GOOD;
    [Space(5)]
    [SerializeField] private Animator GREAT;
    [Space(5)]
    [SerializeField] private Animator AWESOME;
    [Space(5)]
    [SerializeField] private Animator NICE;
    [Space(5)]
    [SerializeField] private Animator WOW;
    [Space(5)]
    [SerializeField] private Animator COMBO;

    public void PLAY_GOOD()
    {
        GOOD.SetTrigger("GOOD");
    }

    public void PLAY_GREAT()
    {
        GREAT.SetTrigger("GREAT");
    }

    public void PLAY_AWESOME()
    {
        AWESOME.SetTrigger("AWESOME");
    }
    public void PLAY_NICE()
    {
        NICE.SetTrigger("NICE");
    }
    public void PLAY_WOW()
    {
        WOW.SetTrigger("WOW");
    }
    public void PLAY_COMBO()
    {
        COMBO.SetTrigger("COMBO");
    }

    public void PLAY_APPROPRIATE_COMBO(int lastDamageGiven)
    {
        Debug.Log("Last damage given: " + lastDamageGiven);

        if(lastDamageGiven > 80)
        {
            PLAY_WOW();
        }

        else if(lastDamageGiven <= 80 && lastDamageGiven > 50)
        {
            //last shot headshot -> AWESOME && GREAT
            bool temp = Random.Range(0, 100) >= 50 ? true : false;
            if (temp) { PLAY_AWESOME(); } else { PLAY_GREAT(); }
        }
        else if(lastDamageGiven <= 50 && lastDamageGiven > 20)
        {
            //last shot torso -> NICE
            PLAY_NICE();
        }

        else { PLAY_GOOD(); }
    }
}
