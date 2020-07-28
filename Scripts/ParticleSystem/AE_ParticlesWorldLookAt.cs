using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_ParticlesWorldLookAt : MonoBehaviour
{
    ParticleSystem.MainModule main;
    private Vector3 startRotation;
    private Vector3 prevPos;

    private const float rad = 57.29578f;


    // Use this for initialization
    void Start ()
    {
        prevPos = transform.position;
        main = GetComponent<ParticleSystem>().main;
	    main.startRotation3D = true;

        startRotation.x = main.startRotationX.constant * rad;
	    startRotation.y = main.startRotationY.constant * rad;
	    startRotation.z = main.startRotationZ.constant * rad;
    }

    public Vector3 test;
	// Update is called once per frame
	void Update ()
	{
	    var dir = (transform.position - prevPos);
	    if (dir.magnitude < 0.0001f) return;
	    prevPos = transform.position;

        var roatation = Quaternion.LookRotation(dir).eulerAngles;
	    main.startRotationX = (roatation.x + startRotation.x * 0) / rad; //todo some bug with radian and correct start rotation?
	    main.startRotationY = (roatation.y + startRotation.y * 0) / rad;
        main.startRotationZ = (roatation.z + startRotation.z * 0) / rad;
    }
}
