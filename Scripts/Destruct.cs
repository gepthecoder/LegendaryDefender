using UnityEngine;
using System.Collections;
using System;

public class Destruct : MonoBehaviour {

	//Add this script to the object you want to break and assign variables in the inspector (done by default in Prefabs folder)

	public GameObject fracturedPrefab;
	public GameObject smokeParticle;
	public AudioClip shatter;

    public int health;
    public int maxHealth = 100;

    public event Action<float> OnHealthPctChanged = delegate { };

    private PopUpController popUpController;

    GameObject smoke = null;

    [Range(5, 40)]
    public float impactForce;
    [Range(10, 60)]
    public float removeDebrisDelay;

    public GameObject child0;

    float takeHealthCoolDown = 0f;


    void Start() {
		if (fracturedPrefab == null) {
			Debug.LogError ("Fractured prefab not assigned for object: " + this.gameObject.name);
		}
        health = maxHealth;
        popUpController = GetComponent<PopUpController>();
	}

    void OnTriggerEnter(Collider other)
    {
        EnemyController enemyControls = other.transform.root.GetComponent<EnemyController>();
        if (enemyControls != null)
        {
            // enemy can attack fence
            enemyControls.canAttackFence = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        EnemyController enemyControls = other.transform.root.GetComponent<EnemyController>();
        if (enemyControls != null)
        {
            if(health <= 0) { enemyControls.canAttackFence = false; return; }
            takeHealthCoolDown -= Time.deltaTime;
            if(takeHealthCoolDown <= 0)
            {
                // enemy can attack fence
                DamageFence(enemyControls.Damage);
                takeHealthCoolDown = 2f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        EnemyController enemyControls = other.transform.root.GetComponent<EnemyController>();
        if (enemyControls != null)
        {
            // enemy can attack fence
            enemyControls.canAttackFence = false;
        }
    }

    public void DamageFence(int damageAmount)
    {
        bool _isDead = health <= 0;
        if (_isDead) return;

        if (!child0.activeSelf) { child0.SetActive(true); }

        popUpController.PlayPopUpAnime(damageAmount.ToString());
        ModifyHealth(damageAmount);

        if (health <= 0)
        {
            //die
            DestroyFence();
        }
    }

    public void ModifyHealth(int amount)
    {
        health -= amount;
        float currentHealthPct = (float)health / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);
    }

    private void DestroyFence()
    {
        BreakObject(this, transform.position, transform.position);
    }

    private void BreakObject(Destruct instance, Vector3 rayDirection, Vector3 hitPoint)
    {
        GameObject fractured = instance.fracturedPrefab;
        GameObject smokeEffect = instance.smokeParticle;
        AudioClip breakSound = instance.shatter;
        Vector3 pos = instance.transform.position;
        Quaternion rot = instance.transform.rotation;
        Destroy(instance.gameObject);
        if (smokeEffect)
        {
            Vector3 heightFix = new Vector3(pos.x, pos.y + 1f, pos.z);
            smoke = (GameObject)Instantiate(smokeEffect, heightFix, rot);
        }
        AudioSource.PlayClipAtPoint(breakSound, pos);
        GameObject go = (GameObject)Instantiate(fractured, pos, rot);
        go.name = "FracturedClone";
        foreach (Rigidbody rb in go.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForceAtPosition(rayDirection * impactForce, hitPoint, ForceMode.Impulse);
        }
        StartCoroutine(removeDebris(go, removeDebrisDelay));
        if (smoke)
        {
            Destroy(smoke, 2f);
        }
    }

    IEnumerator removeDebris(GameObject go, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(go);
    }
}
