using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    public static Collectables instance;

    public GameObject collectableItem__Bomb;
    public GameObject collectableItem__Coin;
    public GameObject collectableItem__Diamond;
    public GameObject collectableItem__Heart;
    public GameObject collectableItem__Key;
    public GameObject collectableItem__Shield;
    public GameObject collectableItem__Axe;

    void Awake()
    {
        instance = this;
    }


    // SPAWN OBJECTS
    public void SPAWN_BOMB(Transform spawnPos) { Instantiate(collectableItem__Bomb, spawnPos.position + offsetY, Quaternion.identity); }

    public void SPAWN_COIN(Transform spawnPos) { Instantiate(collectableItem__Coin, spawnPos.position + offsetY, Quaternion.identity); }

    public void SPAWN_DIAMOND(Transform spawnPos) { Instantiate(collectableItem__Diamond, spawnPos.position + offsetY, Quaternion.identity); }

    public void SPAWN_HEART(Transform spawnPos) { Instantiate(collectableItem__Heart, spawnPos.position + offsetY, Quaternion.identity); }

    public void SPAWN_KEY(Transform spawnPos) { Instantiate(collectableItem__Key, spawnPos.position + offsetY, Quaternion.identity); }

    public void SPAWN_SHIELD(Transform spawnPos) { Instantiate(collectableItem__Shield, spawnPos.position + offsetY, Quaternion.identity); }

    public void SPAWN_AXE(Transform spawnPos) { Instantiate(collectableItem__Axe, spawnPos.position + offsetY, Quaternion.identity); }
    //

    private Vector3 offsetY = new Vector3(0, 1.5f, 0);
    public void SPAWN_RANDOM_OBJECT_LOWREWARD(Transform spawnPos)
    {
        int random = Random.Range(0, 540);

        if(random >= 0 && random < 20) { SPAWN_AXE(spawnPos); }
        else if(random >= 20 && random < 400) { SPAWN_COIN(spawnPos); }
        else if (random >= 400 && random < 450) { SPAWN_HEART(spawnPos); }
        else if (random >= 450 && random < 500) { SPAWN_BOMB(spawnPos); }
        else if (random >= 500 && random <= 540) { SPAWN_SHIELD(spawnPos); }
    }

    public void SPAWN_RANDOM_OBJECT_HIGHREWARD(Transform spawnPos)
    {
        int random = Random.Range(0, 100);

        if (random >= 0 && random < 50) { SPAWN_KEY(spawnPos); }
        else { SPAWN_DIAMOND(spawnPos); }
    }
}
