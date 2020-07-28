using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_AudioEnableByRandom : MonoBehaviour
{ 
    public float EnableChance = 0.5f;

    void OnEnable()
    {
        if (Random.value < EnableChance)
        {
            GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            GetComponent<AudioSource>().enabled = false;
        }
    }
}