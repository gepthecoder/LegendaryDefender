using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenTrigger : MonoBehaviour
{
 
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PRINCESS")
        {
            QueenLogic.instance.canGoToDestination = false;
            QueenLogic.instance.FaceTarget(false);
            QueenLogic.instance.reachedDestination = true;
        }
    }
}
