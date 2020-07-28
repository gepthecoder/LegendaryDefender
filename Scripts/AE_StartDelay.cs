using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_StartDelay : MonoBehaviour {

    public GameObject ActivatedGameObject;
    public float Delay = 1;

    private float currentTime = 0;
    private bool isEnabled;

    // Use this for initialization
    void OnEnable()
    {
        ActivatedGameObject.SetActive(false);
        isEnabled = false;
        // Invoke("ActivateGO", Delay);
        currentTime = Time.time;
    }

    void Update()
    {
        if ((Time.time - currentTime) >= Delay && !isEnabled)
        {
            isEnabled = true;
            ActivatedGameObject.SetActive(true);
        }
    }
}
