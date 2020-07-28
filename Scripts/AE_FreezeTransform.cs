using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_FreezeTransform : MonoBehaviour {

    public bool FreezePosition = true;
    public bool FreezeRotation = true;
    public float TimeDelay = 0;

    Vector3 startPos;
    Vector3 startRotation;
    Vector3 lastPos;
    Vector3 lastRotation;

    float currentTime;

    void Awake () {
        startPos = transform.localPosition;
        startRotation = transform.localRotation.eulerAngles;
	}


    private void OnEnable()
    {
        transform.localPosition = startPos;
        transform.localRotation = Quaternion.Euler(startRotation);
        currentTime = Time.time;
    }

    void LateUpdate () {
        if (Mathf.Abs(Time.time - currentTime) > TimeDelay)
        {
            if (FreezePosition)
                transform.position = lastPos;
            if (FreezeRotation)
                transform.rotation = Quaternion.Euler(lastRotation);
        }
        else
        {
            lastPos = transform.position;
            lastRotation = transform.rotation.eulerAngles + startRotation;
        }
	}
}
