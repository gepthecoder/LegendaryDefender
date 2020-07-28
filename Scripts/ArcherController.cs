using UnityEngine;
using System.Collections;

public class ArcherController : MonoBehaviour
{
    public static ArcherController instance;

    public GameObject NORMAL_ArrowPrefab;                  // The Arrow to shoot
    public GameObject SKILL_01_ArrowPrefab;                  // The Arrow to shoot
    public GameObject SKILL_02_ArrowPrefab;                  // The Arrow to shoot
    public GameObject SKILL_03_ArrowPrefab;                  // The Arrow to shoot
    public GameObject SKILL_03_CLONES;
    
    public GameObject shootSpot;

    public float MaximalShootRange = 100f;          // Maximal distance to shoot
    public float MinimalShootRange = 4f;            // Minimal distance to shoot
    public float SpreadFactor = 0.5f;               // Accuracy
    public float SpreadFactorDistanceImpact = 0.1f; // Impact of the distance (from shooter to target) on the accuracy
    public float HightMultiplier = 2f;              // Changes the Parabola-Height of the flightpath (Arrows fly in a higher arc)
    public float ArrowFlightSpeed = 6f;             // Speed of the Arrow
    public float ArrowLifeTime = 120f;              // Time until the Arrow gets destroyed (in seconds) 

    public Transform[] allEnemiesSpawned;

    public bool bLoadEnemies;
    public bool bEnemiesLoaded;

    private float shootCoolDown;
    public float coolDownTime = .07f;

    private Controller controller;

    public bool preparedForTrigger;
    public bool preparedForTrigger1;

    public bool preparedForPunch;

    public bool enemyKilled;

    public bool enemyIsClose;

    public bool bUseSkill01;
    public bool bUseSkill02;
    public bool bUseSkill03;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        bEnemiesLoaded = false;
        enemyKilled = false;
        bLoadEnemies = false; // change to false
        controller = GetComponent<Controller>();
    }

    bool LoadEnemies()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            bEnemiesLoaded = false;
            Debug.Log("No enemies left!! Are therre there deadFish? hmm");
            return false;
        }
        GameObject[] goS = GameObject.FindGameObjectsWithTag("Enemy");
       
        allEnemiesSpawned = new Transform[goS.Length];
        //populate my array with transforms
        for (int i = 0; i < goS.Length; i++)
        {
            allEnemiesSpawned[i] = goS[i].transform;
        }
        Debug.Log(allEnemiesSpawned.Length + " Lenght of enemies list");
        bEnemiesLoaded = true;
        return true;
    }

    public Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        if(enemies.Length > 0)
        {
            //by default grab first enemy
            bestTarget = enemies[0];
            Vector3 directionToTarget = bestTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            closestDistanceSqr = dSqrToTarget;
        }
        foreach (Transform potentialTarget in enemies)
        {
            if(potentialTarget == bestTarget) { continue; }

            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    private bool usingSkill01;

    IEnumerator SKILL_01_EFFECT()
    {
        Time.timeScale = .6f;
        yield return new WaitForSeconds(.4f);
        ShootArrow(SKILL_01_ArrowPrefab, ArrowFlightSpeed, HightMultiplier - .45f, false);
        yield return new WaitForSeconds(.15f);                                
        ShootArrow(SKILL_01_ArrowPrefab, ArrowFlightSpeed, HightMultiplier - .45f, false);
        yield return new WaitForSeconds(.15f);                                
        ShootArrow(SKILL_01_ArrowPrefab, ArrowFlightSpeed, HightMultiplier - .45f, false);
        yield return new WaitForSeconds(.2f);
        Debug.Log("<color=red>LAST ARROW</color>");
        SkillManager.instance.bSKILL_01_ACTIVATED = false;
        Time.timeScale = 1f;
    }

    private bool usingSkill02;

    IEnumerator SKILL_02_EFFECT()
    {
        Time.timeScale = .6f;
        yield return new WaitForSeconds(.5f);
        ShootArrow(SKILL_02_ArrowPrefab, ArrowFlightSpeed - 2f, HightMultiplier + 5f, true);
        Debug.Log("SKILL 2 ARROW INCOMING!");
        yield return new WaitForSeconds(.5f);
        SkillManager.instance.bSKILL_02_ACTIVATED = false;
        Time.timeScale = 1f;
    }

    IEnumerator SKILL_03_EFFECT()
    {
        Quaternion skillRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, 0);
        GameObject clones = Instantiate(SKILL_03_CLONES, transform, false);
        Destroy(clones, 5f);

        Time.timeScale = .6f;
        yield return new WaitForSeconds(.5f);
        ShootArrow(SKILL_03_ArrowPrefab, ArrowFlightSpeed - 2f, HightMultiplier + 5f, true);
        yield return new WaitForSeconds(.5f);
        SkillManager.instance.bSKILL_03_ACTIVATED = false;
        Time.timeScale = 1f;
    }

    public void SHOOT_TRIPLE_ARROW()
    {
        StartCoroutine(SKILL_01_EFFECT());
    }

    public void SHOOT_HIGH_ARROW()
    {
        StartCoroutine(SKILL_02_EFFECT());
    }

    public void CLONE_ATTACK()
    {
        StartCoroutine(SKILL_03_EFFECT());
    }

    private void Update()
    {
        // LOAD NEW LIST OF CLOSEST ENEMIES
        if (bLoadEnemies)
        {
            LoadEnemies();
            bLoadEnemies = false;
        }
        else if (enemyKilled)
        {
            LoadEnemies();
            enemyKilled = false;
        }
        //////////////////////////////////
        if (bEnemiesLoaded && !WaveSpawner.waveComplited)
        {
            shootCoolDown -= Time.deltaTime;

            bool canShoot;
            bool canShoot1;

            bool canPunch;

            bool UsingSkill01 = SkillManager.instance.bSKILL_01_ACTIVATED;
            bool UsingSkill02 = SkillManager.instance.bSKILL_02_ACTIVATED;
            bool UsingSkill03 = SkillManager.instance.bSKILL_03_ACTIVATED;

            bool canUseSkill = UsingSkill01 || UsingSkill02 || UsingSkill03;

            if(GetClosestEnemy(allEnemiesSpawned) == null) { SkillManager.instance.controllerAllowsToActivateSkill = false;  return; }
            Transform closestEnemyPos = GetClosestEnemy(allEnemiesSpawned);

            float distance = Vector3.Distance(transform.position, closestEnemyPos.position);

            canShoot    = distance > (MaximalShootRange / 2)    && distance <= MaximalShootRange        ? true : false;
            canShoot1   = distance > MinimalShootRange          && distance <= (MaximalShootRange / 2)  ? true : false;

            canPunch = distance <= MinimalShootRange;

            preparedForTrigger  = canShoot  && shootCoolDown <= 0   && !preparedForTrigger1 && !canUseSkill ? true : false;
            preparedForTrigger1 = canShoot1 && shootCoolDown <= 0   && !preparedForTrigger  && !canUseSkill ? true : false;

            preparedForPunch = canPunch && shootCoolDown <= 0 ? true : false;

            SkillManager.instance.controllerAllowsToActivateSkill = (canShoot || canShoot1) ?  true : false;

            if (SkillManager.instance.bFireTripleArrow)
            {
                PlayerManager.instance.PLAY_COMBO();
                SkillManager.instance.bFireTripleArrow = false;
                SHOOT_TRIPLE_ARROW();
            }
            else if (SkillManager.instance.bFireHighArrow)
            {
                PlayerManager.instance.PLAY_COMBO();
                SkillManager.instance.bFireHighArrow = false;
                SHOOT_HIGH_ARROW();
            }
            else if (SkillManager.instance.bFireCloneArrow)
            {
                PlayerManager.instance.PLAY_COMBO();
                SkillManager.instance.bFireCloneArrow = false;
                CLONE_ATTACK();
            }

            if (canUseSkill) { return; }

            if ((canShoot || canShoot1) && shootCoolDown <= 0 && controller.canShoot)
            {
                Debug.Log("shootArrow");
                ShootArrow(NORMAL_ArrowPrefab, ArrowFlightSpeed, HightMultiplier, false);
                shootCoolDown = coolDownTime;
                controller.canShoot = false;
            }
            if (canPunch && shootCoolDown <= 0 && controller.canPunch)
            {
                Debug.Log("Punch him in the face!");
                FrontHandBackhand(closestEnemyPos);
                shootCoolDown = coolDownTime;
                controller.canPunch = false;
            }
        }
        else if (WaveSpawner.waveComplited) { preparedForTrigger = false; preparedForPunch = false; preparedForTrigger1 = false; }
    }

    private void FrontHandBackhand(Transform closest)
    {
        Rigidbody rb = closest.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.AddExplosionForce(200f, closest.position, 2f, 3f, ForceMode.Impulse);
            EnemyHealth enemyHealth = rb.GetComponent<EnemyHealth>();
            if(enemyHealth != null)
            {
                enemyHealth.DamageEnemy(10);
                Debug.Log("Enemy just got punched in the face!! :))");
            }
        }
    }

    private void ShootArrow(GameObject arrowToShoot, float _ArrowFlightSpeed, float _HightMultiplier, bool isSpecialAttack)
    {
        // Every "hit.point" can be replaced with any vector as target
        Vector3 origin = shootSpot.transform.position;
        Vector3 direction = shootSpot.transform.position - GetClosestEnemy(allEnemiesSpawned).position;
        // Shoot a ray from the camera to the cursor to get the cursor position as the target
        Ray ray = new Ray(origin, -direction);
        RaycastHit hit;
        Debug.DrawRay(origin, -direction, Color.green, 100f);
        if (Physics.Raycast(ray, out hit))
        {
            // Make the raycast ignore Arrows
            if (hit.collider.tag != "Projectile" )
            {
                // Only allow to shoot targets withi3n minimum and maximum range
                var distance = Vector3.Distance(transform.position, hit.point);
                if (distance >= MinimalShootRange && distance <= MaximalShootRange)
                {
                    // Calculate the spread-range relative to the distance
                    float spreadFactorByDistance = SpreadFactor * (1f + (SpreadFactorDistanceImpact * distance));

                    // Calculate inaccurate target (somewhere around the original target)
                    Vector3 inaccurateTarget = (Random.insideUnitSphere * spreadFactorByDistance) + hit.point;

                    // Create a new Arrow
                    var Arrow = Instantiate(arrowToShoot, origin, shootSpot.transform.rotation);

                    // Name the arrow "Arrow" to remove the default name with "(Clone)"
                    Arrow.name = "Arrow";

                    // Tell the arrow to go shwoooosh
                    Arrow.GetComponent<ArrowController>().Shoot(inaccurateTarget, gameObject, _ArrowFlightSpeed, _HightMultiplier, ArrowLifeTime, isSpecialAttack);
                }
            }
        }
    }
}