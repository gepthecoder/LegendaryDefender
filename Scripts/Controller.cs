using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Controller : MonoBehaviour
{
    public static Controller instance;

    [Header("CUSTOM CHARACTER CONTROLLER SCRIPT")]
    [Space(10)]
    public FixedJoystick Ljoystick;
    [Space(10)]
    [SerializeField] private FixedJoystick Rjoystick;
    [Space(6)]
    [SerializeField] private Transform cam;
    [Space(6)]
    [SerializeField] private CinemachineFreeLook cinemachine;
    [Space(10)]
    CinemachineComposer comp;

    //private CharacterController movementControler;
    private Rigidbody rb;
    private Animation anime;
    private CapsuleCollider collider;

    [Header("VARIABLES")]
    [Space(10)]
    [Range(1f, 20f)] [SerializeField] private float speed = 8f;
    private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private float verticalVelocity;
    private const float gravity = 14.0f;
    [Range(100f, 1000f)]
    [SerializeField]
    private float jumpForce = 300f;
    private Vector3 moveDir = Vector3.zero;
    private bool bInJumpMotion;
    float y;

    [Space(10)]
    [SerializeField] private float jetForce;
    [Space(10)]
    [SerializeField] private float jetWait;
    [Space(10)]
    [SerializeField] private float jetRecovery;
    [Space(10)]
    [SerializeField] private float max_fuel;
    [Space(10)]
    [SerializeField] private Image fuel_Slider;

    private float   current_fuel;
    private float   current_recovery;
    private bool    canJet;
    private bool    isJetting;

    private ArcherController archerControls;
    public bool canShoot;
    public bool canPunch;

    //  JETPACK PARTICLE SYSTEM

    //  BLUE THUNDER
    public ParticleSystem jetpack_fire;
    public ParticleSystem jetpack_lines;
    public ParticleSystem jetpack_fog;
    //

    //  GREEN MAMBA
    public ParticleSystem GM_jetpack_fire;
    public ParticleSystem GM_jetpack_lines;
    public ParticleSystem GM_jetpack_fog;
    //

    //  GOLDEN JUICE
    public ParticleSystem GJ_jetpack_fire;
    public ParticleSystem GJ_jetpack_lines;
    public ParticleSystem GJ_jetpack_fog;
    //

    //  CONSERVATIVE
    public ParticleSystem C_jetpack_fire;
    public ParticleSystem C_jetpack_lines;
    public ParticleSystem C_jetpack_fog;
    //

    //  DOUBLE FURIOUS
    public ParticleSystem DF_jetpack_fire;
    public ParticleSystem DF_jetpack_lines;
    public ParticleSystem DF_jetpack_fog;

    public ParticleSystem DF_jetpack_fire1;
    public ParticleSystem DF_jetpack_lines1;
    public ParticleSystem DF_jetpack_fog1;
    //

    // SUPER TURBO
    public ParticleSystem ST_jetpack_fire;
    public ParticleSystem ST_jetpack_lines;
    public ParticleSystem ST_jetpack_fog;
    //

    //

    [Space(10)]
    [SerializeField]
    private Image jumpFlyImg;
    [Space(5)]
    public Sprite jumpSprite;
    [Space(5)]
    public Sprite flySprite;

    public GameObject SHADOW;

    public bool bSkillAttack = false;
    public float skill_01_anime_lenght = 1.04f;
    public float skill_02_anime_lenght = .25f;

    public static bool bCanPlantMine;
    public static bool bCanUseShield;

    private PlayerHealth health;
    public bool enemyVictory;

    public bool _inShop;

    void Awake()
    {
        instance = this;
        //movementControler   = GetComponent              <CharacterController>();
        anime               = GetComponentInChildren    <Animation>();
        rb                  = GetComponent              <Rigidbody>();
        collider            = GetComponent              <CapsuleCollider>();
        archerControls      = GetComponent              <ArcherController>();
        health              = GetComponent              <PlayerHealth>();
    }

    void Start()
    {
        //comp = cinemachine.GetRig(0).GetCinemachineComponent<CinemachineComposer>();
        current_fuel = max_fuel;
        jumpFlyImg.sprite = jumpSprite;
    }

    void Update()
    {
        //float DistanceToTheGround = collider.bounds.extents.y;
        if (health.isDead)
        {
            enemyVictory = true;
            Play_Dead();
            return;
        }

        if (WaveSpawner.instance.playerVictorious)
        {
            Play_Victory();
            return;
        }

        if (_inShop)
        {
            //Play Animation
            Play_InShop();
            return;
        }

        bool jet = CrossPlatformInputManager.GetButton("Jump") || Input.GetKey(KeyCode.Space);
        bool jump = CrossPlatformInputManager.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space);
        //bool isGrounded = Physics.CheckCapsule(collider.bounds.center, new Vector3(collider.bounds.center.x, collider.bounds.min.y - 0.1f, collider.bounds.center.z), 0.5f);
        bool isGrounded = Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, 0.1f);
        //Debug.DrawRay(transform.position + Vector3.up * .1f, Vector3.down, Color.red, 0.1f);

        float horizontal = Ljoystick.input.x;
        float vertical = Ljoystick.input.y;
        float lookX = Rjoystick.input.x;
        float lookY = Rjoystick.input.y;

        cinemachine.m_XAxis.m_InputAxisValue = lookX;
        cinemachine.m_YAxis.m_InputAxisValue = lookY;


        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        //HandleCam(horizontal, vertical);

        if (jump && !isGrounded) { canJet = true; Debug.Log("can jet!!"); }

        //if (WaveSpawner.waveComplited)
        //{
        //    celebrationTime -= Time.deltaTime;
        //    Play_Victory();
        //    if (celebrationTime <= 0f)
        //    {
        //        celebrationTime = 5f;
        //        WaveSpawner.waveComplited = false;
        //    }
        //    return;
        //}

        if (canJet && jet && current_fuel > 0)
        {
            //Vector3 flyMotion = Vector3.up * jetForce * Time.deltaTime;
            //movementControler.Move(flyMotion);
            rb.AddForce(Vector3.up * jetForce * Time.deltaTime, ForceMode.Acceleration);
            current_fuel = Mathf.Max(0, current_fuel - Time.deltaTime);
            isJetting = true;
            //canShoot = true;
            Play_PrepareBow();
        }
        else { isJetting = false; }

        if (isGrounded)
        {
            canJet = false;
            jumpFlyImg.sprite = jumpSprite;
            if (current_recovery < jetWait) { current_recovery = Mathf.Min(jetWait, current_recovery + Time.deltaTime); }
            else { current_fuel = Mathf.Min(max_fuel, current_fuel + Time.deltaTime * jetRecovery); }
            if (jump) {
                //y = jumpForce;
                Play_Jump();
                rb.AddForce(Vector3.up * jumpForce);
                current_recovery = 0;
                bInJumpMotion = true;
            }
            else { bInJumpMotion = false; }

            SHADOW.SetActive(true);
        }
        else { jumpFlyImg.sprite = flySprite;
            SHADOW.SetActive(false);
        }
        //y -= gravity * Time.deltaTime;
        //Vector3 jumpVect = new Vector3(0, y, 0);
        //movementControler.Move(jumpVect);     

        //TO:DO -> set slider value of jetpack fuel
        fuel_Slider.fillAmount = current_fuel / max_fuel;

        if (direction.magnitude >= 0.1f) {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (isJetting)
            {
                transform.rotation = Quaternion.Euler(Mathf.LerpAngle(transform.rotation.x, 12f, 4f), angle, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(Mathf.LerpAngle(transform.rotation.x, 0f, 4f), angle, 0f);
            }

            if (isGrounded && !bInJumpMotion)
            {
                Play_Run();
                canShoot = false;
                canPunch = false;
                bCanPlantMine = false;
                bCanUseShield = false;
            }

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        
        //movementControler.Move(moveDir.normalized * speed * Time.deltaTime);
        rb.MovePosition(transform.position + (moveDir.normalized * speed * Time.deltaTime));
        }
        else
        {
            bCanPlantMine = true;

            if(!isJetting && isGrounded) { bCanUseShield = true; }
            else { bCanUseShield = false; }

            if (!isJetting && isGrounded && archerControls.bEnemiesLoaded)
            {
                LookAtEnemy();
            }

            if (SkillManager.instance.bSKILL_01_ACTIVATED && !WaveSpawner.waveComplited)
            {
                Play_Skill01();
            }
            else if (SkillManager.instance.bSKILL_02_ACTIVATED && !WaveSpawner.waveComplited)
            {
                Play_Skill02();
            }
            else if (SkillManager.instance.bSKILL_03_ACTIVATED && !WaveSpawner.waveComplited)
            {
                Play_Skill03();
            }
            else
            {
                bool hasEnemies = EnemiesAreAlive();
                if (archerControls.preparedForTrigger && !WaveSpawner.waveComplited && hasEnemies)
                {
                    if (!anime.IsPlaying("Skill01") || !anime.IsPlaying("Skill02"))
                        Play_Attack("Attack01");
                    //Debug.Log("<color=green>Attack 1</color>");
                }
                else if (archerControls.preparedForTrigger1 && !WaveSpawner.waveComplited && hasEnemies)
                {
                    //Debug.Log("<color=red>Attack 2</color>");
                    if (!anime.IsPlaying("Skill01") || !anime.IsPlaying("Skill02"))
                        Play_Attack("Attack02");
                }
                else if (archerControls.preparedForPunch)
                {
                    Play_Fight();
                }
                else
                {
                    if (isGrounded && !bInJumpMotion)
                        Play_Idle();
                }
            }
        }
    }

    public float celebrationTime = 5f;

    void FixedUpdate()
    {
        int cJet = PlayerPrefs.GetInt("jetpack", 0);

        JETPACK_HANDLER(cJet);
    }

    //private void HandleCam(float h, float v)
    //{
    //    if (v >= .9f) { comp.m_TrackedObjectOffset.y = 3; }
    //    else if (v <= -.9f) { comp.m_TrackedObjectOffset.y = -3; }
    //}


    #region ANIMATIONS
    private void Play_Idle()
    {
        anime.Play("Idle01");
    }

    private void Play_Run()
    {
        anime.Play("Run");
    }

    private void Play_Jump()
    {
        anime["Jump"].speed = 1.5f;
        anime.Play("Jump");
        //Debug.Log("play jump anime");
    }

    private void Play_PrepareBow()
    {
        anime.Play("AttackWating");
        //Debug.Log("play jump anime");
    }

    private void Play_Attack(string atckVal)
    {
        //Debug.Log("<color=green>Attacking NORMAL</color>");
        anime.Play(atckVal);
    }

    private string Attack_Anime_Trigger()
    {
        int a = Random.Range(0, 10);
        return a >= 5 ? "Attack01" : "Attack02";
    }

    private void Play_Victory()
    {
        anime.Play("Victory");
    }

    private void Play_Fight()
    {
        anime.Play("Action");
    }

    private void Play_Dead()
    {
        anime.Play("Die");
    }

    private void Play_Skill01()
    {
        anime.clip = anime.GetClip("Skill01");
        anime.Play("Skill01");
        Debug.Log("<color=red>anime SKILL1 is playing</color>");

    }

    private void Play_Skill02()
    {
        anime.Play("Skill02");
        Debug.Log("<color=red>anime SKILL2 is playing</color>");
    }

    private void Play_Skill03()
    {
        anime.Play("Skill02");
        Debug.Log("<color=red>anime SKILL2 is playing</color>");
    }

    private void Play_InShop()
    {
        anime["Idle03"].speed = .4f;
        anime.Play("Idle03");
    }
    #endregion
    
    public bool fireOnce01;
    public int timesShot01;
    private void SKILL_01()
    {
        bSkillAttack = true;
        Play_Skill01();
        fireOnce01 = true;
        StartCoroutine(Skill_01_Attacking());
    }

    public bool fireOnce02;
    public int timesShot02;

    private void SKILL_02()
    {
        bSkillAttack = true;
        Play_Skill02();
        fireOnce02 = true;
        StartCoroutine(Skill_02_Attacking());
    }

    private IEnumerator Skill_01_Attacking()
    {
        yield return new WaitForSeconds(skill_01_anime_lenght);
        bSkillAttack = false;
        SkillManager.instance.bSKILL_01_ACTIVATED = false;
        Debug.Log("<color=blue>STOP SPECIAL ATTACK</color>");
    }

    private IEnumerator Skill_02_Attacking()
    {
        yield return new WaitForSeconds(skill_02_anime_lenght);
        bSkillAttack = false;
        SkillManager.instance.bSKILL_02_ACTIVATED = false;
        Debug.Log("<color=blue>STOP SPECIAL ATTACK</color>");

    }



    private void LookAtEnemy()
    {
        if(GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            if (Vector3.Distance(transform.position, archerControls.GetClosestEnemy(archerControls.allEnemiesSpawned).position) > archerControls.MaximalShootRange) { return; }

            Vector3 targetDirection = archerControls.GetClosestEnemy(archerControls.allEnemiesSpawned).position - transform.position;
            float step = 3f * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    float searchCountdown;

    bool EnemiesAreAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    private void JETPACK_ON(int currentJetpack)
    {
        switch (currentJetpack)
        {
            case (int)JetPackManager.JETPACKS.blueThunder:
                jetpack_fire.Play();
                jetpack_fog.Play();
                jetpack_lines.Play();
                break;
            case (int)JetPackManager.JETPACKS.greenMamba:
                GM_jetpack_fire.Play();
                GM_jetpack_fog.Play();
                GM_jetpack_lines.Play();
                break;
            case (int)JetPackManager.JETPACKS.goldenJuice:
                GJ_jetpack_fire.Play();
                GJ_jetpack_fog.Play();
                GJ_jetpack_lines.Play();
                break;
            case (int)JetPackManager.JETPACKS.conservative:
                C_jetpack_fire.Play();
                C_jetpack_fog.Play();
                C_jetpack_lines.Play();
                break;
            case (int)JetPackManager.JETPACKS.doubleFurious:
                DF_jetpack_fire.Play();
                DF_jetpack_fog.Play();
                DF_jetpack_lines.Play();

                DF_jetpack_fire1.Play();
                DF_jetpack_fog1.Play();
                DF_jetpack_lines1.Play();
                break;
            case (int)JetPackManager.JETPACKS.superTurbo:
                ST_jetpack_fire.Play();
                ST_jetpack_fog.Play();
                ST_jetpack_lines.Play();
                break;
        }
      
    }

    private void JETPACK_OFF(int currentJetpack)
    {
        switch (currentJetpack)
        {
            case (int)JetPackManager.JETPACKS.blueThunder:
                if (!jetpack_fire.isStopped)
                    jetpack_fire.Stop();
                if (!jetpack_fog.isStopped)
                    jetpack_fog.Stop();
                if (!jetpack_lines.isStopped)
                    jetpack_lines.Stop();
                break;
            case (int)JetPackManager.JETPACKS.greenMamba:
                if (!GM_jetpack_fire.isStopped)
                    GM_jetpack_fire.Stop();
                if (!GM_jetpack_fog.isStopped)
                    GM_jetpack_fog.Stop();
                if (!GM_jetpack_lines.isStopped)
                    GM_jetpack_lines.Stop();
                break;
            case (int)JetPackManager.JETPACKS.goldenJuice:
                if (!GJ_jetpack_fire.isStopped)
                    GJ_jetpack_fire.Stop();
                if (!GJ_jetpack_fog.isStopped)
                    GJ_jetpack_fog.Stop();
                if (!GJ_jetpack_lines.isStopped)
                    GJ_jetpack_lines.Stop();
                break;
            case (int)JetPackManager.JETPACKS.conservative:
                if (!C_jetpack_fire.isStopped)
                    C_jetpack_fire.Stop();
                if (!C_jetpack_fog.isStopped)
                    C_jetpack_fog.Stop();
                if (!C_jetpack_lines.isStopped)
                    C_jetpack_lines.Stop();
                break;
            case (int)JetPackManager.JETPACKS.doubleFurious:
                if (!DF_jetpack_fire.isStopped)
                    DF_jetpack_fire.Stop();
                if (!DF_jetpack_fog.isStopped)
                    DF_jetpack_fog.Stop();
                if (!DF_jetpack_lines.isStopped)
                    DF_jetpack_lines.Stop();

                if (!DF_jetpack_fire1.isStopped)
                    DF_jetpack_fire1.Stop();
                if (!DF_jetpack_fog1.isStopped)
                    DF_jetpack_fog1.Stop();
                if (!DF_jetpack_lines1.isStopped)
                    DF_jetpack_lines1.Stop();
                break;
            case (int)JetPackManager.JETPACKS.superTurbo:
                if (!ST_jetpack_fire.isStopped)
                    ST_jetpack_fire.Stop();
                if (!ST_jetpack_fog.isStopped)
                    ST_jetpack_fog.Stop();
                if (!ST_jetpack_lines.isStopped)
                    ST_jetpack_lines.Stop();
                break;
        }
    }

    private void JETPACK_HANDLER(int currentJetpack)
    {
        if (isJetting) { JETPACK_ON(currentJetpack); } else { JETPACK_OFF(currentJetpack); }
    }

    public void SET_MAX_FUEL(float maxJetFuel)
    {
        max_fuel = maxJetFuel;
    }
}
