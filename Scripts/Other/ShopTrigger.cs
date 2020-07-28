using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ShopTrigger : MonoBehaviour
{
    public static ShopTrigger instance;

    public PlayableDirector director;
    public GameObject GUI_Controls;

    public float timer;

    void Awaik()
    {
        instance = this;
    }

    void Update()
    {
        timer -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && timer <= 0f)
        {
            GUI_Controls.SetActive(false);
            director.Play();
            StartCoroutine(PlayerIsInShopDelay(other));

        }
    }

    IEnumerator PlayerIsInShopDelay(Collider other)
    {
        yield return new WaitForSeconds(.3f);
        other.gameObject.GetComponent<Controller>()._inShop = true;
    }
}
