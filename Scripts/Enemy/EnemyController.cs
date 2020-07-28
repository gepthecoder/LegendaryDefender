using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Range(1, 50)]
    public int Damage = 10;
    [Space(10)]
    [Range(1, 50)]
    [SerializeField]private float fightSpeed = 2f;
    float attackCooldown = 0f;

    [Range(1, 200)]
    [SerializeField ]private float lookRadius = 10f;
    [Range(1, 50)]
    [SerializeField] private float chestMonsterAttackTrickRadius = 10f;

    Transform player_target;
    Transform princess_target;

    NavMeshAgent agent;

    private Animator anime;
    private EnemyHealth health;

    public bool canAttackFence;

    void Start()
    {
        princess_target = PlayerManager.instance.princess.transform;
        player_target = PlayerManager.instance.player.transform;

        agent = GetComponent<NavMeshAgent>();
        anime = GetComponentInChildren<Animator>();
        health = GetComponent<EnemyHealth>();

    }

    public bool CAN_PLAY_ANIME()
    {
        return gameObject.name == "Enemy01(Clone)" || gameObject.name == "Enemy02(Clone)";
    }

    public bool IsChestMonster()
    {
        return gameObject.name == "ChestMonsterBlue(Clone)" || gameObject.name == "ChestMonsterGreen(Clone)";
    }

    void Update()
    {

        if (player_target.GetComponent<PlayerHealth>().isDead || princess_target.GetComponent<QueenLogic>().isDead)
        {
            anime.SetTrigger("Victory");
            return;
        }

        if (!PlayerManager.instance.canChase || health.isDead) { return; }

        if (canAttackFence)
        {
            attackCooldown -= Time.deltaTime;
            if(attackCooldown <= 0)
            {
                anime.SetTrigger("attack");
                attackCooldown = fightSpeed;
            }

            return;
        }
        else if (IsChestMonster())
        {
            if(health.health <= 0) { return; }
            float playerDistance = Vector3.Distance(player_target.position, transform.position);
            attackCooldown -= Time.deltaTime;

            if (playerDistance <= lookRadius)
            {
                if(playerDistance <= chestMonsterAttackTrickRadius)
                {
                    agent.isStopped = false;

                    // CHANGE TAG TO ENEMY
                    if (gameObject.tag != "Enemy")
                    {
                        gameObject.tag = "Enemy";
                        // LOAD ENEMIES
                        ArcherController.instance.bLoadEnemies = true;
                    }

                    //ACTIVATE CHILDREN
                    health.child0.SetActive(true);

                    // PERSUE ENEMY TILL DEAD
                    agent.SetDestination(player_target.position);
                    anime.SetBool("run", true);
                    Debug.Log("<color=green>ENEMY RUN</color>");
                    chestMonsterAttackTrickRadius = lookRadius;
                    // WAIT TO ATTACK
                    if (playerDistance <= agent.stoppingDistance)
                    {
                        //Attack
                        if (attackCooldown <= 0)
                        {
                            PlayerHealth playerHealth = player_target.GetComponent<PlayerHealth>();
                            if (playerHealth != null)
                            {
                                playerHealth.DamagePlayer(Damage);
                                // Attack the player
                                anime.SetTrigger("attack");
                                Rigidbody playerRB = playerHealth.GetComponent<Rigidbody>();
                                if(playerRB != null)
                                {
                                    StartCoroutine(AddForceToPlayer(playerRB));
                                }
                            }
                            attackCooldown = fightSpeed;
                        }
                        FaceTarget(true);
                    }
                }
                else
                {
                    // STOP 
                    // HIDE
                    agent.isStopped = true;
                    anime.SetBool("run", false);
                }
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player_target.position);
                anime.SetBool("run", true);
            }
        }
        else
        {
            float playerDistance = Vector3.Distance(player_target.position, transform.position);
            attackCooldown -= Time.deltaTime;

            if (playerDistance <= lookRadius)
            {
                // player comes close to enemy -> enemy changes destination to player
                agent.SetDestination(player_target.position);
                //if (CAN_PLAY_ANIME()) { anime.SetTrigger("run"); }
                

                if (playerDistance <= agent.stoppingDistance)
                {
                    if (attackCooldown <= 0)
                    {
                        if (player_target.GetComponent<PlayerHealth>() != null)
                        {
                            player_target.GetComponent<PlayerHealth>().DamagePlayer(Damage);
                            // Attack the player
                            anime.SetTrigger("attack");
                        }
                        attackCooldown = fightSpeed;
                    }
                    // Face the target
                    FaceTarget(true);
                }
            }
            else
            {
                float princessDistance = Vector3.Distance(princess_target.position, transform.position);

                agent.SetDestination(princess_target.position);

                if (princessDistance <= agent.stoppingDistance)
                {
                    if (attackCooldown <= 0)
                    {
                        if (princess_target.GetComponent<QueenLogic>() != null)
                        {
                            princess_target.GetComponent<QueenLogic>().DamagePrincess(5);
                            // Attack the princess
                            anime.SetTrigger("attack");
                        }
                        attackCooldown = fightSpeed;
                    }
                    // look at the target
                    FaceTarget(false);
                }
            }
        }
      
    }

    void FaceTarget(bool player)
    {
        Vector3 choosenTarget = player ? player_target.position : princess_target.position;

        Vector3 direction = (choosenTarget - transform.position).normalized; //GET DIRECTION TO THE TARGET
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // ROTATION TO POINT AT THE TARGET

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chestMonsterAttackTrickRadius);

    }

    IEnumerator AddForceToPlayer(Rigidbody rb)
    {
        yield return new WaitForSeconds(.8f);
        rb.AddExplosionForce(40f, rb.transform.position, 6f);
    }
}
