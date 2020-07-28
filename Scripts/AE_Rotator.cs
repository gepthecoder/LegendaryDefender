using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_Rotator : MonoBehaviour {

    public Vector3 Rotation = new Vector3(1, 0, 0);
    Vector3 currentRotation; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    currentRotation += Rotation*Time.deltaTime;
        transform.rotation = Quaternion.Euler(currentRotation);
	}
}
