using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_RelativeTransform : MonoBehaviour
{
    public bool RelativePosition = true;
    //public bool RelativeRotation;
    public bool RelativeScale;

    bool isInitialized;

    void OnEnable()
    {
        isInitialized = false;
    }

    void Update()
    {
        if (isInitialized) return;
        isInitialized = true;

        var lossy = transform.lossyScale;
        if (RelativePosition)
        {
            var pos = transform.localPosition;
            transform.localPosition = new Vector3(pos.x / lossy.x, pos.y / lossy.y, pos.z / lossy.z);
        }
        if(RelativeScale)
        {
            transform.localScale = new Vector3(1f / lossy.x, 1f / lossy.y, 1f / lossy.z);
        }
    }

}
