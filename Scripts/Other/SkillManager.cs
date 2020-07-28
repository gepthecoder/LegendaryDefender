using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Image SKILL_01_IMAGE;
    public Image SKILL_02_IMAGE;
    public Image SKILL_03_IMAGE;

    private Button skill_01_btn;
    private Button skill_02_btn;
    private Button skill_03_btn;

    private Animator skill_01_anime;
    private Animator skill_02_anime;
    private Animator skill_03_anime;


    [SerializeField] private float SKILL_01_TIMER = 5f;
    [SerializeField] private float SKILL_02_TIMER = 10f;
    [SerializeField] private float SKILL_03_TIMER = 20f;


    public bool bSKILL_01_ACTIVATED;
    public bool bSKILL_02_ACTIVATED;
    public bool bSKILL_03_ACTIVATED;


    public bool bFireTripleArrow;
    public bool bFireHighArrow;
    public bool bFireCloneArrow;

    public bool controllerAllowsToActivateSkill;


    /// <summary>
    ///  DEFENSE SHIELD EFFECT
    /// </summary>
    /// 

    public GameObject defenseEffect;
    public Transform player;

    public Button shieldButton;
    public Image shieldImageVisualTimer;
    public int shieldUseTimeLimit = 30;
    public int numOfShield = 2;
    public Text textShield;


    void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("numOfShield"))
        {
            // we had a previous session
            numOfShield = PlayerPrefs.GetInt("numOfShield", 2);
        }
        else
        {
            SavePrefs();
        }
    }

    void Start()
    {
        skill_01_btn = SKILL_01_IMAGE.GetComponent<Button>();
        skill_02_btn = SKILL_02_IMAGE.GetComponent<Button>();
        skill_03_btn = SKILL_03_IMAGE.GetComponent<Button>();


        skill_01_anime = SKILL_01_IMAGE.GetComponent<Animator>();
        skill_02_anime = SKILL_02_IMAGE.GetComponent<Animator>();
        skill_03_anime = SKILL_03_IMAGE.GetComponent<Animator>();


        SKILL_01_IMAGE.fillAmount = 1f;
        SKILL_02_IMAGE.fillAmount = 1f;
        SKILL_03_IMAGE.fillAmount = 1f;

        shieldImageVisualTimer.fillAmount = 1f;

        updateShieldAmountText();
    }

    void Update()
    {
        ManageSkill01Button();
        ManageSkill02Button();
        ManageSkill03Button();

        HandleShieldButton();
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("numOfShield", numOfShield);
    }

    public void updateShieldAmountText()
    {
        textShield.text = numOfShield.ToString();
    }

    public void USE_SHIELD_EFFECT()
    {
        numOfShield--;
        updateShieldAmountText();
        SavePrefs();
        Instantiate(defenseEffect, player, false);
        StartCoroutine(shield_effect());
    }

    IEnumerator shield_effect()
    {
        shieldImageVisualTimer.fillAmount = 0f;


        float timer = 0f;

        while (timer <= shieldUseTimeLimit)
        {
            timer += Time.deltaTime;
            shieldImageVisualTimer.fillAmount = timer / shieldUseTimeLimit;
            yield return null;
        }
    }

    private void HandleShieldButton()
    {
        if(ShieldReady() && Controller.bCanUseShield && numOfShield > 0)
        {
            shieldButton.interactable = true;
        }
        else { shieldButton.interactable = false; }
    }

    bool ShieldReady()
    {
        return shieldImageVisualTimer.fillAmount >= 1;
    }


    private void ManageSkill01Button()
    {
        if (SKILL_01_IMAGE.fillAmount == 1f && controllerAllowsToActivateSkill && !bSKILL_02_ACTIVATED && !bSKILL_03_ACTIVATED && !WaveSpawner.waveComplited)
        {
            skill_01_btn.interactable = true;
            // start anime
            skill_01_anime.SetBool("skillActive", true);
        }
        else { skill_01_btn.interactable = false; }
    }

    private void ManageSkill02Button()
    {
        if (SKILL_02_IMAGE.fillAmount == 1f && controllerAllowsToActivateSkill && !bSKILL_01_ACTIVATED && !bSKILL_03_ACTIVATED && !WaveSpawner.waveComplited)
        {
            skill_02_btn.interactable = true;
            // start anime
            skill_02_anime.SetBool("skillActive", true);
        }
        else { skill_02_btn.interactable = false; }
    }

    private void ManageSkill03Button()
    {
        if (SKILL_03_IMAGE.fillAmount == 1f && controllerAllowsToActivateSkill && !bSKILL_01_ACTIVATED && !bSKILL_02_ACTIVATED && !WaveSpawner.waveComplited)
        {
            skill_03_btn.interactable = true;
            // start anime
            skill_03_anime.SetBool("skillActive", true);
        }
        else { skill_03_btn.interactable = false; }
    }

    public void SKILL_01()
    {
        bSKILL_01_ACTIVATED = true;
        bFireTripleArrow = true;
        Debug.Log("SKILL <color=green> activated </color>");
        StartCoroutine(USE_SKILL_01());
    }

    public void SKILL_02()
    {
        bSKILL_02_ACTIVATED = true;
        bFireHighArrow = true;
        StartCoroutine(USE_SKILL_02());
    }

    public void SKILL_03()
    {
        bSKILL_03_ACTIVATED = true;
        bFireCloneArrow = true;
        StartCoroutine(USE_SKILL_03());
    }

    private IEnumerator USE_SKILL_01()
    {
        skill_01_anime.SetBool("skillActive", false);

        SKILL_01_IMAGE.fillAmount = 0f;

        float timer = 0;

        while(timer <= SKILL_01_TIMER)
        {
            timer += Time.deltaTime;
            SKILL_01_IMAGE.fillAmount = timer / SKILL_01_TIMER;
            yield return null;
        }
    }

    private IEnumerator USE_SKILL_02()
    {
        skill_02_anime.SetBool("skillActive", false);

        SKILL_02_IMAGE.fillAmount = 0f;

        float timer = 0;

        while (timer <= SKILL_02_TIMER)
        {
            timer += Time.deltaTime;
            SKILL_02_IMAGE.fillAmount = timer / SKILL_02_TIMER;
            yield return null;
        }
    }

    private IEnumerator USE_SKILL_03()
    {
        skill_03_anime.SetBool("skillActive", false);

        SKILL_03_IMAGE.fillAmount = 0f;

        float timer = 0;

        while (timer <= SKILL_03_TIMER)
        {
            timer += Time.deltaTime;
            SKILL_03_IMAGE.fillAmount = timer / SKILL_03_TIMER;
            yield return null;
        }
    }
}
