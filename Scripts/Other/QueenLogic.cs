using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class QueenLogic : MonoBehaviour
{
    public static QueenLogic instance;

    public int currentHealth;

    [Range(100, 1000)]
    [SerializeField]
    private int maxHealth = 300;

    [SerializeField]
    private float updateSpeedSeconds = 0.5f;

    public Text health_text;
    public Image health_bar;

    public bool isDead;

    private Animation anime;

    private NavMeshAgent agent;
    public Transform target_destination;
    public bool canGoToDestination;
    public Transform lookPoint;
    public bool reachedDestination;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText(currentHealth, maxHealth);
        anime = GetComponentInChildren<Animation>();
        agent = GetComponent<NavMeshAgent>();
        health_bar.fillAmount = 1;
    }

    void Update()
    {
        if (!isDead)
        {
            if (!canGoToDestination)
            {
                anime.Play("Fail");
                if (reachedDestination) { FaceTarget(false); }
            }
            else
            {
                agent.SetDestination(target_destination.position);
                anime["Run"].speed = .7f;
                anime.Play("Run");
                FaceTarget(true);
            }
        }
    }

    public void UpdateHealthText(int _currentHealth, int _maxHealth=300)
    {
        health_text.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
    }

    public void DamagePrincess(int damage)
    {
        bool _isDead = currentHealth <= 0;

        if (_isDead) { return; }

        currentHealth -= damage;
        anime.Play("Damage");
        UpdateHealthText(currentHealth, maxHealth);
        StartCoroutine(ChangeToPct((float)currentHealth / (float)maxHealth));

        if (_isDead)
        {
            //game over
            GameOver();
        }
    }

    void GameOver()
    {
        isDead = true;
        anime.Play("Die");

        Debug.Log("GAME OVER!!");
    }


    public void FaceTarget(bool target)
    {
        Vector3 choosenTarget = target ? target_destination.position : lookPoint.position;

        Vector3 direction = (choosenTarget - transform.position).normalized; //GET DIRECTION TO THE TARGET
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // ROTATION TO POINT AT THE TARGET

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = health_bar.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            health_bar.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        health_bar.fillAmount = pct;
    }
}
