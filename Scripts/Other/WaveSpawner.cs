using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{

    public static WaveSpawner instance;

    public enum SpawnState { SPAWNING, WAITING, COUNTING }
    private SpawnState state = SpawnState.COUNTING;


    [System.Serializable]
    public class Wave
    {
        public string Name;
        public Transform[] Enemies;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public int nextWave = 0;

    public Transform[] spawnPoints;
    public Transform[] bossSpawnPoints;

    public float timeBetweenWaves = 21f;
    public float waveCountDown;

    private float searchCountdown = 1f;

    public static bool waveComplited;

    public Animator timeBetweenWavesAnime;
    public Text timeBetweenWavesText;
    public Text waveNameText;
    public Text enemiesLeftText;
    public Text infoText; //till next wave

    public int numOfEnemies;

    public bool animateWaveNameText;

    public bool playerVictorious;
    public Animator waveComplitedAnime;
    public Button skipButton;

    private bool _canSkip;

    void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("nextWave"))
        {
            nextWave = PlayerPrefs.GetInt("nextWave", 0);
        }
        else { SavePrefs(); }
    }

    void Start()
    {
        if (spawnPoints.Length == 0 || bossSpawnPoints.Length == 0) { Debug.LogError("Spawn points missing!"); }
        waveNameText.text = "";
        infoText.text = "";

        waveCountDown = 4.5f;
    }

    void Update()
    {
        _canSkip = waveCountDown <= 5 ? false : true;
        if (_canSkip) { skipButton.interactable = true; }
        else { skipButton.interactable = false; }

        if(state == SpawnState.WAITING)
        {
            // has player killed all enemies
            if (!EnemiesAreAlive())
            {
                // begin new round
                WaveComplited();
                return;
            }
            else
            {
                return;
            }
        }

        if(waveCountDown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                // time to start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }

        }
        else
        {
            waveCountDown -= Time.deltaTime;

            int min = (int)(waveCountDown / 60);
            int sec = (int)(waveCountDown % 60);
            int fraction = (int)(waveCountDown * 1000);
            fraction %= 1000;
            timeBetweenWavesText.text = min.ToString() + ":" + sec.ToString() + ":" + fraction.ToString().Substring(0,1);
        }

        if (animateWaveNameText)
        {
            StartCoroutine(TypeSentence(waves[nextWave].Name, waveNameText));
            animateWaveNameText = false;
        }
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("nextWave", nextWave);
    }

    IEnumerator TypeSentence(string sentence, Text txtObj)
    {
        txtObj.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            txtObj.text += letter;
            yield return new WaitForSeconds(.5f);
        }
    }

    bool EnemiesAreAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null && GameObject.FindGameObjectWithTag("deadFish") == null)
            {
                return false;
            }
        }
        return true;
    }

    private void DestroyAllArrows()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject arrow in arrows)
        {
            Destroy(arrow);
        }
    }

    private void DestroyAllJunk()
    {
        GameObject[] junks = GameObject.FindGameObjectsWithTag("junk");
        foreach (GameObject junk in junks)
        {
            Destroy(junk);
        }
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        DestroyAllArrows();
        DestroyAllJunk();
        ShopManager.instance._isAvailable = false;

        waveComplited = false;

        timeBetweenWavesAnime.SetBool("showTime", false);

        Debug.Log("Spawning wave: " + _wave.Name);
        StartCoroutine(TypeSentence(_wave.Name, waveNameText));

        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            // spawn enemy
            int randomEnemy = Random.Range(0, _wave.Enemies.Length);
            SpawnEnemy(_wave.Enemies[randomEnemy], _wave);
            numOfEnemies++;
            enemiesLeftText.text = numOfEnemies.ToString();
            ArcherController.instance.bLoadEnemies = true;
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform _enemy, Wave _WAVE_)
    {
        // spawn enemy
        Debug.Log("Spawning enemy" + _enemy.name);
        Transform _sp = null;
        if (_WAVE_.Name.Contains("BOSS"))
        {
            _sp = bossSpawnPoints[Random.Range(0, bossSpawnPoints.Length)];
        }
        else { _sp = spawnPoints[Random.Range(0, spawnPoints.Length)]; }

        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

    void WaveComplited()
    {
        Debug.Log("Wave Complited");
        state = SpawnState.COUNTING;
        timeBetweenWavesAnime.SetBool("showTime", true);
        waveComplitedAnime.SetTrigger("waveComplited");
        waveCountDown = timeBetweenWaves;
        if(nextWave+1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All waves complete! Looping..");
        }
        nextWave++;
        waveComplited = true;
        ArcherController.instance.bEnemiesLoaded = false;
        ShopManager.instance._isAvailable = true;
        StartCoroutine(TypeSentence("TILL NEXT WAVE", infoText));
        StartCoroutine(PlayerSuccess());
    }

    private IEnumerator PlayerSuccess()
    {
        playerVictorious = true;
        yield return new WaitForSeconds(3f);
        playerVictorious = false;
    }

    public void SkipTimeBetweenWaves()
    {
        waveCountDown = 5f;
    }

}
