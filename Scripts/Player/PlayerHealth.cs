using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public int currentHealth = 200;
    private int maxHealth = 200;

    public bool isDead;

    private Animation anime;

    public Image health_slider;
    public Text health_text;

    [SerializeField]
    private float updateSpeedSeconds = 0.5f;

    public Animator healthEffect;
    private Text healthEffectAmountText;

    void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("currentHealth"))
        {
            // we had a previous session
            currentHealth = PlayerPrefs.GetInt("currentHealth", maxHealth);
        }
        else
        {
            SavePrefs();
        }

    }

    void Start()
    {
        anime = GetComponentInChildren<Animation>();

        if(currentHealth < 200)
        {
            currentHealth = maxHealth;
            SavePrefs();
        }

        UpdateHealthGUI(currentHealth, maxHealth);
        healthEffectAmountText = healthEffect.GetComponentInChildren<Text>();
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("currentHealth", currentHealth);
    }

    public void UpdateHealthGUI(int _health, int _maxHealth)
    {
        health_text.text = _health.ToString() + "/" + _maxHealth.ToString();
    }

    public void DamagePlayer(int damage_amount)
    {
        bool _isDead = currentHealth <= 0;

        if (_isDead)
            return;

        currentHealth -= damage_amount;
        SavePrefs();
        healthEffectAmountText.text = "-" + damage_amount;

        healthEffect.SetTrigger("healthEffect");
        anime.Play("Damage");

        StartCoroutine(ChangeToPct((float)currentHealth/(float)maxHealth));
        Debug.Log("Current Health Amount: " + currentHealth);
        UpdateHealthGUI(currentHealth, maxHealth);

        if (_isDead)
        {
            //Player is dead
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        //anime.Stop();
        //anime.Play("Die");
        //Respawwn if princess still alive..
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = health_slider.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            health_slider.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        health_slider.fillAmount = pct;
    }

    public void INCREASE_HEALTH(int amount)
    {
        currentHealth += amount;
        healthEffectAmountText.text = "+" + amount;
        healthEffect.SetTrigger("healthEffect");
        StartCoroutine(ChangeToPct((float)currentHealth / (float)maxHealth));
        UpdateHealthGUI(currentHealth,maxHealth);
    }
}
