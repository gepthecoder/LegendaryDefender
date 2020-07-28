using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int health;

    [Range(10, 500)]
    [SerializeField] private int maxHealth = 100;

    public bool isDead;

    private Animator anime;

    public event Action<float> OnHealthPctChanged = delegate { };

    private PopUpController popUpController;
    private EnemyController controller;
    private NavMeshAgent agent;

    public GameObject child0;
    public GameObject child1;

    void Start()
    {
        health = maxHealth;
        isDead = false;
        anime = GetComponentInChildren<Animator>();
        popUpController = GetComponent<PopUpController>();
        controller = GetComponent<EnemyController>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void DamageEnemy(int damage)
    {
        bool _isDead = health <= 0;

        if (_isDead) { return; }

        popUpController.PlayPopUpAnime(damage.ToString());

        ModifyHealth(damage);
        PlayerManager.instance.PLAY_APPROPRIATE_COMBO(damage);

        Debug.Log("current health : " + health);
        if (health <= 0)
        {
            isDead = true;
            if (!controller.CAN_PLAY_ANIME())
            {
                anime.SetTrigger("dead");
                Debug.Log("SET TRIGGER DEAD!");
            }
            //die

            Die();
        }
        else
        {
            if (!controller.CAN_PLAY_ANIME())
            {
                Debug.Log("SET TRIGGER TAKE DAMAGE!!");
                anime.SetTrigger("takeDamage");
            }
        }
    }

    void Die()
    {
        Destroy(child0);
        Destroy(child1);
        agent.enabled = false;
        PlayerManager.instance.PLAY_WOW();

        gameObject.tag = "deadFish";
        ArcherController.instance.enemyKilled = true;
        WaveSpawner.instance.numOfEnemies--;
        WaveSpawner.instance.enemiesLeftText.text = WaveSpawner.instance.numOfEnemies.ToString();

        StartCoroutine(Eliminated());
    }

    public void ModifyHealth(int amount)
    {
        health -= amount;
        float currentHealthPct = (float)health / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    private IEnumerator Eliminated()
    {
        yield return new WaitForSeconds(1f);
     
        if (!controller.CAN_PLAY_ANIME())
        {
            yield return new WaitForSeconds(4f);
            if (isBoss()) { Collectables.instance.SPAWN_RANDOM_OBJECT_HIGHREWARD(transform); }
            else { Collectables.instance.SPAWN_RANDOM_OBJECT_LOWREWARD(transform); }
            Debug.Log("<color=blue>DESTORY 03!!</color>");
            Destroy(gameObject);
          
        }
        else
        {
            Debug.Log("<color=red>DESTROY enemy01 or 02!!</color>");
            Collectables.instance.SPAWN_RANDOM_OBJECT_LOWREWARD(transform);
            Destroy(gameObject);
        }
    }

    bool isBoss()
    {
        return gameObject.name.Contains("BOSS");
    }
}
